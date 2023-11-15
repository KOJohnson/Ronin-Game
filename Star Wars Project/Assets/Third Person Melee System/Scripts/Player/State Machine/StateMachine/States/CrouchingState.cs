using ThirdPersonMeleeSystem.Managers;

namespace ThirdPersonMeleeSystem.StateMachine
{
    public class CrouchingState : BaseLocomotion
    {
        public CrouchingState(StateMachineController stateMachineController, StateMachine stateMachine) : base(stateMachineController, stateMachine)
        {

        }
        
        private const float _crouchTarget = 1f;

        public override void EnterState()
        {
            base.EnterState();
            InputController.CrouchFlag = true;
            _stateMachineController.ThirdPersonController.SetPlayerSpeed(_stateMachineController.ThirdPersonController.WalkSpeed);
            _stateMachineController.AnimationManager.SetIsCrouching(true);
            _stateMachineController.ThirdPersonController.CrouchEnter();
        }

        protected override void ExitState()
        {
            base.ExitState();
            _stateMachineController.AnimationManager.SetIsCrouching(false);
            _stateMachineController.ThirdPersonController.CrouchExit();
        }

        public override void Tick(float delta)
        {
            base.Tick(delta);
            _stateMachineController.AnimationManager.SetAnimatorLinearVelocity(_crouchTarget);
            _stateMachineController.AnimationManager.SetAnimatorDirectionalVelocity(_crouchTarget);
        }

        public override void CheckSwitchState()
        {
            base.CheckSwitchState();

            if (!InputController.CrouchFlag)
            {
                ChangeState(_stateMachine.IdleState());
            }
        }
    }
}