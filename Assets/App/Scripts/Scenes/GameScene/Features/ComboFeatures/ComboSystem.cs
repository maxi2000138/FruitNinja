using System.Threading;
using System.Threading.Tasks;
using App.Scripts.Scenes.GameScene.Features.CameraFeatures.ScreenSettingsProvider;
using App.Scripts.Scenes.GameScene.Features.InputFeatures;
using Unity.Mathematics;
using UnityEngine;

public class ComboSystem
{
    private CancellationTokenSource _cts;
    private bool _comboStarted;
    private int _comboCount;
    private float _lastTime = 0f;
    private readonly Slicer _slicer;
    private readonly ComboConfig _comboConfig;
    private Vector2 _projectilePosition;
    private ScreenSettingsProvider _screenSettingsProvider;
    private ComboParenter _comboParenter;
    private ScoreSystem _scoreSystem;
    private readonly ScoreConfig _scoreConfig;

    public ComboSystem(Slicer slicer, ComboConfig comboConfig, ComboParenter comboParenter, ScreenSettingsProvider screenSettingsProvider, ScoreSystem scoreSystem, ScoreConfig scoreConfig)
    {
        _scoreSystem = scoreSystem;
        _scoreConfig = scoreConfig;
        _comboParenter = comboParenter;
        _screenSettingsProvider = screenSettingsProvider;
        _slicer = slicer;
        _comboConfig = comboConfig;
        _slicer.OnSlice += OnComboSlice;
    }
    
    public void SpawnComboPrefab(int comboCount)
    {
        ComboPrefab comboPrefab = GameObject.Instantiate(_comboConfig.ComboPrefab, ConstrainPosition(_projectilePosition, _comboConfig.ComboPrefab.GetComponent<RectTransform>().sizeDelta * _comboConfig.Scale), quaternion.identity, _comboParenter.transform);
        comboPrefab.transform.localScale = _comboConfig.Scale; 
        comboPrefab.Construct(_comboConfig, comboCount);
    }

    private Vector2 ConstrainPosition(Vector2 position, Vector2 size)
    {
        Vector2 screenPosition = _screenSettingsProvider.WorldToScreenPosition(position);
        screenPosition.x = Mathf.Clamp(screenPosition.x, size.x/2, Screen.width - size.x/2);
        screenPosition.y = Mathf.Clamp(screenPosition.y, size.y/2, Screen.height - size.y/2);
        return _screenSettingsProvider.ScreenToWorldPosition(screenPosition);
    }

    private void OnComboSlice(Vector2 projectilePosition, ProjectileType projectileType)
    {
        _projectilePosition = projectilePosition;
        
        if (_comboStarted)
        {
            if (projectileType != ProjectileType.Fruit)
            {
                _comboStarted = false;
                _cts.Cancel();
                return;
            }
            
            if (Time.time - _lastTime <= _comboConfig.DelayComboDestroy)
            {
                _cts.Cancel();
                _cts = new CancellationTokenSource();
                _lastTime = Time.time;
                ComboCountdown(_comboCount++, _cts.Token);
            }
        }
        else
        {
            _cts = new CancellationTokenSource();
            _comboCount = 1;
            _comboStarted = true;
            _lastTime = Time.time;
            ComboCountdown(_comboCount++, _cts.Token);    
        }
    }

    private async Task ComboCountdown(int combo, CancellationToken cancellationToken)
    {
        await Task.Delay((int)(_comboConfig.DelayComboDestroy*1000), cancellationToken);
        
        if(cancellationToken.IsCancellationRequested)
            return;
        
        _comboStarted = false;
        if (combo > 1)
        {
            SpawnComboPrefab(combo);
            _scoreSystem.AddSliceScore(_scoreConfig.SliceScore * 2 * combo);
        }
    }
}