using UnityEngine;

public class CloneRotater : PhysicsBehaviour
{
    private GameObject _cloneObject;

    public void Construct(GameObject cloneObject)
    {
        _cloneObject = cloneObject;
    }

    public override void ExecuteOperation(GameObject physicsObject)
    {
        if (_cloneObject != null)
        {
            _cloneObject.transform.rotation = physicsObject.transform.rotation;
        }
    }
}