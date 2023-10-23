using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    private HealthConfig _healthConfig;
    private Sprite _heartSprite;

    private readonly List<HeartView> _hearts = new();
    private TweenCore _tweenCore;
    private TokenController _tokenController;

    public void Construct(HealthConfig healthConfig, TweenCore tweenCore)
    {
        _tokenController = new TokenController();
        _tweenCore = tweenCore;
        _healthConfig = healthConfig;
    }

    private void OnDestroy()
    {
        _tokenController.CancelTokens();
    }

    public void SetupHealth(int healthAmount)
    {
        for (int i = 0; i < _hearts.Count; i++)
        {
            Destroy(_hearts[i].gameObject);
        }
        
        _hearts.Clear();
        
        for (int i = 0; i < healthAmount; i++)
        {
            var heartObject = CreateView();
            SetPosition(heartObject, i);
        }
    }

    public void HealthHealth(Vector3 startAnimationPoint)
    {
        Vector2 endPoint = new Vector2(_healthConfig.RightOffset + _healthConfig.SpriteOffset * _hearts.Count - 1, _healthConfig.UpOffset);
        HeartView heartView = CreateView();
        RectTransform rectTransform = heartView.GetComponent<RectTransform>();
        _tweenCore.TweenByTime<Vector3>((position) => rectTransform.anchoredPosition = position, startAnimationPoint, endPoint, 1f, CustomEase.Linear, _tokenController.CreateCancellationToken());
    }
    
    public void LooseHealth()
    {
        HeartView heart = _hearts[^1];
        _hearts.Remove(heart);
        heart.DestroyView();
    }


    private HeartView CreateView()
    {
        HeartView heartObject = Instantiate(_healthConfig.HeartPrefab, transform).GetComponent<HeartView>();
        heartObject.Construct(_tweenCore);
        _hearts.Add(heartObject);
        return heartObject;
    }

    private void SetPosition(HeartView heartObject, int i)
    {
        heartObject.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(_healthConfig.RightOffset + _healthConfig.SpriteOffset * i, _healthConfig.UpOffset);
    }
}
