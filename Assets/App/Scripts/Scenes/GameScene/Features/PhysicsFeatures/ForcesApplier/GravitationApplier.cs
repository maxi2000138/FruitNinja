using System;
using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesApplier
{
    public class GravitationApplier : MonoBehaviour, IMover
    {
        [SerializeField] private PhysicsConfig _physicsConfig;
        private float _gravityValue;

        private void Start()
        {
            Enable();
        }

        public void Enable()
        {
            _gravityValue = _physicsConfig.StartGravityValue;
        }

        public void Disable()
        {
            _gravityValue = 0f;
        }

        public void Clear()
        {
            
        }
    
        public Vector2 Move(Vector2 movementVector, float deltaTime)
        {
            return new Vector3(0f, _gravityValue * deltaTime * deltaTime, 0f);
        }
    }
}