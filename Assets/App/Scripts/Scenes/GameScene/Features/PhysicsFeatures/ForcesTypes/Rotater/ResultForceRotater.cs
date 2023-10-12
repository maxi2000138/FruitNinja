using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.PhysicsFramework;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Rotater
{
    public class ResultForceRotater : PhysicsBehaviour
    {
        private List<IRotater> _rotaters = new();

        private void Awake()
        {
            _rotaters.AddRange(GetComponents<IRotater>());
        }
    
        public override void ExecuteOperation(GameObject physicsObject)
        {
            Vector3 rotationVector = Vector3.zero;
            foreach (IRotater mover in _rotaters)
            {
                rotationVector += mover.Rotate(Time.fixedDeltaTime);
            }

            physicsObject.transform.Rotate(rotationVector, Space.World);
        }
    }
}