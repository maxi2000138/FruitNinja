using UnityEngine;

public class SliceBlock : MonoBehaviour
{
    [field: SerializeField] public ForceApplier LeftPartForceApplier { get; private set; }
    [field: SerializeField] public ForceApplier RightPartForceApplier { get; private set; }
    [field: SerializeField] public GravitationApplier GravitationApplier { get; private set; }
    [field: SerializeField] public TorqueApplier TorqueApplier { get; private set; }
    [field: SerializeField] public ForceApplier ForceApplier { get; private set; }
    [field: SerializeField] public ParticleSystemController ParticleSystemController { get; private set; }
    
    

    public void Slice(Vector2 sliceVector, float sliceForce)
    {
        TorqueApplier.Clear();
        ForceApplier.Clear();
        GravitationApplier.Clear();

        float zAngle = Vector2.Angle(Vector2.up, sliceVector);
        Vector3 eulerAngles = transform.localEulerAngles;
        eulerAngles.z = zAngle;
        transform.localEulerAngles = eulerAngles;

        Vector2 perpendicularVector = Vector2.Perpendicular(sliceVector) * sliceForce;
        Vector2 up = Vector2.zero;
        Vector2 down = Vector2.zero;
        
        ParticleSystemController.PlayAll();
        
        if (perpendicularVector.y >= 0)
        {
            up = perpendicularVector;
            down = -perpendicularVector;
        }
        else
        {
            up = -perpendicularVector;
            down = perpendicularVector;
        }

        if (LeftPartForceApplier.transform.position.y > RightPartForceApplier.transform.position.y)
        {
            LeftPartForceApplier.AddForce(up);
            RightPartForceApplier.AddForce(down);
        }
        else
        {
            LeftPartForceApplier.AddForce(down);
            RightPartForceApplier.AddForce(up);
        }
    }
}