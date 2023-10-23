using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HeartView : MonoBehaviour
{
    private TweenCore _tweenCore;
    private float _healtUpDistance = 5f;
    private TokenController _tokenController;

    public void Construct(TweenCore tweenCore)
    {
        _tokenController = new TokenController();
        _tweenCore = tweenCore;
    }

    private void OnDestroy()
    {
        _tokenController.CancelTokens();
    }

    public async void DestroyView()
    {
        if(transform == null)
            return;
        
        float startValue = transform.position.y;
        await _tweenCore.TweenByTime(PositionYTween, startValue, startValue + _healtUpDistance, 1f, CustomEase.OutQuad, _tokenController.CreateCancellationToken());
        _tokenController.CancelTokens();
        Destroy(gameObject);
    }

    private void PositionYTween(float value)
    {
        transform.position = new Vector3(transform.position.x, value, transform.position.z);
    }
}
