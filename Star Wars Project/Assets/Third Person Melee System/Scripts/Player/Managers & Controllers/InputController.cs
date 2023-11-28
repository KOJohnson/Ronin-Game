using UnityEngine;

namespace ThirdPersonMeleeSystem.Managers
{
    public class InputController : MonoBehaviour
    {
        #region Public Fields

        public static bool WalkToggle;
        public static bool SprintFlag;
        public static bool IsMoving;
        public static bool DrawWeaponFlag;
        public static bool BlockFlag;
        public static bool DodgeFlag;
        public static bool LockOnFlag;
        public static bool TargetSwitchLeftFlag;
        public static bool TargetSwitchRightFlag;
        public static bool LightAttackFlag { get; private set; }
        public static bool HeavyAttackFlag { get; private set; }
        public static bool FinisherFlag;
        public static bool JumpFlag;
        public static bool CrouchFlag;
        public static bool SlideFlag;
        public static bool ClimbFlag;

        #endregion

        #region Private Fields

        private Vector2 _rawInput;
        private Vector2 _currentInput;
        private Vector2 _currentInputRef;

        private Vector3 _currentMovementDirection;
        private Vector3 _currentMovementDirRef;

        
        #endregion

        #region Serialized Fields

        [SerializeField] private Camera mainCamera;
        [SerializeField] private float smoothInputSpeed = 0.15f;

        #endregion

        #region Getters

        public Inputs PlayerInput { get; private set; }

        #endregion

        private void Awake()
        {
            PlayerInput = new Inputs();
            PlayerInput.Gameplay.Move.performed += _ => IsMoving = true;
            PlayerInput.Gameplay.Move.canceled += _ => IsMoving = false;
            PlayerInput.Gameplay.ToggleWalk.performed += _ => WalkToggle = !WalkToggle;
            PlayerInput.Gameplay.Sprint.performed += _ => SprintFlag = true;
            PlayerInput.Gameplay.Sprint.canceled += _ => SprintFlag = false;
            PlayerInput.Gameplay.ToggleCrouch.performed += _ => CrouchFlag = !CrouchFlag;
        }

        private void OnEnable()
        {
            PlayerInput.Enable();
        }

        private void OnDisable()
        {
            PlayerInput.Disable();
        }

        private void Update()
        {
            ClimbFlag = PlayerInput.Gameplay.Climb.WasPressedThisFrame();
            BlockFlag = PlayerInput.Combat.Block.IsPressed();
            SlideFlag = PlayerInput.Gameplay.Slide.WasPerformedThisFrame();
            JumpFlag = PlayerInput.Gameplay.Jump.WasPerformedThisFrame();
            FinisherFlag = PlayerInput.Combat.Finisher.WasPerformedThisFrame();
            DodgeFlag = PlayerInput.Combat.Dodge.WasPerformedThisFrame();
            DrawWeaponFlag = PlayerInput.Combat.DrawWeapon.WasPerformedThisFrame();
            LockOnFlag = PlayerInput.Combat.LockOn.WasPerformedThisFrame();
            TargetSwitchLeftFlag = PlayerInput.Combat.TargetSwitchLeft.WasPerformedThisFrame();
            TargetSwitchRightFlag = PlayerInput.Combat.TargetSwitchRIght.WasPerformedThisFrame();
            HandleAttackInput();
        }

        private void HandleAttackInput()
        {
            if (PlayerInput.Combat.AttackModifier.IsPressed() && !IsMoving)
            {
                HeavyAttackFlag = PlayerInput.Combat.HeavyAttack.WasPerformedThisFrame();
            }
            else
            {
                LightAttackFlag = PlayerInput.Combat.LightAttack.WasPerformedThisFrame();
            }
        }

        public Vector2 GetRawMovementInput()
        {
            return PlayerInput.Gameplay.Move.ReadValue<Vector2>();
        }

        public Vector3 GetCameraRelativeMovementDirection()
        {
            Vector3 forward = mainCamera.transform.forward;
            forward.y = 0;
            forward = forward.normalized;
            return GetRawMovementInput().x * mainCamera.transform.right + GetRawMovementInput().y * forward;
        }
        
        public Vector3 GetCameraTransformMovementDirection()
        {
            return GetRawMovementInput().x * transform.right + GetRawMovementInput().y * transform.forward;
        }

        public Vector3 GetSmoothedCameraMovementDirection()
        {
            return _currentMovementDirection = Vector3.SmoothDamp(_currentMovementDirection, GetCameraRelativeMovementDirection(), ref _currentMovementDirRef, smoothInputSpeed);
        }
        
        public Vector3 GetSmoothedTransformMovementDirection()
        {
            return _currentMovementDirection = Vector3.SmoothDamp(_currentMovementDirection, GetCameraTransformMovementDirection(), ref _currentMovementDirRef, smoothInputSpeed);
        }

        public Vector2 GetLookInput()
        {
            return PlayerInput.Gameplay.Look.ReadValue<Vector2>();
        }
    }
}
