using System;
using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.GameScene.Features.CameraFeatures.ScreenSettingsProvider;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ColliderFeatures;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.InputFeatures
{
    public class Slicer : MonoBehaviour, ILooseGameListener, IRestartGameListener
    {
        public event Action<Vector2, ProjectileType> OnSlice;
        
        [SerializeField] private TrailRenderer _trailRenderer;
        [SerializeField] private float _zPosition;

        private SliceCollidersController _sliceCollidersController;
        private ScreenSettingsProvider _screenSettingsProvider;
        private ShootConfig _shootConfig;
        private InputReader _inputReader;
        private Coroutine _sliceCoroutine;
        private Vector2 _lastWorldPosition;
        private bool _isSlicing;

        public void Construct(InputReader inputReader, ScreenSettingsProvider screenSettingsProvider, SliceCollidersController sliceCollidersController, ShootConfig shootConfig)
        {
            _shootConfig = shootConfig;
            _sliceCollidersController = sliceCollidersController;
            _screenSettingsProvider = screenSettingsProvider;
            _inputReader = inputReader;
            Enable();
        }

        private void Update()
        {
            if(!_isSlicing)
                return;
        
            Vector3 worldPosition = _screenSettingsProvider.ScreenToWorldPosition(_inputReader.TouchPosition);
            worldPosition.z = _zPosition;
        
            if (_sliceCollidersController.TryGetIntersectionCollider(worldPosition, out Mover forceMover, out SliceCircleCollider collider))
            {
                collider.SliceObject.Slice(forceMover, _shootConfig.SliceForce, out bool disableColliderOnSlice);
                if(disableColliderOnSlice)
                    collider.Disable();
                OnSlice?.Invoke(worldPosition, collider.SliceObject.ProjectileType);
            }
        
            _trailRenderer.transform.position = worldPosition;
            _lastWorldPosition = worldPosition;
        }

        private void OnDestroy()
        {
            Disable();
        }

        public void OnRestartGame()
        {
            Enable();
        }
        
        public void OnLooseGame()
        {
            Disable();
        }
        
        public void StartSlicing()
        {
            _trailRenderer.Clear();
            _trailRenderer.enabled = true;
            _isSlicing = true;
        }

        public void EndSlicing()
        {
            _isSlicing = false;
            _trailRenderer.enabled = false;
        }

        private void Enable()
        {
            _inputReader.SliceStartedEvent += StartSlicing;
            _inputReader.SliceEndedEvent += EndSlicing;
        }

        private void Disable()
        {
            _inputReader.SliceStartedEvent -= StartSlicing;
            _inputReader.SliceEndedEvent -= EndSlicing;
            EndSlicing();
        }

    }
}
