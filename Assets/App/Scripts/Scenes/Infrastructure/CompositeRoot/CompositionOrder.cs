using UnityEngine;
using System.Collections.Generic;

class CompositionOrder : MonoBehaviour
{
    [SerializeField] 
    private List<Installer> _order;

    public void CompositeAll(MonoBehaviourSimulator monoBehaviourSimulator)
    {
        foreach (var compositionRoot in _order)
        {
            compositionRoot.Compose(monoBehaviourSimulator);
        }
    }
}
