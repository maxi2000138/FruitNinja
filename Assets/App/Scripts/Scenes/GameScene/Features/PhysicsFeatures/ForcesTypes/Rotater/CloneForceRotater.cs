using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.PhysicsFramework;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Rotater
{
    public class CloneForceRotater : PhysicsBehaviour
    {
        private GameObject _cloneObject;

        public void Construct(GameObject cloneObject)
        {
            _cloneObject = cloneObject;
        }

        public override void ExecuteOperation(GameObject physicsObject, float deltaTime)
        {
            if (_cloneObject != null)
            {
                _cloneObject.transform.rotation = physicsObject.transform.rotation;
            }
        }
    }
}