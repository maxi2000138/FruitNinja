using System;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] 
    private CompositionOrder _compositionOrder;

    private ProjectileShooter _projectileShooter;
    private readonly MonoBehaviourSimulator _monoBehaviourSimulator = new();

    public void Construct(ProjectileShooter projectileShooter)
    {
        _projectileShooter = projectileShooter;
    }

    public void Awake()
    {
        _compositionOrder.CompositeAll(_monoBehaviourSimulator);
        _monoBehaviourSimulator.InitializeAll();
        
        _projectileShooter.StartShooting();
    }

    private void Update()
    {
        _monoBehaviourSimulator.UpdateAll(Time.deltaTime);
    }
}
