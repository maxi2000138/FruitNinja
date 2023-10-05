using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    private List<IRotater> _rotaters = new();

    private void Awake()
    {
        _rotaters.AddRange(GetComponents<IRotater>());
    }

    private void FixedUpdate()
    {
        Vector3 rotationVector = Vector3.zero;
        foreach (IRotater mover in _rotaters)
        {
            rotationVector += mover.Rotate(Time.fixedDeltaTime);
        }

        transform.Rotate(rotationVector);
    }
}