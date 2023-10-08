using UnityEngine;

public class CloneMover : PhysicsBehaviour
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
            Vector3 newPosition = _cloneObject.transform.position;
            (newPosition.x, newPosition.y) = (physicsObject.transform.position.x, physicsObject.transform.position.y);
            _cloneObject.transform.position = newPosition;
        }
    }
}
