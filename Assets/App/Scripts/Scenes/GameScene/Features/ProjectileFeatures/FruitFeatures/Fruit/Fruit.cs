using App.Scripts.Scenes.GameScene.Features.ParticleFeatures;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesApplier;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Rotater;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Enum;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileFactory;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Fruit
{
    public class Fruit : MonoBehaviour, ISlicable
    {
        public float SpriteMaxHeight => SpriteDiagonal();
        public Vector2 Scale => transform.localScale;

        [field: SerializeField] public CloneForceMover ShadowCloneMover;
        [field: SerializeField] public CloneForceRotater ShadowCloneRotater;
        [field: SerializeField] public ScaleByTimeApplier ScaleByTimeApplier;
        [field: SerializeField] public TorqueApplier TorqueApplier;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private ParticleSystemPlayer _particleSystemPlayer;
        private Color _sliceColor;

        public void Construct(Color sliceColor, ParticleSystemPlayer particleSystemPlayer)
        {
            _sliceColor = sliceColor;
            _particleSystemPlayer = particleSystemPlayer;
        }

        public void OnSlice()
        {
            _particleSystemPlayer.PlayAll(transform.position, _sliceColor);   
        }

        public void SetSprite(Sprite sprite, int sortingOrder)
        {
            _spriteRenderer.sprite = sprite;
            _spriteRenderer.sortingOrder = sortingOrder;
        }

        private float SpriteDiagonal() => new Vector2(_spriteRenderer.sprite.bounds.size.x, _spriteRenderer.sprite.bounds.size.y).magnitude;
    }
}