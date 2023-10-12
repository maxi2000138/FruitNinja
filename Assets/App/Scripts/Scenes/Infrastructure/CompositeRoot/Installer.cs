using UnityEngine;

namespace App.Scripts.Scenes.Infrastructure.CompositeRoot
{
    public abstract class Installer : MonoBehaviour
    {
        public abstract void Compose(MonoBehaviourSimulator.MonoBehaviourSimulator monoBehaviourSimulator);
    }
}