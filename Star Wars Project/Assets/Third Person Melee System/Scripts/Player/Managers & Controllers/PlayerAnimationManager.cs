using UnityEngine;

namespace ThirdPersonMeleeSystem.Managers
{
    public class PlayerAnimationManager : AnimationManager
    {
        #region Public Fields

        public static readonly float ToDirectionalBlendTree = 1f;
        public static readonly float ToLinearBlendTree = 0f;
        
        public readonly int VelocityID = Animator.StringToHash("Velocity");
        public readonly int VelocityXID = Animator.StringToHash("VelocityX");
        public readonly int VelocityZID = Animator.StringToHash("VelocityZ");
        public readonly int IsSprintingID = Animator.StringToHash("IsSprinting");
        public readonly int IsMovingID = Animator.StringToHash("IsMoving");
        public readonly int IsWalkingID = Animator.StringToHash("IsWalking");
        public readonly int IsLockedOnID = Animator.StringToHash("IsLockedOn");
        public readonly int IsWeaponDrawnID = Animator.StringToHash("IsWeaponDrawn");
        public readonly int IsCrouchingID = Animator.StringToHash("IsCrouching");
        public readonly int WeaponEquipTriggerID = Animator.StringToHash("EquipWeapon");
        public readonly int JumpTriggerID = Animator.StringToHash("Jump");
        public readonly int LinearToDirectionalID = Animator.StringToHash("LinearToDirectional");
        public readonly int StandingToCrouchingID = Animator.StringToHash("StandingToCrouch");
        public readonly int JogToWalkID = Animator.StringToHash("JogToWalk");
        public readonly int IsBlockingID = Animator.StringToHash("IsBlocking");
        
        #endregion
    
        #region Private Fields

        private float velocity;
        private float VelocityX;
        private float VelocityZ;
        
        private float _velocityRef;
        private float _velocityXRef;
        private float _velocityYRef;
        
        #endregion
    
        #region Serialized Fields

        [SerializeField] private InputController _inputController;
        [SerializeField] private float animationSmoothTime = 0.15f;
        [SerializeField] private float directionalSmoothTime = 0.15f;
        [SerializeField] private float dampTime;

        #endregion
    
        #region Getters
    
        public static PlayerAnimationManager Instance { get; private set; }

        #endregion

        protected override void Start()
        {
            base.Start();
            SetupSingleton();
        }

        private void SetupSingleton()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Update()
        {
            SetStandingToCrouchParameter(AnimancerComponent.Animator.GetBool(IsCrouchingID) ? 1f : 0f);
            AnimancerComponent.SetFloat(VelocityID, velocity);
            AnimancerComponent.Animator.SetFloat(VelocityXID, VelocityX);
            AnimancerComponent.Animator.SetFloat(VelocityZID, VelocityZ);
            AnimancerComponent.Animator.SetBool(IsWeaponDrawnID, WeaponManager.Instance.IsWeaponDrawn);
            AnimancerComponent.Animator.SetBool(IsLockedOnID, CameraController.Instance.LockedOnTarget);
            AnimancerComponent.Animator.SetBool(IsWalkingID, InputController.WalkToggle);
            AnimancerComponent.Animator.SetBool(IsSprintingID, InputController.SprintFlag);
            AnimancerComponent.Animator.SetBool(IsMovingID, InputController.IsMoving);
        }

        public void PlayController()
        {
            AnimancerComponent.PlayController();
        }

        public void SetAnimatorDirectionalVelocity(float target)
        {
            VelocityX = Mathf.SmoothDamp(VelocityX, target * _inputController.GetRawMovementInput().normalized.x, ref _velocityXRef, directionalSmoothTime);
            VelocityZ = Mathf.SmoothDamp(VelocityZ, target * _inputController.GetRawMovementInput().normalized.y, ref _velocityYRef, directionalSmoothTime);
        }

        public void SetAnimatorLinearVelocity(float targetBlend)
        {
            velocity = Mathf.SmoothDamp(velocity, targetBlend * _inputController.GetRawMovementInput().magnitude, ref _velocityRef, animationSmoothTime);
        }

        public void SetJogToWalkParameter(float target)
        {
            AnimancerComponent.Animator.SetFloat(JogToWalkID, target, dampTime, Time.deltaTime);
        }
        
        public void SetLinearToDirectionalParameter(float target)
        {
            AnimancerComponent.Animator.SetFloat(LinearToDirectionalID, target, dampTime, Time.deltaTime);
        }
        
        public void SetStandingToCrouchParameter(float target)
        {
            AnimancerComponent.Animator.SetFloat(StandingToCrouchingID, target, dampTime, Time.deltaTime);
        }

        public void SetIsCrouching(bool state)
        {
            AnimancerComponent.SetBool(IsCrouchingID, state);
        }

        public void SetIsBlocking(bool state)
        {
            AnimancerComponent.SetBool(IsBlockingID, state);
        }

        public void ResetAnimatorVelocityParameters()
        {
            velocity = 0f;
            VelocityX = 0f;
            VelocityZ = 0f;
        }
    }
}
