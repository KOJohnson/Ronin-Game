using UnityEngine;

namespace ThirdPersonMeleeSystem.Core
{
    public static class Helpers
    {
        public static Vector3 TargetOffset(Vector3 currentPosition, Vector3 targetPosition, float offset)
        {
            targetPosition.y = currentPosition.y;
            return Vector3.MoveTowards(targetPosition, currentPosition, offset);
        }

        public static bool IsInRangeOf(this int value, Vector2 minMax)
        {
            return value >= minMax.x && value <= minMax.y;
        }
        
        public static Vector3 MoveQuadraticCurve(this Vector3 value, float t, Vector3 startPoint, Vector3 endPoint, Vector3 tangent)
        {
            return BezierCurve.QuadraticBezierCurve(t, startPoint, endPoint, tangent);
        }
    }
}