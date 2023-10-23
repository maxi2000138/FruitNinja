using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesApplier
{
    public class VelocityApplier : MonoBehaviour, IMover
    {
        private const float FORCE_APPLY_TIME = 1/60f;
        private Vector2 _velocityVector;

        public void AddVelocity(Vector2 forceVector)
        {
             _velocityVector += forceVector;
        }

        public Vector2 Move(float deltaTime)
        {
            if (_velocityVector != Vector2.zero)
            {
                Vector2 forceVector = _velocityVector * FORCE_APPLY_TIME;
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
