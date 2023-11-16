using System.Collections.Generic;

namespace ThirdPersonMeleeSystem.StateMachine
{
    public class StateMachine
    {
        public BaseState currentState;
        public BaseState lastState;
        
        private enum _states
        {
            IDLE,
            WALK,
            JOG,
            RUN,
            LIGHTATTACK,
            HEAVYATTACK,
            SPRINTATTACK,
            INAIRSTATE,
            DODGESTATE,
            ROLLSTATE,
            FINISHERSTATE,
            JUMPINGSTATE,
            SLIDINGSTATE,
            CROUCHINGSTATE,
            BLOCKINGSTATE
        }
        private readonly Dictionary<_states, BaseState> _stateDictionary = new();
        private StateMachineController _stateMachineController;

        public StateMachine(StateMachineController stateMachineController)
        {
            _stateDictionary[_states.IDLE] = new IdleState(stateMachineController, this);
            _stateDictionary[_states.WALK] = new WalkingState(stateMachineController, this);
            _stateDictionary[_states.JOG] = new JoggingState(stateMachineController, this);
            _stateDictionary[_states.RUN] = new RunningState(stateMachineController, this);
            _stateDictionary[_states.LIGHTATTACK] = new LightAttackState(stateMachineController, this);
            _stateDictionary[_states.HEAVYATTACK] = new HeavyAttackState(stateMachineController, this);
            _stateDictionary[_states.SPRINTATTACK] = new SprintAttackState(stateMachineController, this);
            _stateDictionary[_states.INAIRSTATE] = new InAirState(stateMachineController, this);
            _stateDictionary[_states.DODGESTATE] = new DodgeState(stateMachineController, this);
            _stateDictionary[_states.ROLLSTATE] = new RollState(stateMachineController, this);
            _stateDictionary[_states.FINISHERSTATE] = new FinisherState(stateMachineController, this);
            _stateDictionary[_states.JUMPINGSTATE] = new JumpingState(stateMachineController, this);
            _stateDictionary[_states.SLIDINGSTATE] = new SlidingState(stateMachineController, this);
            _stateDictionary[_states.CROUCHINGSTATE] = new CrouchingState(stateMachineController, this);
            _stateDictionary[_states.BLOCKINGSTATE] = new BlockingState(stateMachineController, this);
        }
        
        public void Initialise(BaseState startingState)
        {
            currentState = startingState;
            startingState.EnterState();
        }

        public BaseState IdleState()
        {
            return _stateDictionary[_states.IDLE];
        }
        
        public BaseState WalkingState()
        {
            return _stateDictionary[_states.WALK];
        }

        public BaseState JoggingState()
        {
            return _stateDictionary[_states.JOG];
        }
        
        public BaseState RunningState()
        {
            return _stateDictionary[_states.RUN];
        }

        public BaseState LightAttackState()
        {
            return _stateDictionary[_states.LIGHTATTACK];
        }
        
        public BaseState HeavyAttackState()
        {
            return _stateDictionary[_states.HEAVYATTACK];
        }
        
        public BaseState SprintAttackState()
        {
            return _stateDictionary[_states.SPRINTATTACK];
        }

        public BaseState InAirState()
        {
            return _stateDictionary[_states.INAIRSTATE];
        }

        public BaseState DodgeState()
        {
            return _stateDictionary[_states.DODGESTATE];
        }
        
        public BaseState RollState()
        {
            return _stateDictionary[_states.ROLLSTATE];
        }

        public BaseState FinisherState()
        {
            return _stateDictionary[_states.FINISHERSTATE];
        }
        
        public BaseState JumpingState()
        {
            return _stateDictionary[_states.JUMPINGSTATE];
        }
        
        public BaseState SlidingState()
        {
            return _stateDictionary[_states.SLIDINGSTATE];
        }
        
        public BaseState CrouchingState()
        {
            return _stateDictionary[_states.CROUCHINGSTATE];
        }

        public BaseState BlockingState()
        {
            return _stateDictionary[_states.BLOCKINGSTATE];
        }
    }
}


