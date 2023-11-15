using ThirdPersonMeleeSystem.Managers;
using ThirdPersonMeleeSystem.ScriptableObjects;
using UnityEngine;

namespace ThirdPersonMeleeSystem.StateMachine
{
    public abstract class BaseAttackState : BaseState
    {
        public BaseAttackState(StateMachineController stateMachineController, StateMachine stateMachine) : base(stateMachineController, stateMachine)
        {
        }
        
        protected int _lightAttackIndex;
        protected int _heavyAttackIndex;

        private Vector2 _inputDirection;
        private Quaternion _startRotation;

        private float _cameraY;
        private float _timeElapsed;
        private float _lerpDuration = 0.3f;

        public override void EnterState()
        {
            if (!_stateMachineController.WeaponManager.IsWeaponDrawn)
            {
                _stateMachineController.WeaponManager.FastEquipWeapon();
            }
            
            _stateMachineController.AnimationManager.ResetAnimatorVelocityParameters();
            HandleFreeFlowCombat();
            HandleAttackOnStateEnter();
            
            _timeElapsed = 0f;
            _cameraY = _stateMachineController.ThirdPersonController.MainCam.transform.eulerAngles.y;
            _inputDirection = _stateMachineController.InputController.GetRawMovementInput();
            _startRotation = _stateMachineController.transform.rotation;
        }

        protected override void ExitState()
        {
            WeaponManager.Instance.PlayerWeaponEvents.OnEnableCombo(false);
        }
        
        public override void Tick(float delta)
        {
            HandleRotation();
        }

        public override void CheckSwitchState()
        {
            if (!PlayerAnimationManager.Instance.IsInteracting)
            {
                ResetAttackIndex();
                
                if (!_stateMachineController.ThirdPersonController.PlayerGrounded)
                {
                    ChangeState(_stateMachine.InAirState());
                }
                else
                {
                    ChangeState(_stateMachine.IdleState()); 
                }
            }

            if (InputController.HeavyAttackFlag && WeaponManager.Instance.CanCombo)
            {
                ChangeState(_stateMachine.HeavyAttackState());
            }
            
            if (InputController.LightAttackFlag && WeaponManager.Instance.CanCombo)
            {
                ChangeState(_stateMachine.LightAttackState());
            }
            
            if (_stateMachineController.FinisherComponent.CanTriggerFinisher)
            {
                ChangeState(_stateMachine.FinisherState());
            }
            
            if (InputController.DodgeFlag)
            {
                ChangeState(_stateMachine.DodgeState());
            }
        }

        protected abstract void HandleAttackOnStateEnter();

        private void HandleRotation()
        {
            if (_stateMachineController.CameraController.HardLockOnTarget)
            {
                RotateToTarget(_stateMachineController.CameraController.CurrentLockOnTarget.LockOnPoint());
            }
            else if (_stateMachineController.FreeFlowCombatController.FreeFlowTarget != null)
            {
                RotateToTarget(_stateMachineController.FreeFlowCombatController.FreeFlowTarget.transform.position);
            }
            else
            {
                _stateMachineController.ThirdPersonController.RotateToAttackDirection(_inputDirection, _cameraY);
            }
        }

        private void HandleFreeFlowCombat()
        {
            if (_stateMachineController.CameraController.IsHardLockTarget) return;
            //if we have a target and no input we want to keep that same target 
            if (_stateMachineController.FreeFlowCombatController.FreeFlowTarget && _stateMachineController.InputController.GetCameraRelativeMovementDirection() == Vector3.zero) return;
            _stateMachineController.FreeFlowCombatController.GetPotentialTargets();
            //change soft lock target to the free flow target
            _stateMachineController.CameraController.ChangeSoftLockTarget(_stateMachineController.FreeFlowCombatController.FreeFlowTarget);
            
        }

        private void RotateToTarget(Vector3 target)
        {
            Vector3 direction = (target - _stateMachineController.transform.position).normalized;
            direction.y = 0;
            
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        
            if (_timeElapsed < _lerpDuration)
            {
                _stateMachineController.transform.rotation =
                    Quaternion.Slerp(_startRotation, rotation, _timeElapsed / _lerpDuration);
                _timeElapsed += Time.deltaTime;
            }
            else
            {
                _stateMachineController.transform.rotation = rotation;
            }
        }

        protected void HandleAttackCombo(int index, BaseAttackState attackState)
        {
            if (attackState.GetType() == typeof(LightAttackState))
            {
                if (WeaponManager.Instance.GetCurrentWeapon().lightAttacks.Length == 0) return;
                PlayAttackAnimation(WeaponManager.Instance.GetCurrentWeapon().lightAttacks[index]);
                _lightAttackIndex = (_lightAttackIndex + 1) % WeaponManager.Instance.GetCurrentWeapon().lightAttacks.Length;
            }
            if (attackState.GetType() == typeof(HeavyAttackState))
            {
                if (WeaponManager.Instance.GetCurrentWeapon().heavyAttacks.Length == 0) return;
                PlayAttackAnimation(WeaponManager.Instance.GetCurrentWeapon().heavyAttacks[index]);
                _heavyAttackIndex = (_heavyAttackIndex + 1) % WeaponManager.Instance.GetCurrentWeapon().heavyAttacks.Length;
            }
        }

        protected void PlayAttackAnimation(AttackData attackData)
        {
            if (attackData == null) return;
            if (attackData.attackAnimationData.attackAnimation == null) return;
            _stateMachineController.WeaponManager.SetLastAttack(attackData.attackAnimationData);
            _stateMachineController.AnimationManager.PlayAction(attackData.attackAnimationData.attackAnimation);
        }

        private void ResetAttackIndex()
        {
            _lightAttackIndex = 0;
            _heavyAttackIndex = 0;
        }
    }
}