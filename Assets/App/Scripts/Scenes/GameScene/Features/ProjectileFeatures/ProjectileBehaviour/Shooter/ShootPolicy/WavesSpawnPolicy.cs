using System;
using System.Threading;
using System.Threading.Tasks;
using App.Scripts.Scenes.GameScene.Configs;
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
            float fruitsAmountRange = _spawnConfig.FruitsAmountRange.x;
            float fruitsInGroupSpawnDelay = _spawnConfig.FruitsInGroupSpawnDelayRange.y;
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
                    await Task.Delay((int)(fruitsInGroupSpawnDelay*1000), token);
                }

                if(token.IsCancellationRequested)
                    return;

                await Task.Delay((int)(groupSpawnDelayRange * 1000), token);

                fruitsAmountRange = Mathf.Lerp(_spawnConfig.FruitsAmountRange.x, _spawnConfig.FruitsAmountRange.y
                    ,(float)groupNumber / _spawnConfig.AverageAmountSpawnGroups);
                fruitsInGroupSpawnDelay = Mathf.Lerp(_spawnConfig.FruitsInGroupSpawnDelayRange.y, _spawnConfig.FruitsInGroupSpawnDelayRange.x
                    ,(float)groupNumber / _spawnConfig.AverageAmountSpawnGroups);
                groupSpawnDelayRange = Mathf.Lerp(_spawnConfig.GroupSpawnDelayRange.y, _spawnConfig.GroupSpawnDelayRange.x
                    ,(float)groupNumber / _spawnConfig.AverageAmountSpawnGroups);
            }
        }

    }
}
