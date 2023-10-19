using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.GameScene.Features.CameraFeatures.ScreenSettingsProvider;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileDestroyer.ProjectileDestroyer;
using App.Scripts.Scenes.Infrastructure.MonoInterfaces;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileDestroyer.DestroyTrigger
{
    public class DestroyTrigger : IInitializable, IUpdatable, IDestroyTrigger
    {
        public int DestroyGropCount =>
            _destroyListeners.Count;
        
        private float _yDestroyValue;
        private readonly IScreenSettingsProvider _screenSettingsProvider;
        private readonly IProjectileDestroyer _projectileDestroyer;
        private readonly ProjectileConfig _projectileConfig;
        private readonly List<Transform[]> _destroyListeners = new ();
        
        public DestroyTrigger(IScreenSettingsProvider screenSettingsProvider, IProjectileDestroyer projectileDestroyer, ProjectileConfig projectileConfig)
        {
            _screenSettingsProvider = screenSettingsProvider;
            _projectileDestroyer = projectileDestroyer;
            _projectileConfig = projectileConfig;
        }
    
        public void AddDestroyTriggerListeners(params Transform[] objectTransforms)
        {
            _destroyListeners.Add(objectTransforms);
        }

        public void Initialize()
        {
            Vector2 zeroCameraPoint = _screenSettingsProvider.ViewportToWorldPosition(Vector2.zero);
            _yDestroyValue = zeroCameraPoint.y + _projectileConfig.DestroyTriggerOffset;
        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < _destroyListeners.Count; i++)
            {
                if (IsAnyTriggered(_destroyListeners[i]))
                {
                    RemoveGroupAt(i);
                    i--;
                }
            }
        }


        public void TriggerGroup(Transform destroyTransform)
        {
            for (int i = 0; i < _destroyListeners.Count; i++)
            {
                if (_destroyListeners[i].Contains(destroyTransform))
                    RemoveGroupAt(i);
            }
        }
        private bool IsAnyTriggered(Transform[] destroyListeners)
        {
            for (int i = 0; i < destroyListeners.Length; i++)
            {
                if (destroyListeners[i] != null && destroyListeners[i].position.y <= _yDestroyValue)
                {
                    return true;
                }
            }

            return false;
        }

        private void RemoveGroupAt(int index)
        {
            for (int i = 0; i < _destroyListeners[index].Length; i++)
            {
                Transform destroyListener = _destroyListeners[index][i];
                if (destroyListener != null)
                {
                    _projectileDestroyer.DestroyProjectile(destroyListener.gameObject);
                }
            }
            
            _destroyListeners.RemoveAt(index);
        }
    }
}