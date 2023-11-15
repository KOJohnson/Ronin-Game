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
            //do animation in animator controller
        }

        protected override void ExitState()
        {
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