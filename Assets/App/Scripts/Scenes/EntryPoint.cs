using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] 
    private Initializer _initializer;
    [SerializeField] 
    private CompositionOrder _compositionOrder;
 
    public void Start()
    {
        _compositionOrder.CompositeAll();
        _initializer.Initialize();
    }
}
