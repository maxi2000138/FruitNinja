using System;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileDestroyer.DestroyTrigger;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.SharedFeatures
{
    public class TwoPartsSliceObject : MonoBehaviour, IFullSliceObject
    {
        [SerializeField]
        private Transform _leftPartTransform;
        [SerializeField]
        private Transform _rightsPartTransform;
        [field: SerializeField]
        public ProjectileObject ProjectileObject { get; private set; }
        public event Action OnSliceEvent;

        [field: SerializeField] 
        public ProjectileType ProjectileType { get; private set; }

        public ISlicable Slicable { get; private set; }

        private IDestroyTrigger _destroyTrigger;

        private ISliced _leftObject;

        private ISliced _rightObject;

        private Func<ISliced> _leftPartSpawnMethod;

        private Func<ISliced> _rightPartSpawnMethod;

        private ProjectileType _projectileType;

        private void Awake()
        {
            Slicable = GetComponent<ISlicable>();
        }

        public void Construct(Func<ISliced> leftPartSpawnMethod, Func<ISliced> rightPartSpawnMethod, IDestroyTrigger destroyTrigger)
        {
            _destroyTrigger = destroyTrigger;
            _leftPartSpawnMethod = leftPartSpawnMethod;
            _rightPartSpawnMethod = rightPartSpawnMethod;
        }


        public void Slice(Mover mover, float sliceForces, out bool disableColliderOnSlice)
        {
            OnSliceEvent?.Invoke();
            SpawnParts(); 
            SetupSlicedPart(_leftObject,_leftPartTransform.position, transform.eulerAngles, transform.localScale);
            SetupSlicedPart(_rightObject ,_rightsPartTransform.position, transform.eulerAngles, transform.localScale);
            SetVelocity(mover, sliceForces);
            disableColliderOnSlice = true;
            DestroyFruit();
            Slicable.OnSlice();   
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
            _destroyTrigger.TriggerGroup(GetComponent<ProjectileObject>(), true);
        }

        private void SetVelocity(Mover mover, float sliceForce)
        {
            Vector2 movementVector = mover.MovementVector.normalized;

            Vector2 leftVector = Quaternion.AngleAxis(25f, Vector3.forward) *
                              (movementVector * sliceForce);
            Vector2 rightVector = Quaternion.AngleAxis(-25f, Vector3.forward) *
                                  (movementVector * sliceForce);

            if (Mathf.Abs(Vector3.SignedAngle(transform.up, movementVector, Vector3.forward)) >= 90f)
                (leftVector, rightVector) = (rightVector, leftVector);

            _leftObject.VelocityApplier.AddVelocity(leftVector);
            _rightObject.VelocityApplier.AddVelocity(rightVector);
        }
    }
}