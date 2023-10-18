using UnityEngine;

namespace App.Scripts.Scenes.Infrastructure.CompositeRoot
{
    public abstract class InstallerBehaviour : MonoBehaviour
    {
        public void InstallBindings(MonoBehaviourSimulator.MonoBehaviourSimulator monoBehaviourSimulator)
        {
            ProjectInstaller projectInstaller = FindObjectOfType<ProjectInstaller>();
            OnInstallBindings(monoBehaviourSimulator, projectInstaller);

        }

        public abstract void OnInstallBindings(MonoBehaviourSimulator.MonoBehaviourSimulator monoBehaviourSimulator, ProjectInstaller projectInstaller);
    }
}