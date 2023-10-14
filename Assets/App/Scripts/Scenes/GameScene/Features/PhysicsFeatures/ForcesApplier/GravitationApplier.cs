using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesApplier
{
    public class GravitationApplier : MonoBehaviour, IMover
    {
        [SerializeField] private GravitationConfig _gravitationConfig;

        public void Clear()
        {
            
        }
    
        public Vector2 Move(float deltaTime)
        {
            return new Vector3(0f, _gravitationConfig.StartGravityValue * deltaTime * deltaTime, 0f);
        }
    }
}