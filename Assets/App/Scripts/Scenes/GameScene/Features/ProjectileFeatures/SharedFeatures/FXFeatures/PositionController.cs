using UnityEngine;

public class PositionController : MonoBehaviour
{
    private Transform _controlledTransform;
    public void SetControllingTransform(Transform controlledTransform)
    {
        _controlledTransform = controlledTransform;
    }

    private void Update()
    {
        if(_controlledTransform != null)
            transform.position = _controlledTransform.position;
    }
}
