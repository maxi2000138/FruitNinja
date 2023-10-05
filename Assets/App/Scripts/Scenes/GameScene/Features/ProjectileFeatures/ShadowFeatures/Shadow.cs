using System;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    public void TurnIntoShadow()
    {
        _spriteRenderer.color = Color.black;
        
    }
}
