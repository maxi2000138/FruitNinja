using System;
using System.Collections;
using UnityEngine;

public class NormSpawnPolicy : IShootPolicy
{
    private Coroutine _shootCoroutine;
    private readonly CoroutineRunner _coroutineRunner;
    private readonly SpawnConfig _spawnConfig;

    public NormSpawnPolicy(CoroutineRunner coroutineRunner, SpawnConfig spawnConfig)
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
        while (true)
        {
            int spawnAmount = _spawnConfig.FruitsAmountRange.GetRandomIntBetween();
            for (int i = 0; i < spawnAmount; i++)
            {
                NeedShoot?.Invoke();
                yield return new WaitForSeconds(_spawnConfig.FruitsInGroupSpawnDelayRange.GetRandomFloatBetween());
            }
            yield return new WaitForSeconds(_spawnConfig.GroupSpawnDelayRange.GetRandomFloatBetween());
        }
    }
}
