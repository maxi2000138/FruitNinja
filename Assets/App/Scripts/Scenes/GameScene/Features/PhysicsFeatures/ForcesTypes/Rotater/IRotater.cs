using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Rotater
{
    public interface IRotater
    {
        public Vector3 Rotate(float deltaTime, float timeScale);
        public void Clear();
    }
}