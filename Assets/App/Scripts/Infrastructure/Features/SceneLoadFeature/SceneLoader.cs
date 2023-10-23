using System;
using System.Collections;
using System.Threading.Tasks;
using App.Scripts.Scenes.Infrastructure.CoroutineRunner;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner) => 
            _coroutineRunner = coroutineRunner;

        public async UniTask Load(string name, Action onLoaded = null) =>
            await LoadScene(name, onLoaded);
    
        private async UniTask LoadScene(string nextScene, Action onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoaded?.Invoke();
                return;
            }
            
                
            await SceneManager.LoadSceneAsync(nextScene);
            onLoaded?.Invoke();
        }
    }
}