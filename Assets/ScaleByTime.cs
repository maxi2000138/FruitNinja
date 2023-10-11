using UnityEngine;

public class ScaleByTime : MonoBehaviour, IScaler
{
    private ChangeData _currentChangeData;
    private bool _isActive = false;
    
    public Vector2 Scale(float deltaTime)
    {
        if (_isActive)
        {
            _currentChangeData.CurrentTime += deltaTime;
            return GetScaling(_currentChangeData);
        }
        
        return Vector2.zero;
    }

    public void StartScaling(Vector2 startValue, Vector2 finalValue, float flyTime)
    {
        if(_isActive)
            return;

        _isActive = true;
        _currentChangeData = new ChangeData(startValue, finalValue, flyTime)
        {
            CurrentTime = 0f,
            CurrentValue = startValue,
        };
    }
    
    public void StopScaling()
    {
        if(!_isActive)
            return;

        _isActive = false;
    }
    
    private Vector2 GetScaling(ChangeData changeData)
    {
        Vector2 newValue = Vector2.Lerp(changeData.StartValue, changeData.FinalValue, changeData.CurrentTime/changeData.FlyTime);
        Vector2 deltaValue = newValue - _currentChangeData.CurrentValue;
        _currentChangeData.CurrentValue = newValue;
        return deltaValue;
    }
}
