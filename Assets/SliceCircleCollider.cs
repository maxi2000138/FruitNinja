using UnityEngine;

public class SliceCircleCollider : MonoBehaviour
{
    [field:SerializeField] public SliceBlock SliceBlock { get; private set; }
    [field:SerializeField] public Transform _colliderCenter { get; private set; }
    
    private SliceCollidersController _sliceCollidersController;
    private bool _isActive;

    public void Construct(SliceCollidersController sliceCollidersController)
    {
        _sliceCollidersController = sliceCollidersController;
        _sliceCollidersController.AddCollider(this);
        _isActive = true;
    }

    public void Disable()
    {
        _isActive = false;
    }

    private void OnDestroy()
    {
        _sliceCollidersController.RemoveCollider(this);
    }

    private void Update()
    {
        DebugDrawer.DrawCircle(_colliderCenter.transform.position, _colliderCenter.transform.localScale.magnitude, 100, Color.black);
    }

    public bool IsPointInsideCollider(Vector2 point)
    {
        if (!_isActive)
            return false;
        
        return (point - (Vector2)_colliderCenter.transform.position).magnitude <= _colliderCenter.transform.localScale.magnitude;
    }
}
