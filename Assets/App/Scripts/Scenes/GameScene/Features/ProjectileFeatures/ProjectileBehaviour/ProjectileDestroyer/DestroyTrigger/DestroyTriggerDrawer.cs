using UnityEngine;

public class DestroyTriggerDrawer : MonoBehaviour
{
    [SerializeField] private GameConfig _gameConfig;
    
    private Vector2 _leftPoint;
    private Vector2 _rightPoint;

    private void OnDrawGizmos()
    {
        _leftPoint = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        _rightPoint = Camera.main.ViewportToWorldPoint(new Vector2(1, 0));
        _leftPoint.y += _gameConfig.DestroyLineYOffset;
        _rightPoint.y += _gameConfig.DestroyLineYOffset;
        Gizmos.color = Color.black;
        Gizmos.DrawLine(_leftPoint, _rightPoint);
    }
}
