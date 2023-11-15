namespace ThirdPersonMeleeSystem.StateMachine
{
    public class LightAttackState : BaseAttackState
    {
        public LightAttackState(StateMachineController stateMachineController, StateMachine stateMachine) : base(stateMachineController, stateMachine)
        {
        }

        protected override void HandleAttackOnStateEnter()
        {
            HandleAttackCombo(_lightAttackIndex, this);
        }
    }
}