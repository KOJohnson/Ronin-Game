using ThirdPersonMeleeSystem.Player;
using ThirdPersonMeleeSystem.Managers;
using UnityEngine;

namespace ThirdPersonMeleeSystem.StateMachine
{
    public class StateMachineController : MonoBehaviour
    {
        #region StateMachine

        public StateMachine PlayerStateMachine { get; private set; }

        #endregion
    
        #region Public Fields
        #endregion
    
        #region Private Fields
        #endregion
    
        #region Serialized Fields
        

        #endregion

        #region Getters

        [field:SerializeField]public ThirdPersonController ThirdPersonController { get; private set;}
        [field:SerializeField]public PlayerAnimationManager AnimationManager { get; private set;}
        [field:SerializeField]public InputController InputController { get; private set;}
        [field:SerializeField]public WeaponManager WeaponManager { get; private set;}
        [field:SerializeField]public CameraController CameraController { get; private set;}
        [field:SerializeField]public FreeflowCombatController FreeFlowCombatController { get; private set;}
        [field:SerializeField]public FinisherComponent FinisherComponent { get; private set;}

        #endregion
    
        #region Debug

        [SerializeField] private bool debugStateMachine;
        [SerializeField] private int debugFontSize;

        #endregion

        private void Start()
        {
            PlayerStateMachine = new StateMachine(this);
            PlayerStateMachine.Initialise(PlayerStateMachine.IdleState());
        }

        private void Update()
        {
            PlayerStateMachine.currentState.Tick(Time.deltaTime);
            PlayerStateMachine.currentState.CheckSwitchState();
        }

        private void OnGUI()
        {
            if (!debugStateMachine) return;
            GUI.skin.label.fontSize = debugFontSize;
            GUI.Label(new Rect(10,10,800,100), PlayerStateMachine.currentState.ToString());
        }
    }
}
