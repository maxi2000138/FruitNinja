using UnityEngine;

public class SliceBlock : MonoBehaviour
{
    [field: SerializeField] public ForceApplier LeftPartForceApplier { get; private set; }
    [field: SerializeField] public ForceApplier RightPartForceApplier { get; private set; }
    [field: SerializeField] public GravitationApplier GravitationApplier { get; private set; }
    [field: SerializeField] public TorqueApplier TorqueApplier { get; private set; }
    [field: SerializeField] public ForceApplier ForceApplier { get; private set; }

    public void Slice(Vector2 sliceVector, float sliceForce)
    {
        TorqueApplier.Clear();
        ForceApplier.Clear();
        GravitationApplier.Clear();

        Vector2 perpendicularVector = Vector2.Perpendicular(sliceVector) * sliceForce;
        Vector2 up = Vector2.zero;
        Vector2 down = Vector2.zero;
        
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
        
        //RightPartForceApplier.AddForce(-Vector2.Perpendicular(sliceVector) * sliceForce);
    }
}