using ThirdPersonMeleeSystem.Managers;

namespace ThirdPersonMeleeSystem.StateMachine
{
    public class BaseLocomotion : BaseState
    {
        public BaseLocomotion(StateMachineController stateMachineController, StateMachine stateMachine) : base(stateMachineController, stateMachine)
        {
        }

        private bool _canDrawWeapon => !WeaponManager.Instance.IsWeaponDrawn && 
                                       InputController.DrawWeaponFlag && 
                                       !PlayerAnimationManager.Instance.IsInteracting &&
                                       WeaponManager.Instance.IsValidWeapon();

        private bool _canSheatheWeapon => InputController.DrawWeaponFlag &&
                                          !PlayerAnimationManager.Instance.IsInteracting;
        
        protected const float toLinearBlendTree = 0f;
        protected const float toDirectionalBlendTree = 1f;



        public override void EnterState()
        {
        }

        public override void Tick(float delta)
        {
            _stateMachineController.AnimationManager.PlayController();
            HandleMovement();
            HandleRotation();
            HandleWeaponEquip();
        }

        protected override void ExitState()
        {
        }

        public override void CheckSwitchState()
        {
            if (_stateMachineController.ThirdPersonController.CanJump && InputController.JumpFlag)
            {
                ChangeState(_stateMachine.JumpingState());
            }

            if (InputController.DodgeFlag && !PlayerAnimationManager.Instance.IsInteracting)
            {
                ChangeState(_stateMachine.DodgeState());
            }
            
            HandleAttackStates();

            if (_stateMachineController.WeaponManager.IsWeaponDrawn)
            {
                HandleFinisherState();
            }
        }
        
        private void HandleMovement()
        {
            if (!PlayerAnimationManager.Instance.IsInteracting)
            {
                _stateMachineController.ThirdPersonController.MovePlayer();
            }
        }

        private void HandleRotation()
        {
            if (_stateMachine.currentState is RunningState)
            {
                _stateMachineController.AnimationManager.SetLinearToDirectionalParameter(toLinearBlendTree);
                _stateMachineController.ThirdPersonController.RotateToMovementDirection(!PlayerAnimationManager.Instance.IsInteracting);
            }
            else
            {
                if (_stateMachineController.CameraController.LockedOnTarget)
                {
                    _stateMachineController.AnimationManager.SetLinearToDirectionalParameter(toDirectionalBlendTree);
                    _stateMachineController.ThirdPersonController.LookAtTarget(
                        _stateMachineController.CameraController.CurrentLockOnTarget.transform.position,
                        !PlayerAnimationManager.Instance.IsInteracting);
                }
                else
                {
                    _stateMachineController.AnimationManager.SetLinearToDirectionalParameter(toLinearBlendTree);
                    _stateMachineController.ThirdPersonController.RotateToMovementDirection(!PlayerAnimationManager
                        .Instance.IsInteracting);
                }
            }
        }

        protected void HandleIdleStateTransition()
        {
            if (!InputController.IsMoving && _stateMachineController.ThirdPersonController.PlayerGrounded)
            {
                ChangeState(_stateMachine.IdleState());
            }
        }
        
        protected void HandleWalkStateTransition()
        {
            if (!InputController.SprintFlag && InputController.IsMoving && InputController.WalkToggle && _stateMachineController.ThirdPersonController.PlayerGrounded)
            {
                ChangeState(_stateMachine.WalkingState());
            }
        }
        
        protected void HandleJogStateTransition()
        {
            if (InputController.IsMoving && !InputController.SprintFlag && !InputController.WalkToggle && _stateMachineController.ThirdPersonController.PlayerGrounded)
            {
                ChangeState(_stateMachine.JoggingState());
            }
        }
        
        protected void HandleRunStateTransition()
        {
            if (InputController.IsMoving && InputController.SprintFlag && _stateMachineController.ThirdPersonController.PlayerGrounded)
            {
                ChangeState(_stateMachine.RunningState());
            }
        }

        protected void HandleCrouchStateTransition()
        {
            if (InputController.CrouchFlag && !PlayerAnimationManager.Instance.IsInteracting)
            {
                ChangeState(_stateMachine.CrouchingState());
            }
        }

        protected void HandleSlidingStateTransition()
        {
            
        }

        private void HandleFinisherState()
        {
            if (!_stateMachineController.AnimationManager.IsInteracting && _stateMachineController.FinisherComponent.CanTriggerFinisher)
            {
                ChangeState(_stateMachine.FinisherState());
            }
        }

        private void HandleAttackStates()
        {
            if (!PlayerAnimationManager.Instance.IsInteracting)
            {
                if (InputController.HeavyAttackFlag)
                {
                    ChangeState(_stateMachine.HeavyAttackState());
                }

                if (InputController.LightAttackFlag)
                {
                    if (!InputController.IsMoving || !InputController.SprintFlag)
                    {
                        ChangeState(_stateMachine.LightAttackState());
                    }
                    else
                    {
                        ChangeState(_stateMachine.SprintAttackState());
                    }
                }
            }
        }

        private void HandleWeaponEquip()
        {
            if (_canDrawWeapon)
            {
                WeaponManager.Instance.EquipWeapon();
            }
            else
            {
                if (_canSheatheWeapon)
                {
                    WeaponManager.Instance.SheatheWeapon();
                }
            }
        }
    }
}