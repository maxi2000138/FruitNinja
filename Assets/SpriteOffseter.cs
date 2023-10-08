using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SpriteOffseter
{
    private Coroutine _offsetCoroutine;
    private readonly SpriteRenderer _spriteRenderer;
    private readonly ICoroutineRunner _coroutineRunner;

    public SpriteOffseter(SpriteRenderer spriteRenderer, ICoroutineRunner coroutineRunner)
    {
        _spriteRenderer = spriteRenderer;
        _coroutineRunner = coroutineRunner;
    }

    public void StartOffseter(Vector2 offset, float finalOffset, float flyTime)
    {
        _offsetCoroutine = _coroutineRunner.StartCoroutine(SpriteOffsetCoroutine(offset, finalOffset, flyTime));
    }

    public void StopOffseter()
    {
        if(_offsetCoroutine != null)
            _coroutineRunner.StopCoroutine(_offsetCoroutine);   
    }

    private void SetSpriteOffset(Vector2 position)
    {
        _spriteRenderer.transform.localPosition =position;
    }

    private IEnumerator SpriteOffsetCoroutine(Vector2 offsetVector, float finalOffset, float flyTime)
    {
        Vector2 currentOffset = Vector2.zero;
        Vector2 startOffset = offsetVector * _spriteRenderer.transform.localScale;
        Vector2 maxOffset = offsetVector * finalOffset;
        float time = 0f;

        while (true)
        {
            currentOffset.x = Mathf.Lerp(startOffset.x, maxOffset.x, time / flyTime);
            currentOffset.y = Mathf.Lerp(startOffset.y, maxOffset.y, time / flyTime);
            SetSpriteOffset(currentOffset);
            time += Time.deltaTime;
            yield return null;
        }
    }
    
}
