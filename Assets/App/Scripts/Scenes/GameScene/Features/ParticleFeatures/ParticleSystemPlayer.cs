using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ParticleFeatures
{
    public class ParticleSystemPlayer : SerializedMonoBehaviour
    {
        [OdinSerialize] private Dictionary<ProjectileType, ParticleSystem> _projectileParticles;
        
        private TokenController _tokenController;
        private BonusesConfig _bonusesConfig;


        private void Awake()
        {
            _tokenController = new TokenController();
        }

        public void PlayHeartParticles(Vector2 position)
        {
            PlayParticles(_projectileParticles[ProjectileType.Heart], position);
        }
        
        public void PlayBombSliceParticles(Vector2 position)
        {
            //_cfxrEffect.enabled = false;
            //_cfxrEffect.enabled = true;
            PlayParticles(_projectileParticles[ProjectileType.Bomb], position);
        }

        public void PlayFruitSliceParticles(Vector2 position, Color color)
        {
            ParticleSystem particles = PlayParticles(_projectileParticles[ProjectileType.Fruit], position);
            SetColor(particles, color);
        }

        public async void PlayMagnetSliceParticlesTime(Vector2 position, int milisecPlayTime)
        {
            ParticleSystem particles = PlayParticles(_projectileParticles[ProjectileType.Magnet], position);
            await UniTask.Delay(milisecPlayTime, false, PlayerLoopTiming.Update, _tokenController.CreateCancellationToken());
            StopParticles(particles);
        }

        private ParticleSystem PlayParticles(ParticleSystem particles, Vector2 position) => 
            Instantiate(particles, position, Quaternion.identity, transform);

        private void StopParticles(ParticleSystem particles) => 
            Destroy(particles.gameObject);
        private void StopParticles(List<ParticleSystem> particles)
        {
            particles[0].Stop(true);
        }

        
        private void SetColor(ParticleSystem particleSystems, Color color)
        {
            ParticleSystem[] componentsInChildren = particleSystems.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                ParticleSystem.MainModule mainModule = componentsInChildren[i].main;
                mainModule.startColor = color;
            }
        }
    }
}
