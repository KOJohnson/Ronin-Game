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
    }
}