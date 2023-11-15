using ThirdPersonMeleeSystem.Managers;

namespace ThirdPersonMeleeSystem.StateMachine
{
    public class SprintAttackState : BaseAttackState
    {
        public SprintAttackState(StateMachineController stateMachineController, StateMachine stateMachine) : base(stateMachineController, stateMachine)
        {
        }

        protected override void HandleAttackOnStateEnter()
        {
            PlayAttackAnimation(WeaponManager.Instance.GetCurrentWeapon().sprintAttack);
        }
    }
}