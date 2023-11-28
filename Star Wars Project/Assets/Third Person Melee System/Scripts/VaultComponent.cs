using System;
using ThirdPersonMeleeSystem.Managers;
using UnityEngine;

public class VaultComponent : MonoBehaviour
{
    #region Public Fields
    #endregion
    
    #region Private Fields

    private Vector3 _endPosition;
    private Vector3 _startPosition;

    private float _timeElapsed;
    
    #endregion
    
    #region Serialized Fields

    [Header("Vaulting")]
    [SerializeField] private float rayLength;
    [SerializeField] private float climbDuration;

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
            Vault();
        }
    }

    private void Vault()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;
        const float verticalRayLength = 5f;
        
        origin.y += 0.85f;
        Debug.DrawRay(origin, direction * rayLength, Color.white, 3f);
        if (Physics.Raycast(origin, direction, out _hit, rayLength))
        {
            Vector3 origin2 = origin;
            origin2.y += 0.75f;
            Debug.DrawRay(origin2, direction * rayLength, Color.white, 3f);
            if (Physics.Raycast(origin2, direction, out _hit, rayLength))
            {
                Vector3 origin4 = origin2 + (direction * rayLength);
                origin4.y += 2f;
                Debug.DrawRay(origin4, Vector3.down * verticalRayLength, Color.white, 3f);
                if (Physics.Raycast(origin4, Vector3.down, out _hit, verticalRayLength))
                {
                    SetClimbing();
                }
            }
            else
            {
                Vector3 origin3 = origin2 + (direction * rayLength);
                Debug.DrawRay(origin3, Vector3.down * verticalRayLength, Color.white, 3f);
                if (Physics.Raycast(origin3, Vector3.down, out _hit, verticalRayLength))
                {
                    SetClimbing();
                }
            }
        }
    }

    private void SetClimbing()
    {
        TriggerClimbAction = true;
        _startPosition = transform.position;
        _endPosition = _hit.point;
    }

    public void MoveToEndPoint()
    {
        if (_timeElapsed < climbDuration)
        {
            transform.position = Vector3.Slerp(_startPosition, _endPosition, _timeElapsed / climbDuration);
            _timeElapsed += Time.deltaTime;
        }
        else
        {
            transform.position = _endPosition;
        }
    }

    public void ResetClimbing()
    {
        TriggerClimbAction = false;
        _startPosition = Vector3.zero;
        _endPosition = Vector3.zero;
        _timeElapsed = 0f;
    }

    public bool HasReachedDestination()
    {
        return transform.position == _endPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_hit.point, 0.1f);
    }
}
