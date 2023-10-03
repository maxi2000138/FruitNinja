using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private List<IMover> _movers = new();

    private void Awake()
    {
        _movers.AddRange(GetComponents<IMover>());
    }

    private void FixedUpdate()
    {
        Vector2 movementVector = Vector2.zero;
        foreach (IMover mover in _movers)
        {
            movementVector += mover.Move(Time.fixedDeltaTime);
        }

        Vector2 position = transform.position;
        position += movementVector;
        transform.position = position;
    }
}
