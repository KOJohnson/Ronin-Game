using ThirdPersonMeleeSystem.Timers;
using UnityEngine;

namespace ThirdPersonMeleeSystem.StateMachine
{
    public class SlidingState : BaseState
    {
        public SlidingState(StateMachineController stateMachineController, StateMachine stateMachine) : base(stateMachineController, stateMachine)
        {
        }

        private Timer slideTimer = new();
        
        //capsule cast above player at height of character controller 
        //if we are hitting anything set IsAbove flag to true

        public override void EnterState()
        {
            slideTimer.SetDuration(_stateMachineController.ThirdPersonController.SlideDuration);
            _stateMachineController.ThirdPersonController.CrouchEnter();
            _stateMachineController.AnimationManager.PlayAction(_stateMachineController.ThirdPersonController.GetLocomotionAsset().slideStart);
        }

        protected override void ExitState()
        {
            slideTimer.Reset();
            _stateMachineController.AnimationManager.PlayAction(_stateMachineController.ThirdPersonController.GetLocomotionAsset().slideEnd);
        }

        public override void Tick(float delta)
        {
            slideTimer.Tick(delta);
            
            _stateMachineController.AnimationManager.PlayAction(_stateMachineController.ThirdPersonController.GetLocomotionAsset().slideLoop);

            _stateMachineController.ThirdPersonController.Slide
                (_stateMachineController.transform.forward, _stateMachineController.ThirdPersonController.SlideSpeed);
            
        }

        public override void CheckSwitchState()
        {
            if (slideTimer.IsTimerComplete || _stateMachineController.InputController.GetRawMovementInput() == Vector2.zero)
            {
                ChangeState(_stateMachine.CrouchingState());
            }
        }
    }
}