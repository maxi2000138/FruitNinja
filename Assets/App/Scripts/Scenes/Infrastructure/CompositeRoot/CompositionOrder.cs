using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Scenes.Infrastructure.CompositeRoot
{
    class CompositionOrder : MonoBehaviour
    {
        [SerializeField] 
        private List<Installer> _order;

        public void CompositeAll(MonoBehaviourSimulator.MonoBehaviourSimulator monoBehaviourSimulator)
        {
            foreach (var compositionRoot in _order)
            {
                compositionRoot.Compose(monoBehaviourSimulator);
            }
        }
    }
}
