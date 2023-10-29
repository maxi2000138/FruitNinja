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
        [Header("Mimik")]
        [SerializeField] private ParticleSystem _mimikBeforeChagneParticles;
        [SerializeField] private ParticleSystem _mimikAfterChangeParticles;
        
        private TokenController _tokenController;
        private BonusesConfig _bonusesConfig;


        private void Awake()
        {
            _tokenController = new TokenController();
        }

        public void PlayMimikBeforeChangeAndParent(Transform parent)
        {
            ParticleSystem playParticles = PlayParticles(_mimikBeforeChagneParticles, parent.transform.position);
            playParticles.transform.SetParent(parent);
        }
        
        public void PlayMimikAfterChange(Transform parent)
        {
            ParticleSystem particles = PlayParticles(_mimikAfterChangeParticles, parent.position);
            particles.GetComponent<PositionController>().SetControllingTransform(parent);
        }
        
        public void PlayMimikDefaultAndParent(Transform parent)
        {
            ParticleSystem playParticles = PlayParticles(_projectileParticles[ProjectileType.Mimik], parent.transform.position);
            playParticles.transform.SetParent(parent);
        }
        
        public void PlaySamuraiParticles(Transform parent)
        {
            ParticleSystem particles = PlayParticles(_projectileParticles[ProjectileType.Samurai], parent.position);
            particles.GetComponent<PositionController>().SetControllingTransform(parent);
        }

        public void PlayIceParticles(Vector2 position) =>
            PlayParticles(_projectileParticles[ProjectileType.Ice], position);
        public void PlayBrickParticles(Vector2 position) =>
            PlayParticles(_projectileParticles[ProjectileType.Brick], position);
        public void PlayHeartParticles(Vector2 position) => 
            PlayParticles(_projectileParticles[ProjectileType.Heart], position);

        public void PlayBombSliceParticles(Vector2 position) => 
            PlayParticles(_projectileParticles[ProjectileType.Bomb], position);

        public void PlayFruitSliceParticles(Vector2 position, Color color)
        {
            ParticleSystem particles = PlayParticles(_projectileParticles[ProjectileType.Fruit], position);
            SetColor(particles, color);
        }

        public GameObject PlayMagnetSliceParticlesTime(Vector2 position)
        {
            return PlayParticles(_projectileParticles[ProjectileType.Magnet], position).gameObject;
        }

        private ParticleSystem PlayParticles(ParticleSystem particles, Vector2 position) => 
            Instantiate(particles, new Vector3(position.x, position.y, 0f), Quaternion.identity, transform);

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
