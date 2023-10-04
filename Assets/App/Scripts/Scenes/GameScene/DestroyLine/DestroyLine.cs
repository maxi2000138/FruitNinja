using System.Collections.Generic;
using UnityEngine;

public class DestroyLine : IInitializable, IUpdatable, IDestroyLine
{
    private float _yDestroyValue = 0f;
    private readonly IScreenSettingsProvider _screenSettingsProvider;
    private readonly IProjectileDestroyer _projectileDestroyer;
    private readonly GameConfig _gameConfig;
    private readonly List<Transform> _destroyListeners = new ();


    public DestroyLine(IScreenSettingsProvider screenSettingsProvider, IProjectileDestroyer projectileDestroyer, GameConfig gameConfig)
    {
        _screenSettingsProvider = screenSettingsProvider;
        _projectileDestroyer = projectileDestroyer;
        _gameConfig = gameConfig;
    }
    
    public void AddLineDestroyListener(Transform objectTransform)
    {
        if(!_destroyListeners.Contains(objectTransform))
            _destroyListeners.Add(objectTransform);
    }

    public void Initialize()
    {
        Vector2 zeroCameraPoint = _screenSettingsProvider.ViewportToWorldPosition(Vector2.zero);
        _yDestroyValue = zeroCameraPoint.y + _gameConfig.DestroyLineYOffset;
    }

    public void Update(float deltaTime)
    {
        for (int i = 0; i < _destroyListeners.Count; i++)
        {
            Transform destroyListener = _destroyListeners[i];
            if (destroyListener == null)
            {
                _destroyListeners.Remove(destroyListener);
                i--;
                continue;
            }
            
            if (destroyListener.position.y <= _yDestroyValue)
            {
                _destroyListeners.Remove(destroyListener);
                _projectileDestroyer.DestroyProjectile(destroyListener.gameObject);   
            }
        }
    }
}