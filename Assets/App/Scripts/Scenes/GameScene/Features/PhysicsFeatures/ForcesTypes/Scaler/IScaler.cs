using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Scaler
{
    public interface IScaler
    {
        public Vector2 Scale(float deltaTime);
    }
}