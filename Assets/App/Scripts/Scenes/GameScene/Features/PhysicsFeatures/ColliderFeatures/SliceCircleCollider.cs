using App.Scripts.DebugAndGizmosExtensions;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.SharedFeatures;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ColliderFeatures
{
    public class SliceCircleCollider : MonoBehaviour
    {
        [field:SerializeField] public SliceObject SliceObject { get; private set; }
        [field:SerializeField] public Transform ColliderObject { get; private set; }
    
        private SliceCollidersController _sliceCollidersController;
        private bool _isActive;

        public void Construct(SliceCollidersController sliceCollidersController)
        {
            _sliceCollidersController = sliceCollidersController;
            _sliceCollidersController.AddCollider(this);
            Enable();
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
            DebugAndGizmosDrawer.DrawCircleDebug(ColliderObject.transform.position, ColliderObject.transform.localScale.magnitude, 100, Color.black);
        }

        public bool IsPointInsideCollider(Vector2 point)
        {
            if (!_isActive)
                return false;
        
            return (point - (Vector2)ColliderObject.transform.position).magnitude <= ColliderObject.transform.localScale.magnitude;
        }
    }
}
