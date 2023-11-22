using ThirdPersonMeleeSystem.Managers;

namespace ThirdPersonMeleeSystem.StateMachine
{
    public class InAirState : BaseState
    {
        public InAirState(StateMachineController stateMachineController, StateMachine stateMachine) : base(stateMachineController, stateMachine)
        {
        }

        public override void EnterState()
        {
        }

        public override void Tick(float delta)
        {
            _stateMachineController.ThirdPersonController.MovePlayer();
            
            _stateMachineController.AnimationManager.PlayAction(WeaponManager.Instance.IsWeaponDrawn ?
                WeaponManager.Instance.GetCurrentWeapon().LocomotionAsset.combatJumpLoop :
                _stateMachineController.ThirdPersonController.GetLocomotionAsset().jumpLoop);
            
            if (_stateMachineController.CameraController.LockedOnTarget)
            {
                _stateMachineController.ThirdPersonController.LookAtTarget(
                    _stateMachineController.CameraController.CurrentLockOnTarget.transform.position, true);
            }
            else
            {
                _stateMachineController.ThirdPersonController.RotateToMovementDirection(true);
            }
        }

        protected override void ExitState()
        {
            _stateMachineController.AnimationManager.PlayAction(WeaponManager.Instance.IsWeaponDrawn ?
                WeaponManager.Instance.GetCurrentWeapon().LocomotionAsset.combatJumpEnd :
                _stateMachineController.ThirdPersonController.GetLocomotionAsset().jumpEnd);
        }

        public override void CheckSwitchState()
        {
            if (_stateMachineController.ThirdPersonController.PlayerGrounded)
            {
                ChangeState(_stateMachine.IdleState());
            }
        }
    }
}