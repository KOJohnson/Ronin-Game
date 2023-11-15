using ThirdPersonMeleeSystem.Managers;

namespace ThirdPersonMeleeSystem.StateMachine
{
    public class JumpingState : BaseState
    {
        public JumpingState(StateMachineController stateMachineController, StateMachine stateMachine) : base(stateMachineController, stateMachine)
        {
        }

        public override void EnterState()
        {
            _stateMachineController.ThirdPersonController.Jump(_stateMachineController.ThirdPersonController.JumpVelocity);
            
            _stateMachineController.AnimationManager.PlayAction(WeaponManager.Instance.IsWeaponDrawn ?
                WeaponManager.Instance.GetCurrentWeapon().LocomotionAsset.combatJumpStart :
                _stateMachineController.ThirdPersonController.GetLocomotionAsset().jumpStart);
        }

        public override void Tick(float delta)
        {
            _stateMachineController.ThirdPersonController.MovePlayer();
        }

        protected override void ExitState()
        {
            
        }

        public override void CheckSwitchState()
        {
            if (!PlayerAnimationManager.Instance.IsInteracting)
            {
                ChangeState(_stateMachine.InAirState());
            }
            
            if (_stateMachineController.ThirdPersonController.PlayerGrounded && _stateMachineController.ThirdPersonController.YSpeed < 0)
            {
                ChangeState(_stateMachine.IdleState());
            }
        }
    }
}