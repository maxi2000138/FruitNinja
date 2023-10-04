using System;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] 
    private CompositionOrder _compositionOrder;

    private readonly MonoBehaviourSimulator _monoBehaviourSimulator = new();

    public void Awake()
    {
        _compositionOrder.CompositeAll(_monoBehaviourSimulator);
        _monoBehaviourSimulator.InitializeAll();
    }

    private void Update()
    {
        _monoBehaviourSimulator.UpdateAll(Time.deltaTime);
    }
}
