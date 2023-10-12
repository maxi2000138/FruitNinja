using UnityEngine;

namespace App.Scripts.DebugAndGizmosExtensions
{
    public class DebugAndGizmosDrawer : Debug
    {
        public static void DrawCircleDebug(Vector2 position, float radius, int segments, Color color)
        {
            if (radius <= 0.0f || segments <= 0)
                return;

            float angleStep = (360.0f / segments);
            angleStep *= Mathf.Deg2Rad;

            Vector2 lineStart = Vector2.zero;
            Vector2 lineEnd = Vector2.zero;

            for (int i = 0; i < segments; i++)
            {
                lineStart.x = Mathf.Cos(angleStep * i);
                lineStart.y = Mathf.Sin(angleStep * i);

                lineEnd.x = Mathf.Cos(angleStep * (i + 1));
                lineEnd.y = Mathf.Sin(angleStep * (i + 1));

                lineStart *= radius;
                lineEnd *= radius;

                lineStart += position;
                lineEnd += position;

                DrawLine(lineStart, lineEnd, color);
            }   
        }
    
        public static void DrawArrowGizmos(Vector3 pos, Vector3 dir, Color color, float arrowheadLength = 0.25f, float arrowheadAngle = 20.0f)
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
}
