using UnityEngine;

namespace ThirdPersonMeleeSystem
{
    [RequireComponent(typeof(Collider))]
    public class LockOnTarget : MonoBehaviour
    {
        [SerializeField] private Vector3 lockOnOffset;
        [SerializeField] private Vector3 lockOnUIOffset;

        public Vector3 LockOnPoint()
        {
            return transform.position + lockOnOffset;
        }

        public Vector3 LockOnUIPosition()
        {
            return transform.position + lockOnUIOffset;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(LockOnPoint(), 0.1f);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(LockOnUIPosition(), 0.1f);
        }
    }
}
