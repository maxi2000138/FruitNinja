using System;
using App.Scripts.DebugAndGizmosExtensions;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.SharedFeatures;
using Sirenix.Serialization;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ColliderFeatures
{
    [RequireComponent(typeof(IFullSliceObject))]
    public class SliceCircleCollider : MonoBehaviour
    {
        public IFullSliceObject SliceObject { get; private set; }
        [field:SerializeField] public Mover ForceMover { get; private set; }
        [field:SerializeField] public Transform ColliderObject { get; private set; }
        [SerializeField] private float _colliderRadiusOffset;
        
        private SliceCollidersController _sliceCollidersController;
        private bool _isActive;


        public void Construct(SliceCollidersController sliceCollidersController)
        {
            _sliceCollidersController = sliceCollidersController;
            _sliceCollidersController.AddCollider(this);
            Enable();
        }

        private void Awake()
        {
            SliceObject = GetComponent<IFullSliceObject>();
        }

        public void Enable()
        {
            _isActive = true;
        }

        public void Disable()
        {
            _isActive = false;
        }

        private void OnDestroy()
        {
            _sliceCollidersController.RemoveCollider(this);
        }

        private void Update()
        {
            DebugAndGizmosDrawer.DrawCircleDebug(ColliderObject.transform.position, ColliderObject.transform.localScale.magnitude + _colliderRadiusOffset, 100, Color.black);
        }

        public bool IsPointInsideCollider(Vector2 point)
        {
            if (!_isActive)
                return false;
        
            return (point - (Vector2)ColliderObject.transform.position).magnitude <= ColliderObject.transform.localScale.magnitude + + _colliderRadiusOffset;
        }
    }
}
