using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.GameScene.Features.CameraFeatures.ScreenSettingsProvider;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileDestroyer.ProjectileDestroyer;
using App.Scripts.Scenes.Infrastructure.MonoInterfaces;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileDestroyer.DestroyTrigger
{
    public class DestroyTrigger : IInitializable, IUpdatable, IDestroyTrigger
    {
        public int DestroyGroupCount => _destroyListeners.Count;
        
        private float _yDestroyValue;
        private readonly IScreenSettingsProvider _screenSettingsProvider;
        private readonly ProjectileContainer _projectileContainer;
        private readonly IProjectileDestroyer _projectileDestroyer;
        private readonly ShootConfig _shootConfig;
        private readonly List<ProjectileObject> _destroyListeners = new ();
        
        public DestroyTrigger(IScreenSettingsProvider screenSettingsProvider, ProjectileContainer projectileContainer, IProjectileDestroyer projectileDestroyer, ShootConfig shootConfig)
        {
            _screenSettingsProvider = screenSettingsProvider;
            _projectileContainer = projectileContainer;
            _projectileDestroyer = projectileDestroyer;
            _shootConfig = shootConfig;
        }
    
        public void AddDestroyTriggerListeners(ProjectileObject projectileObject)
        {
            _destroyListeners.Add(projectileObject);
        }

        public void Initialize()
        {
            Vector2 zeroCameraPoint = _screenSettingsProvider.ViewportToWorldPosition(Vector2.zero);
            _yDestroyValue = zeroCameraPoint.y + _shootConfig.DestroyTriggerOffset;
        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < _destroyListeners.Count; i++)
            {
                if (IsTriggered(_destroyListeners[i]))
                {
                    RemoveGroup(_destroyListeners[i]);
                    i--;
                }
            }
        }


        public void TriggerGroup(ProjectileObject projectileObject)
        {
            if(_destroyListeners.Contains(projectileObject))
                RemoveGroup(projectileObject);
        }
        private bool IsTriggered(ProjectileObject destroyListener)
        {
            if (destroyListener != null && destroyListener.transform.position.y <= _yDestroyValue)
            {
                return true;
            }

            return false;
        }

        private void RemoveGroup(ProjectileObject projectileObject)
        {
            _destroyListeners.Remove(projectileObject);
            _projectileContainer.RemoveFromDictionary(projectileObject);
            
            if (projectileObject != null)
            {
                _projectileDestroyer.DestroyProjectiles(projectileObject.ProjectileGameObjects());
            }
        }
    }
}