using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ParticleFeatures
{
    public class ParticleSystemPlayer : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> _particleSystems;

        public void SetColorAndPosition(Vector2 position, Color color)
        {
            for (int i = 0; i < _particleSystems.Count; i++)
            {
                ParticleSystem.MainModule mainModule = _particleSystems[i].main;
                _particleSystems[i].transform.position = position;
                mainModule.startColor = color;
            }
        }

        public void PlayAll(Vector2 position, Color color)
        {
            SetColorAndPosition(position, color);
            
            for (int i = 0; i < _particleSystems.Count; i++) 
                _particleSystems[i].Play();
        }
    }
}
