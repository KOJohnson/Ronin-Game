
namespace ThirdPersonMeleeSystem.StateMachine
{
    public class HeavyAttackState : BaseAttackState
    {
        public HeavyAttackState(StateMachineController stateMachineController, StateMachine stateMachine) : base(stateMachineController, stateMachine)
        {
        }

        protected override void HandleAttackOnStateEnter()
        {
            HandleAttackCombo(_heavyAttackIndex, this);
        }
    }
}