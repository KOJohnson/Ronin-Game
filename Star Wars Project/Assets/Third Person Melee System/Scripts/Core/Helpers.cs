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

        public static bool IsInRange(this int value, Vector2 minMax)
        {
            return value >= minMax.x && value < minMax.y;
        }
    }
}