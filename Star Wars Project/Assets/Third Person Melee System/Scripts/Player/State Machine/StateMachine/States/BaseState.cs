namespace ThirdPersonMeleeSystem.StateMachine
{
    public abstract class BaseState
    {
        protected readonly StateMachineController _stateMachineController;
        protected readonly StateMachine _stateMachine;

        protected BaseState(StateMachineController stateMachineController, StateMachine stateMachine)
        {
            _stateMachineController = stateMachineController;
            _stateMachine = stateMachine;
        }

        public abstract void EnterState();
        protected abstract void ExitState();
        public abstract void Tick(float delta);
        public abstract void CheckSwitchState();

        protected void ChangeState(BaseState newState)
        {
            _stateMachineController.PlayerStateMachine.lastState = this;
            ExitState();
            
            newState.EnterState();
            _stateMachineController.PlayerStateMachine.currentState = newState;
        }
    }
}
