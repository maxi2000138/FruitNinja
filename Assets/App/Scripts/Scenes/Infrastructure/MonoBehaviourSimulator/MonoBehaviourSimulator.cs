using System.Collections.Generic;
using App.Scripts.Scenes.Infrastructure.MonoInterfaces;
using UnityEngine;

namespace App.Scripts.Scenes.Infrastructure.MonoBehaviourSimulator
{
    public class MonoBehaviourSimulator : MonoBehaviour
    {
        private readonly List<IInitializable> _initializables = new();
        private readonly List<IUpdatable> _updatables = new();
        private readonly List<IDestroyable> _destroyables = new();

        private void Update()
        {
            UpdateAll(Time.deltaTime);
        }

        public void InitializeAll()
        {

            for (int i = 0; i < _initializables.Count; i++)
            {
                _initializables[i]?.Initialize();
            }
        }

        private void UpdateAll(float deltaTime)
        {
            for (int i = 0; i < _updatables.Count; i++)
            {
                _updatables[i]?.Update(deltaTime);
            }
        }

        private void DestroyAll()
        {
            for (int i = 0; i < _destroyables.Count; i++)
            {
                _destroyables[i]?.OnDestroy();
            }
        }

        public void AddInitializable(IInitializable initializable) => 
            _initializables.Add(initializable);
        public void AddUpdatable(IUpdatable updatable) => 
            _updatables.Add(updatable);
        public void AddDestroyable(IDestroyable destroyable) => 
            _destroyables.Add(destroyable);
    }
}