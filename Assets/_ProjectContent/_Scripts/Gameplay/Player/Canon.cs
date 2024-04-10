using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services;
using Mirror;
using UnityEngine;
using Utils.Extensions;
using Zenject;

namespace Gameplay.Player
{
    public class Canon : NetworkBehaviour
    {
        [SerializeField] private Transform _canonAimLeftRightRoot;
        [SerializeField] private Transform _canonAimUpDownRoot;
        [SerializeField] private Camera _camera;

        [SerializeField] private float _aimXSpeedMultiplier;
        [SerializeField] private float _aimYSpeedMultiplier;

        [SerializeField] private float _clampLeftRightAngle;
        [SerializeField] private float _clampUpDownAngle;

        [SerializeField] private Material _materialToCopy;
        [SerializeField] private List<MeshRenderer> _meshes;

        private Vector3 _prevMousePosition;
        private PlayerColorTracker _playerColorTracker;

        [Inject]
        private void Inject(PlayerColorTracker playerColorTracker)
        {
            _playerColorTracker = playerColorTracker;
        }

        public void Start()
        {
            _camera.gameObject.SetActive(isOwned);


            if (!isOwned) return;

            _color = _playerColorTracker.PlayerColor;
            SetColor(_color);
            Cursor.lockState = CursorLockMode.Locked;
        }

        [SyncVar(hook = nameof(SetColorHook))]
        public Color32 _color = Color.black;

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

        [ContextMenu("Gather Meshes")]
        private void GatherMeshes()
        {
            _meshes = GetComponentsInChildren<MeshRenderer>().ToList();
        }

        private void Update()
        {
            if (!isOwned) return;

            UpdateInput();
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

            if (Input.GetKeyDown(KeyCode.LeftAlt)) Cursor.lockState = CursorLockMode.None;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (isOwned && hasFocus) Cursor.lockState = CursorLockMode.Locked;
        }
    }
}