using System;
using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Configs;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.PhysicsFramework
{
    public class PhysicsForcesOrder : MonoBehaviour
    {
        [SerializeField] private GameObject _physicsObject;
        [SerializeField] private List<PhysicsBehaviour> _physicOperations;
        
        private TimeScaleService _timeScaleService;

        public void Construct(TimeScaleService timeScaleService)
        {
            _timeScaleService = timeScaleService;
        }

        private void Update()
        {
            for (int i = 0; i < _physicOperations.Count; i++)
            {
                if(_physicOperations[i] != null)
                    _physicOperations[i].ExecuteOperation(_physicsObject, Time.fixedDeltaTime, _timeScaleService.PhysicTimeScale);
            }
        }
    }
}
