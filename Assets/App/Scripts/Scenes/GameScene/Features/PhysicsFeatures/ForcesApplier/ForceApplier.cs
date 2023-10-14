using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesApplier
{
    public class ForceApplier : MonoBehaviour, IMover
    {
        private Vector2 _forceVector;

        public void AddForce(Vector2 forceVector)
        {
            _forceVector += forceVector;
        }

        public Vector2 Move(float deltaTime)
        {
            if (_forceVector != Vector2.zero)
            {
                Vector2 forceVector = _forceVector * deltaTime;
                Clear();
                return forceVector;
            }
            
            return Vector2.zero;
        }

        public void Clear()
        {
            _forceVector = Vector2.zero;
        } 
    }
}
