using System;
using App.Scripts.Scenes.GameScene.Features.ParticleFeatures;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileDestroyer.DestroyTrigger;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.SharedFeatures
{
    public class SliceObject : MonoBehaviour
    {
        [SerializeField]
        private Transform _leftPartTransform;
        [SerializeField]
        private Transform _rightsPartTransform;
        
        private ParticleSystemPlayer _particleSystemPlayer;
        private IDestroyTrigger _destroyTrigger;
        private ISliced _leftObject;
        private ISliced _rightObject;
        
        private Func<ISliced> _leftPartSpawnMethod;
        private Func<ISliced> _rightPartSpawnMethod;

        public void Construct(Func<ISliced> leftPartSpawnMethod, Func<ISliced> rightPartSpawnMethod, ParticleSystemPlayer particleSystemPlayer, IDestroyTrigger destroyTrigger)
        {
            _particleSystemPlayer = particleSystemPlayer;
            _destroyTrigger = destroyTrigger;
            _leftPartSpawnMethod = leftPartSpawnMethod;
            _rightPartSpawnMethod = rightPartSpawnMethod;
        }

        public void Slice(Mover mover, Vector2 worldPosition, float sliceForce)
        {
            PlayParticles(worldPosition, Color.white);
            SpawnParts(); 
            SetupSlicedPart(_leftObject,_leftPartTransform.position, transform.eulerAngles, transform.localScale);
            SetupSlicedPart(_rightObject ,_rightsPartTransform.position, transform.eulerAngles, transform.localScale);
            SetVelocity(mover, sliceForce);
            DestroyFruit();
        }

        private void SpawnParts()
        {
            _leftObject = _leftPartSpawnMethod?.Invoke();
            _rightObject = _rightPartSpawnMethod?.Invoke();
        }

        private void SetupSlicedPart(ISliced leftObject, Vector2 position, Vector3 rotation, Vector2 scale)
        {
            leftObject.Transform.position = position;
            leftObject.Transform.eulerAngles = rotation;
            leftObject.Transform.localScale = scale;
            leftObject.Transform.gameObject.SetActive(true);
        }
        
        private void DestroyFruit()
        {
            _destroyTrigger.TriggerGroup(transform);
        }

        private void SetVelocity(Mover mover, float sliceForce)
        {
            float rotationZ = (transform.eulerAngles.z % 360f);

            if ((rotationZ < 90f || rotationZ > 270f) && mover.MovementVector.y > 0f)
            {
                _leftObject.VelocityApplier.AddVelocity(Quaternion.AngleAxis(-15f, new Vector3(0f, 0f, -1f)) *
                                                        mover.MovementVector * sliceForce);
                _rightObject.VelocityApplier.AddVelocity(Quaternion.AngleAxis(15f, new Vector3(0f, 0f, -1f)) *
                                                         mover.MovementVector * sliceForce);
            }
            else if (mover.MovementVector.y > 0f)
            {
                _leftObject.VelocityApplier.AddVelocity(Quaternion.AngleAxis(15f, new Vector3(0f, 0f, -1f)) *
                                                        mover.MovementVector * sliceForce);
                _rightObject.VelocityApplier.AddVelocity(Quaternion.AngleAxis(-15f, new Vector3(0f, 0f, -1f)) *
                                                         mover.MovementVector * sliceForce);
            }
            else if ((rotationZ < 90f || rotationZ > 270f) && mover.MovementVector.y <= 0f)
            {
                _leftObject.VelocityApplier.AddVelocity(Quaternion.AngleAxis(15f, new Vector3(0f, 0f, -1f)) *
                                                        mover.MovementVector * sliceForce);
                _rightObject.VelocityApplier.AddVelocity(Quaternion.AngleAxis(-15f, new Vector3(0f, 0f, -1f)) *
                                                         mover.MovementVector * sliceForce);
            }
            else if (mover.MovementVector.y <= 0f)
            {
                _leftObject.VelocityApplier.AddVelocity(Quaternion.AngleAxis(-15f, new Vector3(0f, 0f, -1f)) *
                                                        mover.MovementVector * sliceForce);
                _rightObject.VelocityApplier.AddVelocity(Quaternion.AngleAxis(15f, new Vector3(0f, 0f, -1f)) *
                                                         mover.MovementVector * sliceForce);
            }
        }

        private void PlayParticles(Vector2 slicePoint, Color color)
        {
            _particleSystemPlayer.PlayAll(slicePoint, color);
        }
    }
}