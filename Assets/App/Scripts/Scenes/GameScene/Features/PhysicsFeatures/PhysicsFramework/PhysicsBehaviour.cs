using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.PhysicsFramework
{
    public abstract class PhysicsBehaviour : MonoBehaviour
    {
        public abstract void ExecuteOperation(GameObject physicsObject, float deltaTime);
    }
}
