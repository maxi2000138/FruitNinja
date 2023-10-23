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
    private ComboContainer _comboContainer;

    public ComboSystem(Slicer slicer, ComboConfig comboConfig, ComboContainer comboContainer, ScreenSettingsProvider screenSettingsProvider)
    {
        _comboContainer = comboContainer;
        _screenSettingsProvider = screenSettingsProvider;
        _slicer = slicer;
        _comboConfig = comboConfig;
        _slicer.OnSlice += OnComboSlice;
    }
    
    public void SpawnComboPrefab(int comboCount)
    {
        ComboPrefab comboPrefab = GameObject.Instantiate(_comboConfig.ComboPrefab, ConstrainPosition(_projectilePosition, _comboConfig.ComboPrefab.GetComponent<RectTransform>().sizeDelta * _comboConfig.Scale), quaternion.identity, _comboContainer.transform);
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

    private void OnComboSlice(Vector2 projectilePosition)
    {
        _projectilePosition = projectilePosition;
        
        if (_comboStarted)
        {
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
        _comboStarted = false;
        if(combo > 1)
            SpawnComboPrefab(combo);
    }
}