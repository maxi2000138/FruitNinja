using System;
using App.Scripts.Scenes.Infrastructure.MonoInterfaces;

public class SaveTimeTrigger : ISavedTrigger, IUpdatable
{
    public event Action NeedSave;

    private float _saveTime = 2f;
    private float _currentTime = 0f;

    public void Update(float deltaTime)
    {
        _currentTime += deltaTime;
     
        if (_currentTime >= _saveTime)
        {
            _currentTime -= _saveTime;
            NeedSave?.Invoke();
        }
    }
}
