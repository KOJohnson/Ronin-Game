using System;
using Cinemachine;
using Sirenix.OdinInspector;
using ThirdPersonMeleeSystem.Core;
using UnityEngine;

[Serializable]
public class CombatCameraTransition
{
    public CinemachineVirtualCamera combatCamera;
    [MinMaxSlider(0, 10, showFields: true)] public Vector2Int enemyCountMinMax = new(0, 10);
}

public class DynamicCombatCamera : MonoBehaviour
{
    private const int MAX_COLLIDERS = 20;
    private Collider[] _colliders = new Collider[MAX_COLLIDERS];
    [SerializeField] private CinemachineBrain cineMachineBrain;
    [SerializeField] private CombatCameraTransition[] cameraTransitions;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask enemyLayer;
    private void Start()
    {
        EnemyCombatManager.Instance.EnemyCountChanged += CameraTransTest;
    }

    private void CameraTransTest()
    {
        foreach (CombatCameraTransition transition in cameraTransitions)
        {
            if (EnemyCombatManager.Instance.GetEnemyCombatCount().IsInRange(transition.enemyCountMinMax))
            {
                ChangeVirtualCamera(transition.combatCamera);
            }
        }
    }

    private void ChangeVirtualCamera(CinemachineVirtualCamera newVirtualCam)
    {
        CinemachineVirtualCamera oldCam = (CinemachineVirtualCamera)cineMachineBrain.ActiveVirtualCamera;
        if (oldCam == newVirtualCam) return;
        newVirtualCam.VirtualCameraGameObject.SetActive(true);
        oldCam.VirtualCameraGameObject.SetActive(false);
    }
}
