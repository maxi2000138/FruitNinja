using UnityEngine;

public class ConfigsInstaller : Installer
{
    [field: SerializeField] 
    public GameConfig GameConfig { get; private set; }
    [field: SerializeField] 
    public FruitConfig FruitConfig { get; private set; }
    [field: SerializeField] 
    public ProjectileConfig ProjectileConfig { get; private set; }
    [field: SerializeField] 
    public ResourcesConfig ResourcesConfig { get; private set; }


    public override void Compose(MonoBehaviourSimulator monoBehaviourSimulator)
    {
        
    }
}
