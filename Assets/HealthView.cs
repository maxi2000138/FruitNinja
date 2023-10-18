using System.Collections.Generic;
using UnityEngine;

public class HealthView : MonoBehaviour
{
    private HealthConfig _healthConfig;
    private Sprite _heartSprite;

    private readonly List<GameObject> _hearts;

    public void Construct(HealthConfig healthConfig)
    {
        _healthConfig = healthConfig;
    }
    
    public void SetupHealth(int healthAmount)
    {
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
        Destroy(_hearts[^1]);
    }
}
