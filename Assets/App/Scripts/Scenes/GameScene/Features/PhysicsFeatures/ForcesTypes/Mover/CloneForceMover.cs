using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.PhysicsFramework;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover
{
    public class CloneForceMover : PhysicsBehaviour
    {
        private GameObject _cloneObject;

        public void Construct(GameObject cloneObject)
        {
            _cloneObject = cloneObject; 
        }
    
        public override void ExecuteOperation(GameObject physicsObject)
        {
            if (_cloneObject != null)
            {
                Vector2 newPosition = physicsObject.transform.position;
                _cloneObject.transform.position = newPosition;
            }
        }
    }
}
