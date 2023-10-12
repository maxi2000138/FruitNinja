using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesData;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesApplier
{
    public class OffsetByTimeApplier : MonoBehaviour, IMover
    {
        private ChangedByTimeData _currentChangedByTimeData;
        private bool _isActive = false;
 
        public Vector2 Move(float deltaTime)
        {
            if (_isActive)
            {
                _currentChangedByTimeData.CurrentTime += deltaTime;
                return GetOffset(_currentChangedByTimeData);
            }
        
            return Vector2.zero;
   
        }

        public void Clear()
        {
            StopOffseting();
        }

        public void StartOffseting(Vector2 startValue, Vector2 finalValue, float flyTime)
        {
            _currentChangedByTimeData = new ChangedByTimeData(startValue, finalValue, flyTime)
            {
                CurrentTime = 0f,
            };
        }
    
        public void StopOffseting()
        {
            _currentChangedByTimeData = null;
        }
    
        private Vector2 GetOffset(ChangedByTimeData changedByTimeData)
        {
            Vector2 newValue = Vector2.Lerp(changedByTimeData.StartValue, changedByTimeData.FinalValue, changedByTimeData.CurrentTime/changedByTimeData.FlyTime);
            Vector2 deltaValue = newValue - _currentChangedByTimeData.CurrentValue;
            _currentChangedByTimeData.CurrentValue = newValue;
            return deltaValue;
        }
    }
}
