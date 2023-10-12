using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.PhysicsFramework;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Scaler
{
    public class ResultForceScaler : PhysicsBehaviour
    {
        private List<IScaler> _rotaters = new();

        private void Awake()
        {
            _rotaters.AddRange(GetComponents<IScaler>());
        }
    
        public override void ExecuteOperation(GameObject physicsObject)
        {
            Vector2 scaleVector = Vector2.zero;
            foreach (IScaler scaler in _rotaters)
            {
                scaleVector += scaler.Scale(Time.fixedDeltaTime);
            }

            Vector2 transformLocalScale = physicsObject.transform.localScale;
            physicsObject.transform.localScale = transformLocalScale + scaleVector;
        }

    }
}