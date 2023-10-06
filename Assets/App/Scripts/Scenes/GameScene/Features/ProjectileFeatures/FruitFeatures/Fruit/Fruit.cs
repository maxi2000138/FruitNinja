using UnityEngine;

public class Fruit : MonoBehaviour
{
    public float SpriteMaxHeight => SpriteDiagonal() * _spriteScale.y;
    [field: SerializeField] public Shadow Shadow { get; private set; }

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private CloneMover _shadowMover;
    [SerializeField] private CloneRotater _shadowRotater;
    private Vector2 _spriteScale;

    public void SetSprite(Sprite sprite, Vector2 spriteScale)
    {
        _spriteScale = spriteScale;
        _spriteRenderer.sprite = sprite;
        _spriteRenderer.transform.localScale =spriteScale;
    }

    public void SetShadow(Shadow shadow)
    {
        Shadow = shadow;
        _shadowMover.Construct(shadow.gameObject);
        _shadowRotater.Construct(shadow.SpriteGameObject);
    }
    
    private float SpriteDiagonal() => Mathf.Sqrt(Mathf.Pow(_spriteRenderer.sprite.bounds.size.x,2) + Mathf.Pow(_spriteRenderer.sprite.bounds.size.y,2));
}