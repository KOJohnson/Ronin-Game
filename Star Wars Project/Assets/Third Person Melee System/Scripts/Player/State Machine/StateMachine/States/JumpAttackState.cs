using ThirdPersonMeleeSystem.Managers;
using UnityEngine;

namespace ThirdPersonMeleeSystem.StateMachine
{
    public class JumpAttackState : BaseAttackState
    {
        public JumpAttackState(StateMachineController stateMachineController, StateMachine stateMachine) : base(stateMachineController, stateMachine)
        {
        }

        private bool _groundedLastFrame;
        private bool _animPlayed;

        protected override void HandleAttackOnStateEnter()
        {
            PlayAttackAnimation(_stateMachineController.WeaponManager.GetCurrentWeapon().jumpAttackStart);
        }

        protected override void ExitState()
        {
            base.ExitState();
            _animPlayed = false;
        }

        public override void Tick(float delta)
        {
            base.Tick(delta);

            Physics.Raycast(_stateMachineController.transform.position, Vector3.down, out RaycastHit hit, 100f,
                _stateMachineController.ThirdPersonController.WhatIsGround);

            _stateMachineController.ThirdPersonController.MovePlayer();

            if (!_animPlayed)
            {
                _stateMachineController.AnimationManager.PlayAction(_stateMachineController.WeaponManager.GetCurrentWeapon().jumpAttackLoop);
            }
            
            if (!_animPlayed && hit.distance <= _stateMachineController.ThirdPersonController.DistanceToTriggerJumpAttack)
            {
                _animPlayed = true;
                PlayAttackAnimation(_stateMachineController.WeaponManager.GetCurrentWeapon().jumpAttackEnd);
            }
        }

        public override void CheckSwitchState()
        {
            if (_stateMachineController.ThirdPersonController.PlayerGrounded && !PlayerAnimationManager.Instance.IsInteracting)
            {
                ChangeState(_stateMachine.IdleState());
            }
        }
    }
}