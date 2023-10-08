using System;
using System.Collections;
using UnityEngine;

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

    public void StartOffseter(Vector2 offset, Vector2 distanceRange, float flyTime)
    {
        _offsetCoroutine = _coroutineRunner.StartCoroutine(SpriteOffsetCoroutine(offset, distanceRange, flyTime));
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

    private IEnumerator SpriteOffsetCoroutine(Vector2 offsetVector, Vector2 distanceRange, float flyTime)
    {
        Vector2 currentOffset = Vector2.zero;
        Vector2 startOffset = offsetVector * distanceRange.x;
        Vector2 maxOffset = offsetVector * distanceRange.y;
        float time = 0f;

        while (true)
        {
            currentOffset.x = Mathf.Lerp(startOffset.x, maxOffset.x, time / flyTime);
            currentOffset.y = Mathf.Lerp(startOffset.y, maxOffset.y, time / flyTime);
            SetSpriteOffset(currentOffset);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            time += Time.fixedDeltaTime;
        }
    }
    
}
