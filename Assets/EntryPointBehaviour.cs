using UnityEngine;

public abstract class EntryPointBehaviour : MonoBehaviour
{
    private void Start()
    {
        OnEntryPoint();
    }

    public abstract void OnEntryPoint();
}
