using System.Collections;
using UnityEngine;

public class SpriteScaler
{
    private Coroutine _scaleCoroutine;
    private readonly SpriteRenderer _spriteRenderer;
    private readonly ICoroutineRunner _coroutineRunner;

    public SpriteScaler(SpriteRenderer spriteRenderer, ICoroutineRunner coroutineRunner)
    {
        _spriteRenderer = spriteRenderer;
        _coroutineRunner = coroutineRunner;
    }

    public void StartScaling(float maxScale, float flyTime)
    {
        _scaleCoroutine = _coroutineRunner.StartCoroutine(SpriteScaleCoroutine(maxScale, flyTime));
    }

    public void StopScaling()
    {
        if(_scaleCoroutine != null)
            _coroutineRunner.StopCoroutine(_scaleCoroutine);   
    }

    private void SetSpriteScale(Vector2 localScale)
    {
        _spriteRenderer.transform.localScale = localScale;
    }

    private IEnumerator SpriteScaleCoroutine(float finalScale, float flyTime)
    {
        float startScale = _spriteRenderer.transform.localScale.x;
        Vector2 currentScale = _spriteRenderer.transform.localScale;
        float time = 0f;
        while (true)
        {
            currentScale.x = Mathf.Lerp(startScale, finalScale, time/flyTime);
            currentScale.y = Mathf.Lerp(startScale, finalScale, time/flyTime);
            SetSpriteScale(currentScale);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            time += Time.fixedDeltaTime;
        }
    }
}
