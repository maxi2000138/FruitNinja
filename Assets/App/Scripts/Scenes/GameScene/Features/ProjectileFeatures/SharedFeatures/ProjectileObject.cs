using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Rotater;
using UnityEngine;

public class ProjectileObject : MonoBehaviour
{ 
    public Vector2 Scale => transform.localScale;
    public float SpriteDiagonal() => new Vector2(_spriteRenderer.sprite.bounds.size.x, _spriteRenderer.sprite.bounds.size.y).magnitude * 2f;
    
    [field: SerializeField] public CloneForceMover ShadowCloneMover;
    [field: SerializeField] public CloneForceRotater ShadowCloneRotater;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    public void SetSpriteAndScale(Sprite sprite, Vector2 objectScale, int sortingOrder)
    {
        transform.localScale = objectScale;
        _spriteRenderer.sprite = sprite;
        _spriteRenderer.sortingOrder = sortingOrder;
    }
    

}
