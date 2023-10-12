using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Rotater;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ShadowFeatures;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Fruit
{
    public class FruitPart : MonoBehaviour
    {
        public float SpriteMaxHeight => SpriteDiagonal();
        public Vector2 SpriteScale => _spriteRenderer.transform.localScale;
        public Shadow Shadow { get; private set; }

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private CloneForceMover _shadowForceMover;
        [SerializeField] private CloneForceRotater _shadowForceRotater;
        [SerializeField] private Vector2 _spriteScale;
    
        public void SetSprite(Sprite sprite, int sortingOrder)
        {
            _spriteRenderer.sprite = sprite;
            _spriteRenderer.sortingOrder = sortingOrder;
        }

        public void SetShadow(Shadow shadow)
        {
            Shadow = shadow;
            _shadowForceMover.Construct(shadow.gameObject);
            _shadowForceRotater.Construct(shadow.SpriteGameObject);
        }

        public void StartChangingShadowSpriteScale(Vector2 startScale, Vector2 finalScale, float flyTime)
        {
            Shadow.ScaleByTimeApplier.StartScaling(startScale, finalScale, flyTime);
        }

        public void StartChangingShadowOffset(Vector2 startOffset, Vector2 finalOffset, float flyTime)
        {
            Shadow.OffsetByTimeApplier.StartOffseting(startOffset,finalOffset, flyTime);
        }
    
        private float SpriteDiagonal() => Mathf.Sqrt(Mathf.Pow(_spriteRenderer.sprite.bounds.size.x,2) + Mathf.Pow(_spriteRenderer.sprite.bounds.size.y,2));
    }
}