using System;
using ThirdPersonMeleeSystem.Managers;
using UnityEngine;

public class Vaulting : MonoBehaviour
{
    #region Public Fields
    #endregion
    
    #region Private Fields
    #endregion
    
    #region Serialized Fields

    [Header("Vaulting")]
    [SerializeField] private float rayLength;

    private RaycastHit _hit;

    #endregion
    
    #region Getters
    
    public bool TriggerClimbAction { get; private set; }
    
    #endregion

    private void Start()
    {
        
    }

    private void Update()
    {
        if (InputController.ClimbFlag)
        {
            VaultingTest();
        }
    }

    private void VaultingTest()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;
        const float verticalRayLength = 1.8f;
        
        origin.y += 0.85f;
        Debug.DrawRay(origin, direction * rayLength, Color.white, 3f);
        if (Physics.Raycast(origin, direction, out _hit, rayLength))
        {
            Vector3 origin2 = origin;
            origin2.y += 0.8f;
            Debug.DrawRay(origin2, direction * rayLength, Color.white, 3f);
            if (Physics.Raycast(origin2, direction, out _hit, rayLength))
            {
                Vector3 origin4 = origin2 + (direction * rayLength);
                origin4.y += 1f;
                Debug.DrawRay(origin4, Vector3.down * verticalRayLength, Color.white, 3f);
                if (Physics.Raycast(origin4, Vector3.down, out _hit, verticalRayLength))
                {
                    //move here
                    transform.position = _hit.point;
                }
            }
            else
            {
                Vector3 origin3 = origin2 + (direction * rayLength);
                Debug.DrawRay(origin3, Vector3.down * verticalRayLength, Color.white, 3f);
                if (Physics.Raycast(origin3, Vector3.down, out _hit, verticalRayLength))
                {
                    //move here
                    transform.position = _hit.point;
                }
            }
        }
    }

    public void Climb(Vector3 startPosition, Vector3 endPosition)
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_hit.point, 0.1f);
        
        // Vector3 origin = transform.position;
        // Vector3 direction = transform.forward;
        // origin.y += 0.85f;
        // Gizmos.DrawRay(origin, direction * rayLength);
        //
        // Vector3 origin2 = origin;
        // origin2.y += 0.8f;
        // Gizmos.DrawRay(origin2, direction * rayLength);
        //
        // Vector3 origin3 = origin2 + (direction * rayLength);
        // Gizmos.DrawRay(origin3, Vector3.down * 1.5f);
    }
}
