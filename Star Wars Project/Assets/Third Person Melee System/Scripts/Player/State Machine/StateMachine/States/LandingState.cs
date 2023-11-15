namespace ThirdPersonMeleeSystem.StateMachine
{
    public class LandingState : BaseState
    {
        public LandingState(StateMachineController stateMachineController, StateMachine stateMachine) : base(stateMachineController, stateMachine)
        {
        }

        public override void EnterState()
        {
            //do this in animator controller
        }

        protected override void ExitState()
        {
            
        }

        public override void Tick(float delta)
        {
            
        }

        public override void CheckSwitchState()
        {
            
        }
    }
}