using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    private HealthConfig _healthConfig;
    private Sprite _heartSprite;

    private readonly List<HeartView> _hearts = new();
    private TweenCore _tweenCore;

    public void Construct(HealthConfig healthConfig, TweenCore tweenCore)
    {
        _tweenCore = tweenCore;
        _healthConfig = healthConfig;
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
            HeartView heartObject = Instantiate(_healthConfig.HeartPrefab, transform).GetComponent<HeartView>();
            heartObject.Construct(_tweenCore);
            _hearts.Add(heartObject);
            heartObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(_healthConfig.RightOffset + _healthConfig.SpriteOffset * i, _healthConfig.UpOffset);
        }
    }

    public void LooseHealth()
    {
        HeartView heart = _hearts[^1];
        _hearts.Remove(heart);
        heart.DestroyView();
    }
}
