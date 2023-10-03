using System;
using UnityEngine;

[Serializable]
public class SpawnAreaData
{
    [SerializeField]
    private Color32 _lineColor;
    [SerializeField, Range(0,360)]
    private float _lineAngle;
    [SerializeField, Range(0,180)]
    private float _shootMinAngle;
    [SerializeField, Range(0,180)]
    private float _shootMaxAngle; 
    [SerializeField, Range(0,1)]
    private float _viewportPositionX;
    [SerializeField, Range(0,1)]
    private float _viewportPositionY;
    [SerializeField, Range(0,1)]
    private float _length;
    [SerializeField, Range(0.05f, 0.5f)] 
    private float _angleLineLength;

    public Vector2 Point { get; private set; }
    private Vector2 _leftPoint;
    private Vector2 _rightPoint;
    private Vector2 _cameraStartPoint;
    private float _cameraWidth;
    private float _cameraHeight;
    public Vector2 MinAnglePoint { get; private set; }
    public Vector2 MaxAnglePoint { get; private set; }

    public void Validate()
    {
        if (_shootMaxAngle < _shootMinAngle)
            _shootMaxAngle = _shootMinAngle;
        
        _cameraStartPoint = Camera.main.ScreenToWorldPoint(Vector2.zero);
        _cameraHeight = Camera.main.orthographicSize * 2;
        _cameraWidth = _cameraHeight * Camera.main.aspect;
        Point = new Vector2(_cameraStartPoint.x + (_cameraWidth * _viewportPositionX), _cameraStartPoint.y + (_cameraHeight * _viewportPositionY));
        float deltaX = -_cameraWidth * (float)Math.Sin(_lineAngle * Mathf.PI/180) * _length;  
        float deltaY = _cameraWidth * (float)Math.Cos(_lineAngle * Mathf.PI/180) * _length;  
        _leftPoint = new Vector2(Point.x - deltaX, Point.y - deltaY); 
        _rightPoint = new Vector2(Point.x + deltaX, Point.y + deltaY); 
        float deltaX1 = -_cameraWidth * (float)Math.Sin((_lineAngle + _shootMinAngle) * Mathf.PI/180) * _angleLineLength;  
        float deltaX2 = -_cameraWidth * (float)Math.Sin((_lineAngle + _shootMaxAngle) * Mathf.PI/180) * _angleLineLength;  
        float deltaY1 = _cameraWidth * (float)Math.Cos((_lineAngle + _shootMinAngle) * Mathf.PI/180)  * _angleLineLength;  
        float deltaY2 = _cameraWidth * (float)Math.Cos((_lineAngle + _shootMaxAngle) * Mathf.PI/180)  * _angleLineLength;  

        MinAnglePoint = new Vector2(Point.x + deltaX1, Point.y + deltaY1); 
        MaxAnglePoint = new Vector2(Point.x + deltaX2, Point.y + deltaY2); 
    }

    public void DrawGizmos()
    {
        Gizmos.color = _lineColor;
        ForGizmo(Point, _rightPoint - Point, _lineColor);
        ForGizmo(Point, _leftPoint - Point, _lineColor);
        Gizmos.DrawLine(Point,MinAnglePoint);
        Gizmos.DrawLine(Point,MaxAnglePoint);
    }
    
    public static void ForGizmo(Vector3 pos, Vector3 dir, Color color, float arrowheadLength = 0.25f, float arrowheadAngle = 20.0f)
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
