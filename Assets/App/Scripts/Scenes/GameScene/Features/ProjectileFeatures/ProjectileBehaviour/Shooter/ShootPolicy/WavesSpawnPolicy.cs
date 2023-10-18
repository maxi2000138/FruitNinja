using System;
using System.Collections;
using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.Infrastructure.CoroutineRunner;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.Shooter.ShootPolicy
{
    public class WavesSpawnPolicy : IShootPolicy
    {
        private Coroutine _shootCoroutine;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly SpawnConfig _spawnConfig;

        public WavesSpawnPolicy(ICoroutineRunner coroutineRunner, SpawnConfig spawnConfig)
        {
            _coroutineRunner = coroutineRunner;
            _spawnConfig = spawnConfig;
        }
        public event Action NeedShoot;
        public void StartWorking()
        {
            _shootCoroutine = _coroutineRunner.StartCoroutine(ShootCoroutine());
        }

        public void StopWorking()
        { 
            if(_shootCoroutine != null)
                _coroutineRunner.StopCoroutine(_shootCoroutine);
        }

        private IEnumerator ShootCoroutine()
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
                    NeedShoot?.Invoke();
                    yield return new WaitForSeconds(fruitsInGroupSpawnDelay);
                }

                yield return new WaitForSeconds(groupSpawnDelayRange);

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
