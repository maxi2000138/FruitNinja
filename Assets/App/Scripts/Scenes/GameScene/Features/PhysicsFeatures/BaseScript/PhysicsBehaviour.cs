using System.Net.NetworkInformation;
using UnityEngine;

public abstract class PhysicsBehaviour : MonoBehaviour
{
    private bool _isActive = true;

    public void Enable()
    {
        _isActive = true;
    }

    public void Disable()
    {
        _isActive = false;
    }

    public void ExecuteOperation(GameObject physicsObject)
    {
        if(!_isActive)
            return;

        Operation(physicsObject);
    }

    public abstract void Operation(GameObject physicsObject);
}
