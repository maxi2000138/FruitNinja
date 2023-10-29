using System;
using System.Threading;
using App.Scripts.Scenes.GameScene.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.Shooter.ShootPolicy
{
    public class WavesSpawnPolicy : IShootPolicy
    {
        private CancellationTokenSource cts;
        private float _fruitsIncreaseValue = 1f;
        private float _packDeltaTimeDecreaseValue = 1f;
        private readonly SpawnConfig _spawnConfig;
        private readonly TokenController _tokenController;
        private CancellationToken _groupToken;
        private int _groupNumber;
        private int _lastGroupNumber;

        public event Action NeedShoot;

        public WavesSpawnPolicy(SpawnConfig spawnConfig)
        {
            _spawnConfig = spawnConfig;
            _tokenController = new TokenController();
        }

        public void StartWorking()
        {
            ShootTask();
        }

        public void StopWorking()
        {
            if(cts != null)
                cts.Cancel();
        }

        public void SetIncreasedValues(float fruitIncreaseValue, float packDeltaTimeDecreaseValue)
        {
            if(Math.Abs(_fruitsIncreaseValue - fruitIncreaseValue) > 0.01f)
                _lastGroupNumber = _groupNumber;
            _fruitsIncreaseValue = fruitIncreaseValue;
            _packDeltaTimeDecreaseValue = packDeltaTimeDecreaseValue;
            _tokenController.CancelTokens();
        }
        
        public void ResetIncreasedValues()
        {
            _fruitsIncreaseValue = 1f;
            _packDeltaTimeDecreaseValue = 1f;
            _groupNumber = _lastGroupNumber;
            _tokenController.CancelTokens();
        }

        private async UniTask ShootTask()
        {
            
            float fruitsAmountRange = _spawnConfig.BlocksAmountRange.x;
            float fruitsInGroupSpawnDelay = _spawnConfig.BlocksInGroupSpawnDelayRange.y;
            float groupSpawnDelayRange = _spawnConfig.GroupSpawnDelayRange.y;

            _groupNumber = 0;
        
            while (true)
            {
                _groupNumber++;
                int spawnAmount = (int)fruitsAmountRange;
                for (int i = 0; i < spawnAmount; i++)
                {
                    NeedShoot?.Invoke();
                    bool cancellationThrow = await UniTask.Delay((int)(fruitsInGroupSpawnDelay * 1000), DelayType.DeltaTime, PlayerLoopTiming.Update, _tokenController.CreateCancellationToken())
                        .SuppressCancellationThrow();
                    
                    if(cancellationThrow)
                        break;
                }

                await UniTask.Delay((int)(groupSpawnDelayRange * 1000), DelayType.DeltaTime, PlayerLoopTiming.Update, _tokenController.CreateCancellationToken())
                    .SuppressCancellationThrow();

                fruitsAmountRange = Mathf.Lerp(_spawnConfig.BlocksAmountRange.x, _spawnConfig.BlocksAmountRange.y
                    ,(float)_groupNumber / _spawnConfig.AverageAmountSpawnGroups) * _fruitsIncreaseValue;
                fruitsInGroupSpawnDelay = Mathf.Lerp(_spawnConfig.BlocksInGroupSpawnDelayRange.y, _spawnConfig.BlocksInGroupSpawnDelayRange.x
                    ,(float)_groupNumber / _spawnConfig.AverageAmountSpawnGroups) / _packDeltaTimeDecreaseValue;
                groupSpawnDelayRange = Mathf.Lerp(_spawnConfig.GroupSpawnDelayRange.y, _spawnConfig.GroupSpawnDelayRange.x
                    ,(float)_groupNumber / _spawnConfig.AverageAmountSpawnGroups) / _packDeltaTimeDecreaseValue;
            }
        }
    }
}
