using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesApplier;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Rotater;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Enum;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileFactory;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Fruit
{
    public class Fruit : MonoBehaviour
    {
        public float SpriteMaxHeight => SpriteDiagonal();
        public Vector2 Scale => transform.localScale;

        [field: SerializeField] public CloneForceMover ShadowCloneMover;
        [field: SerializeField] public CloneForceRotater ShadowCloneRotater;
        [field: SerializeField] public ScaleByTimeApplier ScaleByTimeApplier;
        [field: SerializeField] public TorqueApplier TorqueApplier;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private IProjectileFactory _projectileFactory;
        private FruitType _fruitType;

        public void Construct(IProjectileFactory projectileFactory, FruitType fruitType)
        {
            _fruitType = fruitType;
            _projectileFactory = projectileFactory;
        }
        
        public void SetSprite(Sprite sprite, int sortingOrder)
        {
            _spriteRenderer.sprite = sprite;
            _spriteRenderer.sortingOrder = sortingOrder;
        }
        
        private float SpriteDiagonal() => new Vector2(_spriteRenderer.sprite.bounds.size.x, _spriteRenderer.sprite.bounds.size.y).magnitude;
    }
}