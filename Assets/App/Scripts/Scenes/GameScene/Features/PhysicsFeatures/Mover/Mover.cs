using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private List<IMover> _movers = new();
    private Vector2 _lastMovementVector = Vector2.zero;

    private void Awake()
    {
        _movers.AddRange(GetComponents<IMover>());
    }

    private void FixedUpdate()
    {
        Vector2 movementVector = Vector2.zero;
        foreach (IMover mover in _movers)
        {
            movementVector += mover.Move(Time.fixedDeltaTime, _lastMovementVector);
        }

        _lastMovementVector = movementVector;
        transform.Translate(_lastMovementVector, Space.World);
    }
}
