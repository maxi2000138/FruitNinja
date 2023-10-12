using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover
{
    public interface IMover
    {
        public Vector2 Move(float deltaTime);
        public void Clear();
    }
}