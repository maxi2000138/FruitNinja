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
                for (int j = 0; j < _destroyListeners[i].Length; j++)
                {
                    if(IsAnyTriggered(_destroyListeners[i]))
                        RemoveGroup(_destroyListeners[i]);
                }
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

        private void RemoveGroup(Transform[] destroyListeners)
        {
            for (int i = 0; i < destroyListeners.Length; i++)
            {
                if (destroyListeners[i] != null)
                {
                    _projectileDestroyer.DestroyProjectile(destroyListeners[i].gameObject);
                }
            }
        }
    }
}