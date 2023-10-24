using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover
{
    public interface IMover
    {
        public Vector2 Move(Vector2 movementVector, float deltaTime);
        public void Clear();
    }
}