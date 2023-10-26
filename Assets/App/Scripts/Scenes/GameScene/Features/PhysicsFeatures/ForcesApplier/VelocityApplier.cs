using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesApplier
{
    public class VelocityApplier : MonoBehaviour, IMover
    {
        private Vector2 _velocityVector;

        public void AddVelocity(Vector2 forceVector)
        {
             _velocityVector += forceVector;
        }

        public Vector2 Move(Vector2 movementVector, float deltaTime, float timeScale)
        {
            if (_velocityVector != Vector2.zero)
            {
                Vector2 forceVector = _velocityVector * (Mathf.Sqrt(timeScale) * deltaTime);
                Clear();
                return forceVector;
            }
            
            return Vector2.zero;
        }

        public void Clear()
        {
            _velocityVector = Vector2.zero;
        } 
    }
}
