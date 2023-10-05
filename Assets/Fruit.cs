using UnityEngine;

public class Fruit : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public void SetFruitSprite(Sprite sprite, Vector2 spriteScale)
    {
        _spriteRenderer.sprite = sprite;
        _spriteRenderer.transform.localScale =spriteScale;
    }
}
