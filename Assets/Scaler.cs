using System.Collections.Generic;
using UnityEngine;

public class Scaler : PhysicsBehaviour
{
    private List<IScaler> _rotaters = new();

    private void Awake()
    {
        _rotaters.AddRange(GetComponents<IScaler>());
    }
    
    public override void Operation(GameObject physicsObject)
    {
        Vector2 scaleVector = Vector2.zero;
        foreach (IScaler scaler in _rotaters)
        {
            scaleVector += scaler.Scale(Time.fixedDeltaTime);
        }

        Vector2 transformLocalScale = physicsObject.transform.localScale;
        physicsObject.transform.localScale = transformLocalScale + scaleVector;
    }

}