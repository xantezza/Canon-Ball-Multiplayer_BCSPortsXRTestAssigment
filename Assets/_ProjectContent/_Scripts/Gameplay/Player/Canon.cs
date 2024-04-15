using System.Collections.Generic;
using System.Linq;
using Infrastructure.Factories;
using Infrastructure.Services;
using Mirror;
using MirrorRoom;
using UnityEngine;
using Utils.Extensions;
using Zenject;

namespace Gameplay.Player
{
    public class Canon : NetworkBehaviour
    {
        [SerializeField] private Transform _canonAimLeftRightRoot;
        [SerializeField] private Transform _canonAimUpDownRoot;
        [SerializeField] private Transform _ballSpawnPoint;
        [SerializeField] private Camera _camera;

        [SerializeField] private float _shotImpulseMultiplier;
        [SerializeField] private float _shotCooldownInSeconds;

        [SerializeField] private float _aimXSpeedMultiplier;
        [SerializeField] private float _aimYSpeedMultiplier;

        [SerializeField] private float _clampLeftRightAngle;
        [SerializeField] private float _clampUpDownAngle;

        [SerializeField] private SoccerBall.SoccerBall _soccerBallPrefab;
        [SerializeField] private Material _materialToCopy;
        [SerializeField] private List<MeshRenderer> _meshes;

        [SyncVar] [SerializeField] private PlayerData _playerData = new();

        [SyncVar] [SerializeField] private int _score;

        [SyncVar(hook = nameof(SetColorHook))]
        public Color32 _color = Color.black;

        private Vector3 _prevMousePosition;

        private float _cooldownLeft;

        private SoccerBallFactoryMirror _ballFactory;

        [Inject]
        private void Inject(SoccerBallFactoryMirror ballFactory)
        {
            _ballFactory = ballFactory;
        }

        public void Start()
        {
            _camera.gameObject.SetActive(isOwned);

            if (!isOwned) return;
            CmdUpdatePlayerData(NetworkRoomPlayerExt.OwnedPlayerData);
            Cursor.lockState = CursorLockMode.Locked;
        }

        void SetColorHook(Color32 _, Color32 newColor)
        {
            SetColor(newColor);
        }

        private void SetColor(Color color)
        {
            var newMaterial = new Material(_materialToCopy)
            {
                color = color
            };

            _meshes.ForEach(x => x.material = newMaterial);
        }

        private void Update()
        {
            UpdateCooldown();

            if (isOwned)
            {
                UpdateInput();
            }
        }

        private void UpdateInput()
        {
            var newAxisInput = new Vector2(
                                   Input.GetAxis("Mouse X") * _aimXSpeedMultiplier,
                                   -Input.GetAxis("Mouse Y") * _aimYSpeedMultiplier
                               )
                               * Time.unscaledDeltaTime
                               * 60;

            var newY = _canonAimLeftRightRoot.localRotation.eulerAngles.y.UnwrapAngle() + newAxisInput.x;
            newY = Mathf.Clamp(newY, -_clampLeftRightAngle, _clampLeftRightAngle);

            var newX = _canonAimUpDownRoot.localEulerAngles.x.UnwrapAngle() + newAxisInput.y;
            newX = Mathf.Clamp(newX, -_clampUpDownAngle, _clampUpDownAngle);

            _canonAimLeftRightRoot.localEulerAngles = new Vector3(0, newY, 0f);
            _canonAimUpDownRoot.localEulerAngles = new Vector3(newX, 0, 0f);

            if (Input.GetMouseButtonDown(0))
            {
                TryShoot();
            }

            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        private void TryShoot()
        {
            if (_cooldownLeft > 0f) return;
            Shoot();
            if (!isServer) CmdShoot();
        }

        private void Shoot()
        {
            _cooldownLeft = _shotCooldownInSeconds;

            _ballFactory.Create(_ballSpawnPoint.position, _ballSpawnPoint.rotation).Init(this, _shotImpulseMultiplier);
            

            if (isServer) RpcFireWeapon();
        }

        private void UpdateCooldown()
        {
            _cooldownLeft -= Time.deltaTime;
            Mathf.Clamp(_cooldownLeft, 0, float.MaxValue);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (isOwned && hasFocus) Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect((_playerData.Index * 130), 150, 120, 130f));

            GUILayout.Label($"{_playerData.Name} ({_playerData.PlayerColorEnum}): {_score}");

            GUILayout.EndArea();
        }

        [Server]
        public void AddScore()
        {
            _score++;
        }

        [Command]
        private void CmdShoot()
        {
            if (_cooldownLeft > 0f) return;
            _cooldownLeft = _shotCooldownInSeconds;
            RpcFireWeapon();
        }

        [Command]
        private void CmdUpdatePlayerData(PlayerData playerData)
        {
            _playerData = playerData;
            _color = playerData.PlayerColor;
        }

        [ClientRpc(includeOwner = false)]
        private void RpcFireWeapon()
        {
            _ballFactory.Create(_ballSpawnPoint.position, _ballSpawnPoint.rotation).Init(this, _shotImpulseMultiplier);
        }

        [ContextMenu("Gather Meshes")]
        private void GatherMeshes()
        {
            _meshes = GetComponentsInChildren<MeshRenderer>().ToList();
        }
    }
}