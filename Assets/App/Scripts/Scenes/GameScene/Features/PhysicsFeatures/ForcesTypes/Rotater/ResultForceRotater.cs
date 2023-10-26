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
    
        public override void ExecuteOperation(GameObject physicsObject, float deltaTime, float timeScale)
        {
            Vector3 rotationVector = Vector3.zero;
            foreach (IRotater mover in _rotaters)
            {
                rotationVector += mover.Rotate(deltaTime, timeScale);
            }

            physicsObject.transform.Rotate(rotationVector * timeScale, Space.World);
        }
    }
}