using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ThirdPersonMeleeSystem.Managers
{
    public class CameraController : MonoBehaviour
    {
        #region Public Fields

        public static CameraController Instance;
    
        #endregion
    
        #region Private Fields
    
        private float _targetPitch;
        private float _targetYaw;
        private List<LockOnTarget> _availableTargets = new();

        private LockOnTarget _nearestLockOnTarget;
        private LockOnTarget _nearestSoftLockOnTarget;
        private LockOnTarget _leftLockOnTarget;
        private LockOnTarget _rightLockOnTarget;

        #endregion
    
        #region Serialized Fields

        [Header("Refs")]
        [SerializeField] private InputController inputController;
        [SerializeField] private Transform cameraHolder;

        [Header("Free Look Camera")] 
        [SerializeField] private float mouseSensitivity = 5f;
        [SerializeField] private float minClamp;
        [SerializeField] private float maxClamp;

        [Header("Soft Lock On Camera")] 
        private const int MAX_COLLIDERS = 10;
        private Collider[] _softLockTargets = new Collider[MAX_COLLIDERS];

        [Header("Hard Lock On Camera")] 
        [SerializeField] private float lookAtSpeed = 0.3f;
        [SerializeField] private float maximumLockOnDistance;
        [SerializeField] private float minInViewDot;
        [SerializeField] private bool autoLockOnDefeat;
        public bool IsHardLockTarget => LockedOnTarget && HardLockOnTarget != null;
    
        #endregion
    
        #region Getters

        public bool cursorVisible;
        public bool LockedOnTarget { get; private set; }
        public LockOnTarget SoftLockOnTarget { get; private set; }
        public LockOnTarget HardLockOnTarget { get; private set; }
        public LockOnTarget CurrentLockOnTarget
        {
            get
            {
                if (HardLockOnTarget != null)
                {
                    return HardLockOnTarget;
                }
                else
                {
                    if (SoftLockOnTarget != null)
                    {
                        return SoftLockOnTarget;
                    } 
                }
                return null;
            }
        }

        #endregion

        private void Start()
        {
            Instance = this; // change this at some point
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = cursorVisible;
        }

        private void Update()
        {
            FindSoftLockTargets();
            HandleHardTargetLockOn();
            TrackTargetDistance();
        }

        private void LateUpdate()
        {
            if (IsHardLockTarget)
            {
                CameraLookAt(CurrentLockOnTarget.LockOnPoint());
            }
            else
            {
                FreeLookCamera();
            }
        }

        private void FreeLookCamera()
        {
            _targetPitch += inputController.GetLookInput().y * (mouseSensitivity * Time.deltaTime);
            _targetYaw += inputController.GetLookInput().x * (mouseSensitivity * Time.deltaTime);
        
            _targetPitch = Mathf.Clamp(_targetPitch, minClamp, maxClamp);
            _targetYaw = Mathf.Clamp(_targetYaw, Single.MinValue, Single.MaxValue);
        
            cameraHolder.rotation = Quaternion.Euler(_targetPitch, _targetYaw, 0f);
        }

        private void CameraLookAt(Vector3 lookAt)
        {
            Vector3 direction = (lookAt - cameraHolder.position).normalized;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);
            cameraHolder.rotation = rotation;
            
            float eulerAnglesX = cameraHolder.eulerAngles.x;
            
            //keep camera rotation when exiting lock on state
            if (eulerAnglesX > maxClamp)
            {
                eulerAnglesX -= 360;
            }
            
            _targetPitch = eulerAnglesX;
            _targetYaw = cameraHolder.eulerAngles.y;
        }

        private void FindSoftLockTargets()
        {
            if (CurrentLockOnTarget) return;
            
            int numOfColliders = Physics.OverlapSphereNonAlloc(transform.position, maximumLockOnDistance, _softLockTargets);
            
            FilterLockOnTargets(_softLockTargets, numOfColliders);
            
            float shortestDistance = Mathf.Infinity;

            for (int i = 0; i < _availableTargets.Count; i++)
            {
                float distanceFromTarget = Vector3.Distance(transform.position, _availableTargets[i].transform.position);

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    _nearestSoftLockOnTarget = _availableTargets[i];
                }
            }

            if (_nearestSoftLockOnTarget != null)
            {
                SoftLockOnTarget = _nearestSoftLockOnTarget;
                LockedOnTarget = true;
            }
        }
    
        private void HandleHardTargetLockOn()
        {
            if (InputController.LockOnFlag && !HardLockOnTarget)
            {
                LockOn();

                if (_nearestLockOnTarget != null)
                {
                    HardLockOnTarget = _nearestLockOnTarget;
                    LockedOnTarget = true;
                }
            }
            else if (InputController.LockOnFlag && HardLockOnTarget && LockedOnTarget)
            {
                ClearLockOn();
            }

            if (HardLockOnTarget && LockedOnTarget && InputController.TargetSwitchLeftFlag)
            {
                LockOn();

                if (_leftLockOnTarget != null)
                {
                    HardLockOnTarget = _leftLockOnTarget;
                }
            }

            if (HardLockOnTarget && LockedOnTarget && InputController.TargetSwitchRightFlag)
            {
                LockOn();

                if (_rightLockOnTarget != null)
                {
                    HardLockOnTarget = _rightLockOnTarget;
                }
            }
        }
    
        private void LockOn()
        {
            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfTargetLeft = -Mathf.Infinity;
            float shortestDistanceOfTargetRight = Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(transform.position, maximumLockOnDistance);
            
            //filter targets 
            FilterLockOnTargets(colliders, colliders.Length);
            
            
            for (int k = 0; k < _availableTargets.Count; k++)
            {
                float distanceFromTarget = Vector3.Distance(transform.position, _availableTargets[k].transform.position);

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    _nearestLockOnTarget = _availableTargets[k];
                }

                if (HardLockOnTarget && LockedOnTarget)
                {
                    Vector3 relativeEnemyPosition = transform.InverseTransformPoint(_availableTargets[k].transform.position);
                    float distanceFromLeftTarget = relativeEnemyPosition.x;
                    float distanceFromRightTarget = relativeEnemyPosition.x;
                
                    if (relativeEnemyPosition.x <= 0 && distanceFromLeftTarget > shortestDistanceOfTargetLeft && _availableTargets[k] != CurrentLockOnTarget)
                    {
                        shortestDistanceOfTargetLeft = distanceFromLeftTarget;
                        _leftLockOnTarget = _availableTargets[k];
                    }
                    else if (relativeEnemyPosition.x >= 0 && distanceFromRightTarget < shortestDistanceOfTargetRight && _availableTargets[k] != CurrentLockOnTarget)
                    {
                        shortestDistanceOfTargetRight = distanceFromRightTarget;
                        _rightLockOnTarget = _availableTargets[k];
                    }
                }
            }
        }

        private void FilterLockOnTargets(Collider[] colliders, int length)
        {
            for (int i = 0; i < length; i++)
            {
                LockOnTarget target = colliders[i].GetComponent<LockOnTarget>();

                if (target == null) continue;

                float distanceFromTarget = Vector3.Distance(transform.position, target.transform.position);

                if (!(distanceFromTarget < maximumLockOnDistance) || _availableTargets.Contains(target)) continue;

                _availableTargets.Add(target);
            }
        }

        public void ChangeSoftLockTarget(LockOnTarget newTarget)
        {
            SoftLockOnTarget = newTarget;
        }

        private void HandleLockOnDefeat()
        {
            if (autoLockOnDefeat)
            {
                ClearLockOn();
                LockOn();
            }
            else
            {
                ClearLockOn();
            }
        }
        
        private void TrackTargetDistance()
        {
            if (CurrentLockOnTarget == null) return;
            if (Vector3.Distance(transform.position, CurrentLockOnTarget.transform.position) > maximumLockOnDistance || !CurrentLockOnTarget.enabled)
            {
                ClearLockOn();
            }
        }
    
        public void ClearLockOn()
        {
            // if (!LockedOnTarget)return;
            LockedOnTarget = false;
            _availableTargets.Clear();
            HardLockOnTarget = null;
            SoftLockOnTarget = null;
            _nearestLockOnTarget = null;
            _nearestSoftLockOnTarget = null;
            _leftLockOnTarget = null;
            _rightLockOnTarget = null;
        }
    }
}