using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesApplier
{
    public class GravitationApplier : MonoBehaviour, IMover
    {
        [SerializeField] private PhysicsConfig _physicsConfig;
        public void Clear()
        {
            
        }
    
        public Vector2 Move(float deltaTime)
        {
            return new Vector3(0f, _physicsConfig.StartGravityValue * deltaTime * deltaTime, 0f);
        }
    }
}