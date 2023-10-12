using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ColliderFeatures
{
    public class SliceCollidersController
    {
        private readonly List<SliceCircleCollider> _colliders = new();

        public void AddCollider(SliceCircleCollider sliceCircleCollider)
        {
            _colliders.Add(sliceCircleCollider);
        }

        public void RemoveCollider(SliceCircleCollider sliceCircleCollider)
        {
            _colliders.Remove(sliceCircleCollider);
        }

        public bool TryGetIntersectionCollider(Vector2 point, out SliceCircleCollider collider)
        {
            collider = null;
            for (int i = 0; i < _colliders.Count; i++)
            {
                if (_colliders[i].IsPointInsideCollider(point))
                {
                    collider = _colliders[i];
                    return true;
                }
            }

            return false;
        }
    }
}
