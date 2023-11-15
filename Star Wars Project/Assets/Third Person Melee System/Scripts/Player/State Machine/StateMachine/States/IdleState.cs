namespace ThirdPersonMeleeSystem.StateMachine
{
    public class IdleState : BaseLocomotion
    {
        public IdleState(StateMachineController stateMachineController, StateMachine stateMachine) : base(stateMachineController, stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            _stateMachineController.ThirdPersonController.SetPlayerSpeed(0f);
        }

        public override void Tick(float delta)
        {
            base.Tick(delta);
            _stateMachineController.AnimationManager.SetAnimatorLinearVelocity(0f);
            _stateMachineController.AnimationManager.SetAnimatorDirectionalVelocity(0f);
        }

        public override void CheckSwitchState()
        {
            base.CheckSwitchState();
            HandleWalkStateTransition();
            HandleJogStateTransition();
            HandleRunStateTransition();
            HandleCrouchStateTransition();
        }
    }
}

