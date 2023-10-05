using UnityEngine;

public class Fruit : MonoBehaviour
{
    public float SpriteMaxHeight => Mathf.Max(_spriteRenderer.sprite.bounds.size.y,_spriteRenderer.sprite.bounds.size.x) * _spriteScale.y;
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Vector2 _spriteScale;

    public void SetFruitSprite(Sprite sprite, Vector2 spriteScale)
    {
        _spriteScale = spriteScale;
        _spriteRenderer.sprite = sprite;
        _spriteRenderer.transform.localScale =spriteScale;
    }
}
