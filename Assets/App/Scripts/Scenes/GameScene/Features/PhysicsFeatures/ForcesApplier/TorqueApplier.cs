using UnityEngine;

public class TorqueApplier : MonoBehaviour, IRotater
{
    private Vector3 _torque = Vector3.zero;
    
    public void AddTorque(float torqueValue)
    {
        _torque.z += torqueValue;
    }

    public void Clear()
    {
        _torque = Vector3.zero;
    }
    
    public Vector3 Rotate(float deltaTime)
    {
        return _torque * deltaTime;
    }
}