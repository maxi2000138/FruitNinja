using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesApplier;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Rotater;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ShadowFeatures;
using UnityEngine;

public class ProjectileObject : MonoBehaviour
{ 
    public Vector2 Scale => transform.localScale;
    public ProjectilePartEnum ProjectilePart { get; private set; }
    public float SpriteDiagonal() => new Vector2(_spriteRenderer.sprite.bounds.size.x, _spriteRenderer.sprite.bounds.size.y).magnitude * 2f;
    
    [field: SerializeField] public CloneForceMover ShadowCloneMover;
    [field: SerializeField] public CloneForceRotater ShadowCloneRotater;
    [field: SerializeField] public Mover Mover;
    [field: SerializeField] public GravitationApplier GravitationApplier;
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    private Shadow _shadow;

    public void Construct(ProjectilePartEnum projectilePart, Shadow shadow)
    {
        ProjectilePart = projectilePart;
        _shadow = shadow;
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
