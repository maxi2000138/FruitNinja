using System.Collections.Generic;
using UnityEngine;

public class Rotater : PhysicsBehaviour
{
    private List<IRotater> _rotaters = new();

    private void Awake()
    {
        _rotaters.AddRange(GetComponents<IRotater>());
    }
    
    public override void ExecuteOperation(GameObject physicsObject)
    {
        Vector3 rotationVector = Vector3.zero;
        foreach (IRotater mover in _rotaters)
        {
            rotationVector += mover.Rotate(Time.fixedDeltaTime);
        }

        physicsObject.transform.Rotate(rotationVector);
    }
}