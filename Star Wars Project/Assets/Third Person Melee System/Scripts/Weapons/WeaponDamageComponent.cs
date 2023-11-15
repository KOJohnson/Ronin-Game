using System.Collections.Generic;
using Animancer;
using Cinemachine;
using ThirdPersonMeleeSystem.Interfaces;
using UnityEngine;

namespace ThirdPersonMeleeSystem.Weapons
{
    [RequireComponent(typeof(Collider))]
    public class WeaponDamageComponent : MonoBehaviour
    {
        #region Private

        private HitStop _hitStop;
        private Ray _hitDetectionRay;
        private Collider _weaponCollider;
        private readonly List<Collider> hitObjects = new();
        private bool _useColliderDetection;

        #endregion
        
        #region Serialized Fields
        
        [Header("Settings")]
        [SerializeField] private MeleeWeapon weapon;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float hitStopDuration;
        [SerializeField] private CinemachineImpulseSource impulseSource;
        [SerializeField] private float impulseForce;

        [Header("Hit Detection")]
        [SerializeField] private Transform[] raycastPoints;
        [SerializeField] private bool useRaycastDetection;

        #endregion

        private void Awake()
        {
            _hitStop = new HitStop(transform.root.GetComponent<HybridAnimancerComponent>());
            InitWeaponCollider();
        }

        private void OnValidate()
        {
            _useColliderDetection = !useRaycastDetection;
        }

        private void OnEnable()
        {
            weapon.PlayAttackSound();
        }

        private void OnDisable()
        {
            hitObjects.Clear();
        }

        private void Update()
        {
            RaycastHitDetection();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!enabled) return;
            if (!_useColliderDetection) return;
            if (other.CompareTag("Player")) return;
            OnHit(other);
        }
        
        private void InitWeaponCollider()
        {
            _weaponCollider = GetComponent<Collider>();
            _weaponCollider.enabled = _useColliderDetection;
            _weaponCollider.isTrigger = true;
        }

        private void RaycastHitDetection()
        {
            if (!useRaycastDetection) return;
            
            for (int i = 0; i < raycastPoints.Length; i++)
            {
                if (i + 1 < raycastPoints.Length)
                {
                    Vector3 direction = raycastPoints[i + 1].position - raycastPoints[i].position;
                    float rayLength = direction.magnitude;
                    direction.Normalize();

                    _hitDetectionRay.origin = raycastPoints[i].position;
                    _hitDetectionRay.direction = direction;

                    if (Physics.Raycast(_hitDetectionRay, out RaycastHit hitInfo, rayLength, layerMask))
                    {
                        Debug.DrawRay(raycastPoints[i].position, _hitDetectionRay.direction, Color.green, 3f);
                        if (hitObjects.Contains(hitInfo.collider)) return;
                        hitObjects.Add(hitInfo.collider);
                        OnHit(hitInfo.collider);
                    }
                    else
                    {
                        Debug.DrawRay(raycastPoints[i].position, _hitDetectionRay.direction, Color.red, 3f);
                    }
                }
            }
        }
        
        private void OnHit(Collider other)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            damageable?.TakeDamage(transform.root.position, weapon.GetWeaponDamage(), weapon.GetPostureDamage(), weapon.GetAttackType());
            HitStop(hitStopDuration);
            //impulseSource.GenerateImpulse(impulseForce);
        }
        
        private void HitStop(float duration)
        {
            if (_hitStop.IsWaiting)return;
            StartCoroutine(_hitStop.FreezeAnimator(duration));
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < raycastPoints.Length; i++)
            {
                Gizmos.DrawSphere(raycastPoints[i].position, 0.05f);
                
                if (i + 1 < raycastPoints.Length)
                {
                    Gizmos.DrawLine(raycastPoints[i].position, raycastPoints[i + 1].position);
                }
            }
        }
    }
}