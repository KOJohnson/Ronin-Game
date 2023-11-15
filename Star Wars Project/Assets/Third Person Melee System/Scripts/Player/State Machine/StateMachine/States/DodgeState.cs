using ThirdPersonMeleeSystem.Managers;
using ThirdPersonMeleeSystem.Timers;
using UnityEngine;

namespace ThirdPersonMeleeSystem.StateMachine
{
    public class DodgeState : BaseState
    {
        public DodgeState(StateMachineController stateMachineController, StateMachine stateMachine) : base(stateMachineController, stateMachine)
        {
        }
        
        //roll duration should always be a smaller value than dodge duration 
        private const float rollDuration = 0.45f;
        private const float dodgeDuration = 0.55f;
        private const float duration = 0.6f;
        private readonly Timer _rollTimer = new(duration);
        private readonly Timer _dodgeTimer = new(duration);
        private Vector2 _input;

        public override void EnterState()
        {
            _stateMachineController.AnimationManager.ResetAnimatorVelocityParameters();
            _input = _stateMachineController.InputController.GetRawMovementInput();
            PlayDodgeAnimation();
        }

        public override void Tick(float delta)
        {
            _rollTimer.Tick(delta);
            _dodgeTimer.Tick(delta);

            if (_stateMachineController.CameraController.LockedOnTarget)
            {
                _stateMachineController.ThirdPersonController.LookAtTarget(_stateMachineController.CameraController.CurrentLockOnTarget.LockOnPoint(), true);
            }
            else
            {
                _stateMachineController.ThirdPersonController.RotateToMovementDirection(true);
            }
        }

        protected override void ExitState()
        {
            _rollTimer.Reset();
            _dodgeTimer.Reset();
        }  

        public override void CheckSwitchState()
        {
            if (!PlayerAnimationManager.Instance.IsInteracting)
            {
                ChangeState(_stateMachine.IdleState());
            }
            
            if (InputController.DodgeFlag && _dodgeTimer.IsTimerComplete)
            {
                ChangeState(_stateMachine.DodgeState());
            }
            else if (InputController.DodgeFlag && !_dodgeTimer.IsTimerComplete)
            {
                ChangeState(_stateMachine.RollState());
            }
        }
        
        private void PlayDodgeAnimation()
        {
            if (_stateMachineController.CameraController.LockedOnTarget)
            {
                if (_input == Vector2.zero)
                {
                    PlayerAnimationManager.Instance.PlayAction(WeaponManager.Instance.GetCurrentWeapon().LocomotionAsset
                        .dodgeBackward);
                }
                else if (_input == Vector2.up)
                {
                    PlayerAnimationManager.Instance.PlayAction(WeaponManager.Instance.GetCurrentWeapon().LocomotionAsset
                        .dodgeForward);
                }
                else if (_input == Vector2.down)
                {
                    PlayerAnimationManager.Instance.PlayAction(WeaponManager.Instance.GetCurrentWeapon().LocomotionAsset
                        .dodgeBackward);
                }
                else if (_input == Vector2.left)
                {
                    PlayerAnimationManager.Instance.PlayAction(WeaponManager.Instance.GetCurrentWeapon().LocomotionAsset
                        .dodgeLeft);
                }
                else if (_input == Vector2.right)
                {
                    PlayerAnimationManager.Instance.PlayAction(WeaponManager.Instance.GetCurrentWeapon().LocomotionAsset
                        .dodgeRight);
                }
            }
            else
            {
                PlayerAnimationManager.Instance.PlayAction(_input != Vector2.zero
                    ? WeaponManager.Instance.GetCurrentWeapon().LocomotionAsset.dodgeForward
                    : WeaponManager.Instance.GetCurrentWeapon().LocomotionAsset.dodgeBackward);
            }
        }
    }
}