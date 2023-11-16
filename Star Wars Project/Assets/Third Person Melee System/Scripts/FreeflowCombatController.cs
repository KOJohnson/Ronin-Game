using System.Collections.Generic;
using ThirdPersonMeleeSystem;
using ThirdPersonMeleeSystem.Interfaces;
using ThirdPersonMeleeSystem.Managers;
using UnityEngine;

public class FreeflowCombatController : MonoBehaviour
{
    #region Public Fields
    #endregion
    
    #region Private Fields
    #endregion
    
    #region Serialized Fields

    [Header("Refs")] 
    [SerializeField] private InputController inputController;
    
    [Header("Settings")]
    [SerializeField] private List<LockOnTarget> potentialTargets;
    [SerializeField] private float freeFlowRange;
    [SerializeField] private float targetSelectionRange;
    
    #endregion
    
    #region Getters

    public LockOnTarget FreeFlowTarget;
    
    #endregion

    private void Start()
    {
        
    }

    private void Update()
    {
        if (!FreeFlowTarget) return;
        if (Vector3.Distance(transform.position, FreeFlowTarget.transform.position) > freeFlowRange || !FreeFlowTarget.enabled)
        {
            ClearFreeFlowTarget();
        }
    }

    public void GetPotentialTargets()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, freeFlowRange);

        for (int i = 0; i < colliders.Length; i++)
        {
            LockOnTarget target = colliders[i].GetComponent<LockOnTarget>();
            if (target == null) continue;
            potentialTargets.Add(target);
        }
        
        FindClosestTarget();
    }

    private void FindClosestTarget()
    {
        float shortestDistance = Mathf.Infinity;
        Vector3 position = inputController.GetCameraRelativeMovementDirection() == Vector3.zero ? transform.position : transform.position + inputController.GetCameraRelativeMovementDirection() * targetSelectionRange;
        
        for (int i = 0; i < potentialTargets.Count; i++)
        {
            float distance = Vector3.Distance(potentialTargets[i].transform.position, position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                FreeFlowTarget = potentialTargets[i];
            }
        }
        
        potentialTargets.Clear();
    }

    private void ClearFreeFlowTarget()
    {
        FreeFlowTarget = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, freeFlowRange);

        if (Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position + inputController.GetCameraRelativeMovementDirection() * targetSelectionRange, 0.1f); 
        }

    }
}
