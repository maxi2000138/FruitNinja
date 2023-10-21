    using System.Collections.Generic;
    using CartoonFX;
    using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ParticleFeatures
{
    public class ParticleSystemPlayer : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> _sliceFruitParticles;
        [SerializeField] private List<ParticleSystem> _sliceBombParticles;
        [SerializeField] private List<ParticleSystem> _sliceHeartParticles;
        [SerializeField] private CFXR_Effect _cfxrEffect;

        
        
        public void PlayHeartParticles(Vector2 position)
        {
            SetPosition(_sliceHeartParticles, position);
            PlayParticles(_sliceHeartParticles);
        }
        
        public void PlayBombSliceParticles(Vector2 position)
        {
            _cfxrEffect.enabled = false;
            _cfxrEffect.enabled = true;
            SetPosition(_sliceBombParticles, position);
            PlayParticles(_sliceBombParticles);
        }

        public void PlayFruitSliceParticles(Vector2 position, Color color)
        {
            SetPosition(_sliceFruitParticles, position);
            SetColor(_sliceFruitParticles, color);
            PlayParticles(_sliceFruitParticles);
        }

        private void PlayParticles(List<ParticleSystem> particles)
        {
            particles[0].Play(true);
        }
        private void SetColor(List<ParticleSystem> particleSystems, Color color)
        {
            for (int i = 0; i < particleSystems.Count; i++)
            {
                ParticleSystem.MainModule mainModule = particleSystems[i].main;
                mainModule.startColor = color;
            }
        }

        private void SetPosition(List<ParticleSystem> particleSystems, Vector2 position)
        {
            particleSystems[0].transform.position = position;
        }
    }
}
