using System.Collections.Generic;
using UnityEngine;

public class HealthView : MonoBehaviour
{
    private HealthConfig _healthConfig;
    private Sprite _heartSprite;

    private readonly List<GameObject> _hearts = new();

    public void Construct(HealthConfig healthConfig)
    {
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
            GameObject heartObject = Instantiate(_healthConfig.HeartPrefab, transform);
            _hearts.Add(heartObject);
            heartObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(_healthConfig.RightOffset + _healthConfig.SpriteOffset * i, _healthConfig.UpOffset);
        }
    }

    public void LooseHealth()
    {
        GameObject heart = _hearts[^1];
        _hearts.Remove(heart);
        Destroy(heart);
    }
}
