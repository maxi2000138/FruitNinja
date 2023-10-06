using UnityEngine;

public class EntryPointInstaller : Installer
{
    [SerializeField] 
    private EntryPoint _entryPoint;
    [Header("Installers")]
    [SerializeField] 
    private ServicesInstaller _servicesInstaller;
    
    public override void Compose(MonoBehaviourSimulator monoBehaviourSimulator)
    {
                       
    }
}
