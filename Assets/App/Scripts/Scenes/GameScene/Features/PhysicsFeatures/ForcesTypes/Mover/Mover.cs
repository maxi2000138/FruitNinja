using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.PhysicsFramework;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover
{
    public class Mover : PhysicsBehaviour
    {
        public Vector2 MovementVector { get; private set; }
        [SerializeField] private float _mass = 1f;   
        private List<IMover> _movers = new();

        private void Awake()
        {
            _movers.AddRange(GetComponents<IMover>());
        }
    
        public override void ExecuteOperation(GameObject physicsObject, float deltaTime)
        {
            foreach (IMover mover in _movers)
            {
                MovementVector += mover.Move(Time.fixedDeltaTime);
            }
            
            physicsObject.transform.Translate(MovementVector, Space.World);
        }
    }
}
