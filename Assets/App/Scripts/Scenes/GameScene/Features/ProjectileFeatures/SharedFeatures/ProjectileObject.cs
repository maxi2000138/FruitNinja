using System;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesApplier;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Rotater;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.PhysicsFramework;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ShadowFeatures;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileObject : MonoBehaviour
{ 
    public Vector2 Scale => transform.localScale;
    public ProjectilePartEnum ProjectilePart { get; private set; }
    public float SpriteDiagonal() => new Vector2(_spriteRenderer.sprite.bounds.size.x, _spriteRenderer.sprite.bounds.size.y).magnitude * 2f;
    public event Action OnDestroyEvent;
    
    [field: SerializeField] public CloneForceMover ShadowCloneMover;
    [field: SerializeField] public CloneForceRotater ShadowCloneRotater;
    [field: SerializeField] public Mover Mover;
    [field: SerializeField] public GravitationApplier GravitationApplier;
    [field: SerializeField] private PhysicsForcesOrder _physicsForcesOrder;
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    private Shadow _shadow;

    public void Construct(ProjectilePartEnum projectilePart, Shadow shadow, TimeScaleService timeScaleService)
    {
        _physicsForcesOrder.Construct(timeScaleService);
        ProjectilePart = projectilePart;
        _shadow = shadow;
    }

    private void OnDestroy()
    {
        OnDestroyEvent?.Invoke();
    }

    public void SetSpriteAndScale(Sprite sprite, Vector2 objectScale, int sortingOrder)
    {
        transform.localScale = objectScale;
        _spriteRenderer.sprite = sprite;
        _spriteRenderer.sortingOrder = sortingOrder;
    }

    public GameObject[] ProjectileGameObjects()
    {
        return new[] { gameObject, _shadow.gameObject };
    }
    
}
