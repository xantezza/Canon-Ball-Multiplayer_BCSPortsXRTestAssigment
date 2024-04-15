using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services;
using Mirror;
using Mirror.Examples.NetworkRoom;
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

        private Vector3 _prevMousePosition;

        private float _cooldownLeft;

        [SyncVar]
        [SerializeField] private PlayerData _playerData = new();

        [SyncVar]
        [SerializeField] private int _score;

        public void Start()
        {
            _camera.gameObject.SetActive(isOwned);
            
            if (!isOwned) return;
            CmdUpdateColor(NetworkRoomPlayerExt.OwnedPlayerData);
            Cursor.lockState = CursorLockMode.Locked;
        }

        [SyncVar(hook = nameof(SetColorHook))]
        public Color32 _color = Color.black;

        void SetColorHook(Color32 _, Color32 newColor)
        {
            SetColor(newColor);
        }

        [Command]
        private void CmdUpdateColor(PlayerData playerData)
        {
            _playerData = playerData;
            _color = playerData.PlayerColor;
        }
        
        private void SetColor(Color color)
        {
            var newMaterial = new Material(_materialToCopy)
            {
                color = color
            };

            _meshes.ForEach(x => x.material = newMaterial);
        }

        [ContextMenu("Gather Meshes")]
        private void GatherMeshes()
        {
            _meshes = GetComponentsInChildren<MeshRenderer>().ToList();
        }

        private void Update()
        {
            if (isServer || isServerOnly)
            {
                UpdateCooldown();
            }

            if (isOwned)
            {
                UpdateCooldown();
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
            CmdShoot();
            Shoot();
            _cooldownLeft = _shotCooldownInSeconds;
        }

        [Command]
        private void CmdShoot()
        {
            if (_cooldownLeft > 0f) return;
            _cooldownLeft = _shotCooldownInSeconds;
            RpcFireWeapon();
        }

        private void Shoot()
        {
            var bullet = Instantiate(_soccerBallPrefab, _ballSpawnPoint.position, _ballSpawnPoint.rotation);
            bullet.Init(this, _shotImpulseMultiplier);
        }

        [ClientRpc(includeOwner = false)]
        private void RpcFireWeapon()
        {
            var bullet = Instantiate(_soccerBallPrefab, _ballSpawnPoint.position, _ballSpawnPoint.rotation);
            bullet.Init(this, _shotImpulseMultiplier);
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

        public void AddScore()
        {
            CmdAddScore();
        }

        [Command]
        private void CmdAddScore()
        {
            _score++;
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect((_playerData.Index * 110), 200, 100, 130f));

            GUILayout.Label($"{_playerData.Name} ({_playerData.PlayerColorEnum}): {_score}");

            GUILayout.EndArea();
        }
    }
}