using System.Collections.Generic;
using UnityEngine;

public class Mover : PhysicsBehaviour
{
    public Vector2 MovementVector { get; private set; }
    private List<IMover> _movers = new();

    private void Awake()
    {
        _movers.AddRange(GetComponents<IMover>());
    }
    
    public override void ExecuteOperation(GameObject physicsObject)
    {
        Vector2 movementVector = Vector2.zero;
        foreach (IMover mover in _movers)
        {
            movementVector += mover.Move(Time.fixedDeltaTime);
        }

        MovementVector = movementVector;
        physicsObject.transform.Translate(MovementVector, Space.World);
    }
}
