using System;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesApplier;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Rotater;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.SharedFeatures;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Fruit
{
    public class Fruit : MonoBehaviour
    {
        public float SpriteMaxHeight => SpriteDiagonal(); 

        [field: SerializeField] public CloneForceMover ShadowCloneMover;
        [field: SerializeField] public CloneForceRotater ShadowCloneRotater;
        [field: SerializeField] public ScaleByTimeApplier ScaleByTimeApplier;
        [field: SerializeField] public TorqueApplier TorqueApplier;
        
        
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public void SetSprite(Sprite sprite, int sortingOrder)
        {
            _spriteRenderer.sprite = sprite;
            _spriteRenderer.sortingOrder = sortingOrder;
        }
        
        private float SpriteDiagonal() => new Vector2(_spriteRenderer.sprite.bounds.size.x, _spriteRenderer.sprite.bounds.size.y).magnitude;
    }
}