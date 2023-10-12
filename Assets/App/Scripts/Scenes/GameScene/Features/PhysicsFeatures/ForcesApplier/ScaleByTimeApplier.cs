using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesData;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Scaler;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesApplier
{
    public class ScaleByTimeApplier : MonoBehaviour, IScaler
    {
        private ChangedByTimeData _currentChangedByTimeData;
        private bool _isActive = false;
    
        public Vector2 Scale(float deltaTime)
        {
            if (_isActive)
            {
                _currentChangedByTimeData.CurrentTime += deltaTime;
                return GetScaling(_currentChangedByTimeData);
            }
        
            return Vector2.zero;
        }

        public void StartScaling(Vector2 startValue, Vector2 finalValue, float flyTime)
        {
            if(_isActive)
                return;

            _isActive = true;
            _currentChangedByTimeData = new ChangedByTimeData(startValue, finalValue, flyTime)
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
    
        private Vector2 GetScaling(ChangedByTimeData changedByTimeData)
        {
            Vector2 newValue = Vector2.Lerp(changedByTimeData.StartValue, changedByTimeData.FinalValue, changedByTimeData.CurrentTime/changedByTimeData.FlyTime);
            Vector2 deltaValue = newValue - _currentChangedByTimeData.CurrentValue;
            _currentChangedByTimeData.CurrentValue = newValue;
            return deltaValue;
        }
    }
}
