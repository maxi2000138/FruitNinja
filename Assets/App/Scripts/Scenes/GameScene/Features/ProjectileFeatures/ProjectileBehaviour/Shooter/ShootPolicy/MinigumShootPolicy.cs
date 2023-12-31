using System;
using System.Collections;
using App.Scripts.Scenes.Infrastructure.CoroutineRunner;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.Shooter.ShootPolicy
{
    public class MinigumShootPolicy : IShootPolicy
    {
        public event Action NeedShoot;
        private Coroutine _shootCoroutine;
        private readonly ICoroutineRunner _coroutineRunner;

        public MinigumShootPolicy(ICoroutineRunner coroutineRunner)
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

        public void OnWinGame()
        {
            StopWorking();
        }

        public void OnLooseGame()
        {
            StopWorking();
        }
    }
}
