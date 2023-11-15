using DG.Tweening;
using ThirdPersonMeleeSystem.Core;
using ThirdPersonMeleeSystem.Managers;
using ThirdPersonMeleeSystem.ScriptableObjects;
using UnityEngine;

namespace ThirdPersonMeleeSystem.StateMachine
{
    public class FinisherState : BaseState
    {
        public FinisherState(StateMachineController stateMachineController, StateMachine stateMachine) : base(stateMachineController, stateMachine)
        {
        }

        public override void EnterState()
        {
            //can disable damage in here so the player cant be hurt
            _stateMachineController.AnimationManager.ResetAnimatorVelocityParameters();
            _stateMachineController.CameraController.ClearLockOn();
            _stateMachineController.FinisherComponent.PerformFinisher();
        }

        protected override void ExitState()
        {
           _stateMachineController.FinisherComponent.ClearCurrentTarget();
        }

        public override void Tick(float delta)
        {
            
        }

        public override void CheckSwitchState()
        {
            if (!PlayerAnimationManager.Instance.IsInteracting)
            {
                ChangeState(_stateMachine.IdleState());
            }
        }

    }
}