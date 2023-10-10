using UnityEngine;

public class CircleCollider : MonoBehaviour
{
    private void Update()
    {
        DebugDrawer.DrawCircle(transform.position, 1f, 100, Color.black);
    }
}
