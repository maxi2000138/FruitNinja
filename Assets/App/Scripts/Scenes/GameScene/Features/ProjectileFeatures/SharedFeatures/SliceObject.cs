using App.Scripts.Scenes.GameScene.Features.ParticleFeatures;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.SharedFeatures
{
    public class SliceObject : MonoBehaviour, ISliceable
    {
        private ISliced _leftObject;
        private ISliced _rightObject;
        [field: SerializeField] public ParticleSystemController ParticleSystemController { get; private set; }

        public void Construct(ISliced leftObject, ISliced rightObject)
        {
            _leftObject = leftObject;
            _rightObject = rightObject;
        }

        public SliceData OnSlice()
        {
            return new SliceData(transform.position, transform.eulerAngles, transform.localScale);
        }

        public void Slice(Vector2 slicePoint, Vector2 sliceVector, float sliceForce)
        {
            PlayParticles();
            DestroyFruit();
            SetSliceParts(OnSlice());
        }

        private void SetSliceParts(SliceData sliceData)
        {
            _leftObject.Transform.position = sliceData.Position;
            _leftObject.Transform.eulerAngles = sliceData.Rotation;
            _leftObject.Transform.localScale = sliceData.Scale;
            _rightObject.Transform.position = sliceData.Position;
            _rightObject.Transform.eulerAngles = sliceData.Rotation;
            _rightObject.Transform.localScale = sliceData.Scale;
        }

        private void DestroyFruit()
        {
            
        }

        private void PlayParticles()
        {
            ParticleSystemController.PlayAll();
        }
    }
}