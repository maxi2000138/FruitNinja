using UnityEngine;

public class OffsetByTime : MonoBehaviour, IMover
{
    private ChangeData _currentChangeData;
    private bool _isActive = false;
 
    public Vector2 Move(float deltaTime)
    {
        if (_isActive)
        {
            _currentChangeData.CurrentTime += deltaTime;
            return GetOffset(_currentChangeData);
        }
        
        return Vector2.zero;
   
    }

    public void Clear()
    {
        StopOffseting();
    }

    public void StartOffseting(Vector2 startValue, Vector2 finalValue, float flyTime)
    {
        _currentChangeData = new ChangeData(startValue, finalValue, flyTime)
        {
            CurrentTime = 0f,
        };
    }
    
    public void StopOffseting()
    {
        _currentChangeData = null;
    }
    
    private Vector2 GetOffset(ChangeData changeData)
    {
        Vector2 newValue = Vector2.Lerp(changeData.StartValue, changeData.FinalValue, changeData.CurrentTime/changeData.FlyTime);
        Vector2 deltaValue = newValue - _currentChangeData.CurrentValue;
        _currentChangeData.CurrentValue = newValue;
        return deltaValue;
    }
}
