using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class SpawnAreaHandler
{
    public SpawnAreaData SpawnAreaData => _spawnAreaData;
    
    [SerializeField] private Color32 _lineColor;
    [SerializeField, Range(0.05f, 0.5f)] private float _angleLineLength;
    [SerializeField ,FormerlySerializedAs("SpawnAreaData")] private SpawnAreaData _spawnAreaData;

    private Vector2 _cameraStartPoint;
    private Vector2 _leftPoint;
    private Vector2 _rightPoint;
    private Vector2 MinAnglePoint;
    private Vector2 MaxAnglePoint;
    private float _cameraWidth;
    private float _cameraHeight;
    private Vector2 _point;
    
    public void Validate()
    {
        if (_spawnAreaData.ShootMaxAngle < _spawnAreaData.ShootMinAngle)
            _spawnAreaData.ShootMaxAngle = _spawnAreaData.ShootMinAngle;
        
        _cameraStartPoint = Camera.main.ScreenToWorldPoint(Vector2.zero);
        _cameraHeight = Camera.main.orthographicSize * 2;
        _cameraWidth = _cameraHeight * Camera.main.aspect;
        _point = new Vector2(_cameraStartPoint.x + (_cameraWidth * _spawnAreaData.ViewportPositionX), _cameraStartPoint.y + (_cameraHeight * _spawnAreaData.ViewportPositionY));
        float deltaY = _cameraWidth * (float)Mathf.Sin(_spawnAreaData.LineAngle * Mathf.PI/180) * _spawnAreaData.LineLength;  
        float deltaX = _cameraWidth * (float)Mathf.Cos(_spawnAreaData.LineAngle * Mathf.PI/180) * _spawnAreaData.LineLength;  
        float deltaY1 = _cameraWidth * (float)Mathf.Sin((_spawnAreaData.LineAngle + _spawnAreaData.ShootMinAngle) * Mathf.Deg2Rad) * _angleLineLength;  
        float deltaY2 = _cameraWidth * (float)Mathf.Sin((_spawnAreaData.LineAngle + _spawnAreaData.ShootMaxAngle) * Mathf.Deg2Rad) * _angleLineLength;  
        float deltaX1 = _cameraWidth * (float)Mathf.Cos((_spawnAreaData.LineAngle + _spawnAreaData.ShootMinAngle) * Mathf.Deg2Rad)  * _angleLineLength;  
        float deltaX2 = _cameraWidth * (float)Mathf.Cos((_spawnAreaData.LineAngle + _spawnAreaData.ShootMaxAngle) * Mathf.Deg2Rad)  * _angleLineLength;
        _leftPoint = new Vector2(_point.x - deltaX, _point.y - deltaY); 
        _rightPoint = new Vector2(_point.x + deltaX, _point.y + deltaY);
        _spawnAreaData.ViewportLeftPosition = Camera.main.WorldToViewportPoint(_leftPoint);
        _spawnAreaData.ViewportRightPosition = Camera.main.WorldToViewportPoint(_rightPoint);
        MinAnglePoint = new Vector2(_point.x + deltaX1, _point.y + deltaY1); 
        MaxAnglePoint = new Vector2(_point.x + deltaX2, _point.y + deltaY2); 
    }

    public void DrawGizmos()
    {
        Gizmos.color = _lineColor;
        DrawArrow(_point, _rightPoint - _point, _lineColor);
        DrawArrow(_point, _leftPoint - _point, _lineColor);
        Gizmos.DrawLine(_point,MinAnglePoint);
        Gizmos.DrawLine(_point,MaxAnglePoint);
    }
    
    public static void DrawArrow(Vector3 pos, Vector3 dir, Color color, float arrowheadLength = 0.25f, float arrowheadAngle = 20.0f)
    {
        Gizmos.color = color;
        Gizmos.DrawRay(pos, dir);
       
        Vector3 up = Quaternion.LookRotation(dir) * new Vector3(0f, Mathf.Sin(arrowheadAngle* Mathf.Deg2Rad), -1f);
        Vector3 down = Quaternion.LookRotation(dir) * new Vector3(0f, -Mathf.Sin(arrowheadAngle* Mathf.Deg2Rad), -1f);
        Vector3 left= Quaternion.LookRotation(dir) * new Vector3(Mathf.Sin(arrowheadAngle* Mathf.Deg2Rad), 0f, -1f);
        Vector3 right = Quaternion.LookRotation(dir) * new Vector3(-Mathf.Sin(arrowheadAngle* Mathf.Deg2Rad), 0f, -1f);
        Gizmos.DrawRay(pos + dir, left * arrowheadLength);
        Gizmos.DrawRay(pos + dir, right * arrowheadLength);
        Gizmos.DrawRay(pos + dir, up * arrowheadLength);
        Gizmos.DrawRay(pos + dir, down * arrowheadLength);
    }
}
