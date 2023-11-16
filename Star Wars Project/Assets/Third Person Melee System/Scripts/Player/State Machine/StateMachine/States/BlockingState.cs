using ThirdPersonMeleeSystem.Managers;

namespace ThirdPersonMeleeSystem.StateMachine
{
    public class BlockingState : BaseLocomotion
    {
        public BlockingState(StateMachineController stateMachineController, StateMachine stateMachine) : base(stateMachineController, stateMachine)
        {
        }

        public override void EnterState()
        {
            _stateMachineController.ThirdPersonController.SetPlayerSpeed(_stateMachineController.ThirdPersonController.WalkSpeed);
            _stateMachineController.AnimationManager.SetIsBlocking(true);
        }

        protected override void ExitState()
        {
            _stateMachineController.AnimationManager.SetIsBlocking(false);
        }

        public override void Tick(float delta)
        {
            _stateMachineController.AnimationManager.PlayController();
            _stateMachineController.AnimationManager.SetAnimatorLinearVelocity(1f);
            _stateMachineController.AnimationManager.SetAnimatorDirectionalVelocity(1f);
            
            HandleMovement();
            HandleRotation();
        }

        public override void CheckSwitchState()
        {
            if (!InputController.BlockFlag)
            {
                ChangeState(_stateMachine.IdleState());
            }
        }
    }
}