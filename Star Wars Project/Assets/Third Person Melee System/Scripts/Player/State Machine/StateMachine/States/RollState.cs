using ThirdPersonMeleeSystem.Managers;
using UnityEngine;

namespace ThirdPersonMeleeSystem.StateMachine
{
    public class RollState : BaseState
    {
        public RollState(StateMachineController stateMachineController, StateMachine stateMachine) : base(stateMachineController, stateMachine)
        {
        }
        
        private Vector2 _input;
        
        public override void EnterState()
        {
            _stateMachineController.AnimationManager.ResetAnimatorVelocityParameters();
            _input = _stateMachineController.InputController.GetRawMovementInput();
            PlayRollAnimation();
        }

        protected override void ExitState()
        {
            
        }

        public override void Tick(float delta)
        {
            if (_stateMachineController.CameraController.LockedOnTarget)
            {
                _stateMachineController.ThirdPersonController.LookAtTarget(_stateMachineController.CameraController.CurrentLockOnTarget.LockOnPoint(), true);
            }
            else
            {
                _stateMachineController.ThirdPersonController.RotateToMovementDirection(true);
            }
        }

        public override void CheckSwitchState()
        {
            if (!PlayerAnimationManager.Instance.IsInteracting)
            {
                ChangeState(_stateMachine.IdleState());
            }
        }
        
        private void PlayRollAnimation()
        {
            if (_stateMachineController.CameraController.LockedOnTarget)
            {
                if (_input == Vector2.zero)
                {
                    PlayerAnimationManager.Instance.PlayAction(WeaponManager.Instance.GetCurrentWeapon().LocomotionAsset
                        .rollBackward);
                }
                else if (_input == Vector2.up)
                {
                    PlayerAnimationManager.Instance.PlayAction(WeaponManager.Instance.GetCurrentWeapon().LocomotionAsset
                        .rollForward);
                }
                else if (_input == Vector2.down)
                {
                    PlayerAnimationManager.Instance.PlayAction(WeaponManager.Instance.GetCurrentWeapon().LocomotionAsset
                        .rollBackward);
                }
                else if (_input == Vector2.left)
                {
                    PlayerAnimationManager.Instance.PlayAction(WeaponManager.Instance.GetCurrentWeapon().LocomotionAsset
                        .rollLeft);
                }
                else if (_input == Vector2.right)
                {
                    PlayerAnimationManager.Instance.PlayAction(WeaponManager.Instance.GetCurrentWeapon().LocomotionAsset
                        .rollRight);
                }
            }
            else
            {
                PlayerAnimationManager.Instance.PlayAction(_input != Vector2.zero
                    ? WeaponManager.Instance.GetCurrentWeapon().LocomotionAsset.rollForward
                    : WeaponManager.Instance.GetCurrentWeapon().LocomotionAsset.rollBackward);
            }
        }
    }
}