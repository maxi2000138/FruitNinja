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
            yield return new WaitForSeconds(Random.Range(0.3f, 0.7f));
            NeedShoot?.Invoke();
        }
    }
}
