using UnityEngine;

namespace App.Scripts.Scenes.Infrastructure.CompositeRoot
{
    public abstract class InstallerBehaviour : MonoBehaviour
    {
        public abstract void InstallBindings(MonoBehaviourSimulator.MonoBehaviourSimulator monoBehaviourSimulator);
    }
}