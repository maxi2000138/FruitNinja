using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesData
{
    public class ChangedByTimeData
    {
        public Vector2 StartValue;
        public Vector2 CurrentValue;
        public Vector2 FinalValue;
        public float FlyTime;
        public float CurrentTime;

        public ChangedByTimeData(Vector2 startValue, Vector2 finalValue, float flyTime)
        {
            StartValue = startValue;
            FinalValue = finalValue;
            FlyTime = flyTime;
        }
    }
}