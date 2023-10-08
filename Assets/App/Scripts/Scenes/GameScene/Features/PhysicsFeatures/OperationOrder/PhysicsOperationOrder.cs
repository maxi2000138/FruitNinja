using System.Collections.Generic;
using UnityEngine;

public class PhysicsOperationOrder : MonoBehaviour
{
    [SerializeField] private GameObject _physicsObject;
    [SerializeField] private List<PhysicsBehaviour> _physicOperations;

    private void FixedUpdate()
    {
        for (int i = 0; i < _physicOperations.Count; i++)
        {
            if(_physicOperations[i] != null)
                _physicOperations[i].ExecuteOperation(_physicsObject);
        }
    }
}
