
using ThirdPersonMeleeSystem.Managers;

namespace ThirdPersonMeleeSystem.StateMachine
{
    public class RunningState : BaseLocomotion
    {
        public RunningState(StateMachineController stateMachineController, StateMachine stateMachine) : base(stateMachineController, stateMachine)
        {
        }
        
        private const float _runTarget = 3f;

        public override void EnterState()
        {
            base.EnterState();
            _stateMachineController.ThirdPersonController.SetPlayerSpeed(_stateMachineController.ThirdPersonController.SprintSpeed);
        }

        public override void Tick(float delta)
        {
            base.Tick(delta);
            _stateMachineController.AnimationManager.SetAnimatorLinearVelocity(_runTarget);
            _stateMachineController.AnimationManager.SetAnimatorDirectionalVelocity(_runTarget);
        }

        public override void CheckSwitchState()
        {
            base.CheckSwitchState();
            HandleWalkStateTransition();
            HandleJogStateTransition();
            HandleIdleStateTransition();

            if (InputController.SlideFlag)
            {
                ChangeState(_stateMachine.SlidingState());
            }
        }
    }
}

