using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Scenes.Infrastructure.CompositeRoot
{
    class InstallersOrder : MonoBehaviour
    {
        [SerializeField] 
        private List<InstallerBehaviour> _order;

        public void CompositeAll(MonoBehaviourSimulator.MonoBehaviourSimulator monoBehaviourSimulator)
        {
            foreach (var installer in _order)
            {
                installer.InstallBindings(monoBehaviourSimulator);
            }
        }
    }
}
