
namespace ThirdPersonMeleeSystem.StateMachine
{
    public class JoggingState : BaseLocomotion
    {
        public JoggingState(StateMachineController stateMachineController, StateMachine stateMachine) : base(stateMachineController, stateMachine)
        {
        }

        private const float _linearJogTarget = 2f;
        private const float _jogBlendTree = 0f;

        public override void EnterState()
        {
            base.EnterState();
            _stateMachineController.ThirdPersonController.SetPlayerSpeed(_stateMachineController.ThirdPersonController.JogSpeed);
        }

        public override void Tick(float delta)
        {
            base.Tick(delta);
            _stateMachineController.AnimationManager.SetJogToWalkParameter(_jogBlendTree);
            _stateMachineController.AnimationManager.SetAnimatorLinearVelocity(_linearJogTarget);
            _stateMachineController.AnimationManager.SetAnimatorDirectionalVelocity(_linearJogTarget);
        }

        public override void CheckSwitchState()
        {
            base.CheckSwitchState();
            HandleWalkStateTransition();
            HandleIdleStateTransition();
            HandleRunStateTransition();
            HandleCrouchStateTransition();
        }
    }
}