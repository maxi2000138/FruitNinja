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
        _tweenCore = tweenCore;
        _tokenController = new TokenController();
    }

    public async void DestroyView()
    {
        await _tweenCore.TweenByTime(PositionYTween, transform.position.y, transform.position.y + _healtUpDistance, 1f, CustomEase.OutQuad, new CancellationToken());
        _tokenController.CancelTokens();
        Destroy(gameObject);
    }

    private void PositionYTween(float value) => 
        transform.position = new Vector3(transform.position.x, value, transform.position.z);
}
