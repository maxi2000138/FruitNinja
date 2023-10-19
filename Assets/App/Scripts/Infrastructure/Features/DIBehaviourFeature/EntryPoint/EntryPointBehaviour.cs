using App.Scripts.Scenes.Infrastructure.CompositeRoot;
using App.Scripts.Scenes.Infrastructure.MonoBehaviourSimulator;
using UnityEngine;

public abstract class EntryPointBehaviour : MonoBehaviour
{
    [SerializeField] 
    private InstallersOrder _installersOrder;
    [SerializeField] 
    private MonoBehaviourSimulator _monoBehaviourSimulator;


    private void Start()
    {
        BaseBehaviour();
        OnEntryPoint();
    }

    public abstract void OnEntryPoint();

    private void BaseBehaviour()
    {
        _installersOrder.CompositeAll(_monoBehaviourSimulator);
        _monoBehaviourSimulator.InitializeAll();
    }
}
