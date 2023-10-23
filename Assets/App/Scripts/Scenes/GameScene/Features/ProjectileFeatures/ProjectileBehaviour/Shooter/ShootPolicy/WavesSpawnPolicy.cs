using System;
using System.Threading;
using System.Threading.Tasks;
using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.Infrastructure.MonoInterfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.Shooter.ShootPolicy
{
    public class WavesSpawnPolicy : IShootPolicy
    {
        private CancellationTokenSource cts;
        private readonly SpawnConfig _spawnConfig;

        public WavesSpawnPolicy(SpawnConfig spawnConfig)
        {
            _spawnConfig = spawnConfig;
        }
        public event Action NeedShoot;
        public void StartWorking()
        {
            cts = new CancellationTokenSource();
            ShootTask(cts.Token);
        }

        public void StopWorking()
        {
            if(cts != null)
                cts.Cancel();
        }

        public void OnWinGame()
        {
            StopWorking();   
        }

        public void OnLooseGame()
        {
            StopWorking();   
        }

        private async Task ShootTask(CancellationToken token)
        {
            float fruitsAmountRange = _spawnConfig.BlocksAmountRange.x;
            float fruitsInGroupSpawnDelay = _spawnConfig.BlocksInGroupSpawnDelayRange.y;
            float groupSpawnDelayRange = _spawnConfig.GroupSpawnDelayRange.y;

            int groupNumber = 0;
        
            while (true)
            {
                groupNumber++;
                int spawnAmount = (int)fruitsAmountRange;
                for (int i = 0; i < spawnAmount; i++)
                {
                    if(token.IsCancellationRequested)
                        return;
                    
                    NeedShoot?.Invoke();
                    await UniTask.Delay((int)(fruitsInGroupSpawnDelay * 1000), DelayType.DeltaTime, PlayerLoopTiming.Update, cts.Token);
                }

                if(token.IsCancellationRequested)
                    return;
                await UniTask.Delay((int)(groupSpawnDelayRange * 1000), DelayType.DeltaTime, PlayerLoopTiming.Update, token);
                fruitsAmountRange = Mathf.Lerp(_spawnConfig.BlocksAmountRange.x, _spawnConfig.BlocksAmountRange.y
                    ,(float)groupNumber / _spawnConfig.AverageAmountSpawnGroups);
                fruitsInGroupSpawnDelay = Mathf.Lerp(_spawnConfig.BlocksInGroupSpawnDelayRange.y, _spawnConfig.BlocksInGroupSpawnDelayRange.x
                    ,(float)groupNumber / _spawnConfig.AverageAmountSpawnGroups);
                groupSpawnDelayRange = Mathf.Lerp(_spawnConfig.GroupSpawnDelayRange.y, _spawnConfig.GroupSpawnDelayRange.x
                    ,(float)groupNumber / _spawnConfig.AverageAmountSpawnGroups);
            }
        }
    }
}
