using System;
using App.Scripts.Scenes.GameScene.Features.ParticleFeatures;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Fruit
{
    public class Fruit : MonoBehaviour, ISlicable
    {
        public event Action DestroyNotSliced;
        
        private ParticleSystemPlayer _particleSystemPlayer;
        private Color _sliceColor;
        private bool _sliced;

        public void Construct(Color sliceColor, ParticleSystemPlayer particleSystemPlayer)
        {
            _sliceColor = sliceColor;
            _particleSystemPlayer = particleSystemPlayer;
        }

        public void OnSlice()
        {
            _sliced = true;
            _particleSystemPlayer.PlayFruitSliceParticles(transform.position, _sliceColor);   
        }
        
        private void OnDestroy()
        {
            if(!_sliced)
                DestroyNotSliced?.Invoke();
        }
    }
}