using System;
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
        [field: SerializeField] 
        public ProjectileType ProjectileType;
        
        private IDestroyTrigger _destroyTrigger;
        private ISliced _leftObject;
        private ISliced _rightObject;
        
        private Func<ISliced> _leftPartSpawnMethod;
        private Func<ISliced> _rightPartSpawnMethod;
        private ISlicable _slicable;

        public void Construct(Func<ISliced> leftPartSpawnMethod, Func<ISliced> rightPartSpawnMethod, ISlicable slicable, IDestroyTrigger destroyTrigger)
        {
            _slicable = slicable;
            _destroyTrigger = destroyTrigger;
            _leftPartSpawnMethod = leftPartSpawnMethod;
            _rightPartSpawnMethod = rightPartSpawnMethod;
        }

        public void Slice(Mover mover, float sliceForce)
        {
            SpawnParts(); 
            SetupSlicedPart(_leftObject,_leftPartTransform.position, transform.eulerAngles, transform.localScale);
            SetupSlicedPart(_rightObject ,_rightsPartTransform.position, transform.eulerAngles, transform.localScale);
            SetVelocity(mover, sliceForce);
            DestroyFruit();
            _slicable.OnSlice();
        }

        private void SpawnParts()
        {
            _leftObject = _leftPartSpawnMethod?.Invoke();
            _rightObject = _rightPartSpawnMethod?.Invoke();
        }

        private void SetupSlicedPart(ISliced slicePart, Vector2 position, Vector3 rotation, Vector2 scale)
        {
            slicePart.Transform.position = position;
            slicePart.Transform.eulerAngles = rotation;
            slicePart.Transform.localScale = scale;
            slicePart.Transform.gameObject.SetActive(true);
        }
        
        private void DestroyFruit()
        {
            _destroyTrigger.TriggerGroup(transform);
        }

        private void SetVelocity(Mover mover, float sliceForce)
        {
            float rotationZ = (transform.eulerAngles.z % 360f);
            Vector2 movementVector = mover.MovementVector.normalized;
            
            if ((rotationZ < 90f || rotationZ > 270f) && movementVector.y > 0f)
            {
                _leftObject.VelocityApplier.AddVelocity(Quaternion.AngleAxis(-15f, new Vector3(0f, 0f, -1f)) *
                                                        movementVector * sliceForce);
                _rightObject.VelocityApplier.AddVelocity(Quaternion.AngleAxis(15f, new Vector3(0f, 0f, -1f)) *
                                                         movementVector * sliceForce);
            }
            else if (movementVector.y > 0f)
            {
                _leftObject.VelocityApplier.AddVelocity(Quaternion.AngleAxis(15f, new Vector3(0f, 0f, -1f)) *
                                                        movementVector * sliceForce);
                _rightObject.VelocityApplier.AddVelocity(Quaternion.AngleAxis(-15f, new Vector3(0f, 0f, -1f)) *
                                                         movementVector * sliceForce);
            }
            else if ((rotationZ < 90f || rotationZ > 270f) && movementVector.y <= 0f)
            {
                _leftObject.VelocityApplier.AddVelocity(Quaternion.AngleAxis(15f, new Vector3(0f, 0f, -1f)) *
                                                        movementVector * sliceForce);
                _rightObject.VelocityApplier.AddVelocity(Quaternion.AngleAxis(-15f, new Vector3(0f, 0f, -1f)) *
                                                         movementVector * sliceForce);
            }
            else if (movementVector.y <= 0f)
            {
                _leftObject.VelocityApplier.AddVelocity(Quaternion.AngleAxis(-15f, new Vector3(0f, 0f, -1f)) *
                                                        movementVector * sliceForce);
                _rightObject.VelocityApplier.AddVelocity(Quaternion.AngleAxis(15f, new Vector3(0f, 0f, -1f)) *
                                                         movementVector * sliceForce);
            }
        }
    }
}