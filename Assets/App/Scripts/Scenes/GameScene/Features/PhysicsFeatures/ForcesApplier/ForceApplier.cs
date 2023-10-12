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
            return _forceVector * deltaTime;
        }

        public void Clear()
        {
            _forceVector = Vector3.zero;
        }
    }
}
