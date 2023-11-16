using System;
using System.Collections;
using Animancer;
using ThirdPersonMeleeSystem.ScriptableObjects;
using UnityEngine;

namespace ThirdPersonMeleeSystem.Managers
{
    public class ThirdPersonController : MonoBehaviour
    {
        #region Public Fields

        public event Action ForceWalkEvent;
        public event Action DisableCombatInputEvent;
        
        

        #endregion

        #region Private Fields
        
        private static readonly int YSpeedHash = Animator.StringToHash("YSpeed");

        private Vector3 _playerVelocity;

        private float _currentSpeed;
        private float _speed;
        private float _speedVelocityRef;

        private float _targetRotation;
        private float _rotationVelocity;

        #endregion

        #region Serialized Fields

        [Header("Refs")] 
        [SerializeField] private HybridAnimancerComponent animancerComponent;
        [SerializeField] private PlayerAnimationManager animationManager;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private InputController inputController;
        [SerializeField] private CharacterController characterController;

        [Header("Gravity")] [SerializeField] private float gravity = -9.81f;

        [Header("Ground Check")] 
        [SerializeField] private bool grounded;
        [SerializeField] private float sphereCastRadius;
        [SerializeField] private float sphereCastDistance;
        [SerializeField] private float yOffset;
        [SerializeField] private float slopeRayLength;
        [SerializeField] private LayerMask whatIsGround;

        [Header("Animation")] 
        [SerializeField] private LocomotionAsset locomotionAsset;

        [Header("Movement Speed")]
        [SerializeField] private float speedSmoothTime;
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float jogSpeed = 5f;
        [SerializeField] private float sprintSpeed = 7f;
        [SerializeField] private float airSpeed;

        [Header("Rotation")]
        [SerializeField] private float rotationSmoothTime;
        [SerializeField] private float lockOnRotationSpeed;

        [Header("Crouch")]
        [SerializeField] private float crouchHeight = 1.3f;
        [SerializeField] private Vector3 crouchCenter = new(0f ,0.7f, 0f);
        private const float _defaultHeight = 1.8f;
        private Vector3 _defaultCenter = new(0f , 1f, 0f);

        [Header("Jump")]
        [SerializeField] private int jumpCount = 1;
        [field:SerializeField] public float JumpVelocity { get; private set; }
        
        [Header("Slide")]
        [SerializeField] private float slideSpeed = 5f;
        [SerializeField] private float slideDuration = 1f;

        [Header("Vaulting")]
        [SerializeField] private Vector3 vaultRayOrigin;
        [SerializeField] private float rayLength;

        #endregion

        #region Getters
        
        public Camera MainCam { get; private set; }
        public float YSpeed { get; set; }
        public float WalkSpeed => walkSpeed;
        public float JogSpeed => jogSpeed;
        public float SprintSpeed => sprintSpeed;
        public float AirSpeed => airSpeed;
        public bool PlayerGrounded => grounded;
        public bool CanJump => grounded && jumpCount > 0;
        public float SlideSpeed => slideSpeed;
        public float SlideDuration => slideDuration;

        #endregion

        private void Start()
        {
            MainCam = Camera.main;
        }

        private void Update()
        {
            SimpleVault();
            GroundCheck();
            HandleGravity();
        }

        public void OnAnimatorMove()
        {
            RootMotionMovement();
        }

        public void MovePlayer()
        {
            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, _speed, ref _speedVelocityRef, speedSmoothTime);
            
            //bug: if we are running I do not want to use SmootherTransformMovementDirection.
            Vector3 moveDir = cameraController.LockedOnTarget && !InputController.SprintFlag ? inputController.GetSmoothedTransformMovementDirection() : inputController.GetSmoothedCameraMovementDirection();
            Vector3 customVelocity = AdjustVelocityToSlope(moveDir * (_currentSpeed * Time.deltaTime));
            Vector3 airVelocity = inputController.GetCameraRelativeMovementDirection() * (airSpeed * Time.deltaTime);

            _playerVelocity = grounded ? customVelocity : airVelocity;
            _playerVelocity.y += YSpeed * Time.deltaTime;
            characterController.Move(_playerVelocity);
        }

        private void RootMotionMovement()
        {
            if (animationManager.UseRootMotion)
            {
                Vector3 rootMotionVelocity = AdjustVelocityToSlope(animancerComponent.Animator.deltaPosition);
                Vector3 airVelocity = inputController.GetCameraRelativeMovementDirection() * (airSpeed * Time.deltaTime);

                _playerVelocity = grounded ? rootMotionVelocity : airVelocity;
                _playerVelocity.y += YSpeed * Time.deltaTime;
                characterController.Move(_playerVelocity);
            }
        }

        public void SetPlayerSpeed(float targetSpeed)
        {
            _speed = targetSpeed;
        }

        public void RotateToMovementDirection(bool canRotate)
        {
            if (!canRotate) return;
            if (inputController.GetRawMovementInput() == Vector2.zero) return;
            Vector2 input = inputController.GetRawMovementInput();
            _targetRotation = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + MainCam.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f,rotation,0f);
        }

        public void RotateToAttackDirection(Vector2 input, float cameraY)
        {
            if (input == Vector2.zero) return;
            _targetRotation = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + cameraY;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f,rotation,0f);
        }

        public void LookAtTarget(Vector3 target, bool canRotate)
        {
            if (!canRotate) return;
            Vector3 direction = (target - transform.position).normalized;
            direction.y = 0;
            
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, lockOnRotationSpeed * Time.deltaTime);
        }

        private void GroundCheck()
        {
            Ray groundRay = new Ray(transform.position + new Vector3(0f, yOffset, 0f), Vector3.down);
            grounded = Physics.SphereCast(groundRay, sphereCastRadius, sphereCastDistance, whatIsGround);
        }

        private void HandleGravity()
        {
            YSpeed = grounded && YSpeed < 0 ? -1.0f : YSpeed += gravity * Time.deltaTime;
            animancerComponent.Animator.SetFloat(YSpeedHash, YSpeed);
        }

        public LocomotionAsset GetLocomotionAsset()
        {
            return locomotionAsset;
        }

        public void Jump(float jumpForce)
        {
            jumpCount--;
            YSpeed = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
        
        
        public void Slide(Vector3 direction, float speed)
        {
            _playerVelocity = direction * (speed * Time.deltaTime);
            _playerVelocity.y += YSpeed * Time.deltaTime;
            characterController.Move(_playerVelocity);
            //now gravity should work
        }

        private void SimpleVault()
        {
            
        }
        
        private void DebugVaulting()
        {
            Vector3 origin = transform.position + vaultRayOrigin;
            Ray firstRay = new Ray(origin, transform.forward);
            Debug.DrawRay(origin, transform.forward * rayLength, Color.green);

            if (!Physics.Raycast(firstRay, out RaycastHit firstHit, rayLength)) return;
        
            Vector3 inverseTransformPoint = transform.InverseTransformPoint(firstHit.point);
            Ray secondRay =
                new Ray(
                    transform.position + new Vector3(0f, characterController.height / 2, inverseTransformPoint.z),
                    transform.forward);
            
            Debug.DrawRay(
                transform.position + new Vector3(0f, characterController.height/2, inverseTransformPoint.z),
                transform.forward * (characterController.radius * 2),
                Color.green);

            if (Physics.Raycast(secondRay, characterController.radius * 2)) return;
            
            Ray thirdRay =
                new Ray(
                    transform.position + new Vector3(0f, characterController.height / 2, inverseTransformPoint.z) 
                                       + transform.forward * (characterController.radius * 2), Vector3.down);
            Debug.DrawRay(
                transform.position + new Vector3(0f, characterController.height/2, inverseTransformPoint.z) 
                                   + transform.forward * (characterController.radius * 2),
                Vector3.down * characterController.height,
                Color.green);

            if (Physics.Raycast(thirdRay, out RaycastHit thirdRayHit, characterController.height))
            {
                Debug.Log(thirdRayHit.point);
            }
            else
            {
                //Generate a point 
            }
        }
        
        public void CrouchEnter()
        {
            characterController.height = crouchHeight;
            characterController.center = crouchCenter;
        }
        
        public void CrouchExit()
        {
            characterController.height = _defaultHeight;
            characterController.center = _defaultCenter;
        }

        public void ResetJumpCount()
        {
            jumpCount = 1;
        }

        private Vector3 AdjustVelocityToSlope(Vector3 velocity)
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, slopeRayLength, whatIsGround))
            {
                Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
                Vector3 adjustedVelocity = slopeRotation * velocity;

                if (adjustedVelocity.y < 0)
                {
                    return adjustedVelocity;
                }
            }
            return velocity;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = grounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(transform.position + new Vector3(0f, yOffset, 0f), sphereCastRadius);
        }
    }
}
