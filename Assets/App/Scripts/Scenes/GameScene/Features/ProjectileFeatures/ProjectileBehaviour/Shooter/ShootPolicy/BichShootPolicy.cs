using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BichShootPolicy : IShootPolicy
{
    public event Action NeedShoot;
    private Coroutine _shootCoroutine;
    private readonly ICoroutineRunner _coroutineRunner;

    public BichShootPolicy(ICoroutineRunner coroutineRunner)
    {
        _coroutineRunner = coroutineRunner;
    }

    public void StartWorking()
    {
        StopWorking();
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
            NeedShoot?.Invoke();
            yield return new WaitForSeconds(Random.Range(1f, 1.5f));
        }
    }
}
