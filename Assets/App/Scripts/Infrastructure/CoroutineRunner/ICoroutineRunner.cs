using System.Collections;
using UnityEngine;

namespace App.Scripts.Scenes.Infrastructure.CoroutineRunner
{
    public interface ICoroutineRunner
    {
        public Coroutine StartCoroutine(IEnumerator routine);
        public void StopCoroutine(Coroutine routine);
    }
}