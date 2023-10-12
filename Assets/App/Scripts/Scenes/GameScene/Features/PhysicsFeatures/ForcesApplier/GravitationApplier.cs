using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesApplier
{
    public class GravitationApplier : MonoBehaviour, IMover
    {
        [SerializeField] private GravitationConfig _gravitationConfig;
        private float _velocity = 0f;

        public void Clear()
        {
            _velocity = 0f;
        }
    
        public Vector2 Move(float deltaTime)
        {
            _velocity += _gravitationConfig.StartGravityValue * deltaTime;
            float deltaY = _velocity * deltaTime;
            return new Vector3(0f, deltaY, 0f);
        }
    }
}