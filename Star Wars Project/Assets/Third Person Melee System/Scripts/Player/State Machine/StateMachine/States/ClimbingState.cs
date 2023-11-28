using System;

namespace ThirdPersonMeleeSystem.StateMachine
{
    public class ClimbingState : BaseState
    {
        public ClimbingState(StateMachineController stateMachineController, StateMachine stateMachine) : base(stateMachineController, stateMachine)
        {
        }

        public override void EnterState()
        {
            
        }

        protected override void ExitState()
        {
            _stateMachineController.VaultComponent.ResetClimbing();
        }

        public override void Tick(float delta)
        {
            _stateMachineController.VaultComponent.MoveToEndPoint();
        }

        public override void CheckSwitchState()
        {
            if (_stateMachineController.VaultComponent.HasReachedDestination())
            {
                ChangeState(_stateMachine.IdleState());
            }
        }
    }
}