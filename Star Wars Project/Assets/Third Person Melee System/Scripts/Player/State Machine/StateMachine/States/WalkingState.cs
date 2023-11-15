namespace ThirdPersonMeleeSystem.StateMachine
{
    public class WalkingState : BaseLocomotion
    {
        public WalkingState(StateMachineController stateMachineController, StateMachine stateMachine) : base(stateMachineController, stateMachine)
        {
        }

        private const float _walkTarget = 1f;

        public override void EnterState()
        {
            base.EnterState();
            _stateMachineController.ThirdPersonController.SetPlayerSpeed(_stateMachineController.ThirdPersonController.WalkSpeed);
        }

        public override void Tick(float delta)
        {
            base.Tick(delta);
            _stateMachineController.AnimationManager.SetJogToWalkParameter(_walkTarget);
            _stateMachineController.AnimationManager.SetAnimatorLinearVelocity(_walkTarget);
            _stateMachineController.AnimationManager.SetAnimatorDirectionalVelocity(_walkTarget);
        }

        public override void CheckSwitchState()
        {
            base.CheckSwitchState();
            HandleIdleStateTransition();
            HandleJogStateTransition();
            HandleRunStateTransition();
            HandleCrouchStateTransition();
        }
    } 
}

