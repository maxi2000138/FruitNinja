using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.PhysicsFramework;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover
{
    public class Mover : PhysicsBehaviour
    {
        public Vector2 MovementVector { get; private set; }
        
        private List<IMover> _movers = new();

        private void Awake()
        {
            _movers.AddRange(GetComponents<IMover>());
        }

        public void AddMover(IMover mover)
        {
            _movers.Add(mover);
        }

        public void RemoveMover(IMover mover)
        {
            _movers.Remove(mover);
        }
    
        public override void ExecuteOperation(GameObject physicsObject, float deltaTime, float timeScale)
        {
            foreach (IMover mover in _movers)
            {
                MovementVector += mover.Move(MovementVector, deltaTime, timeScale);
            }
            
            physicsObject.transform.Translate(MovementVector * timeScale, Space.World);
        }
    }
}
