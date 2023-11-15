using UnityEngine;
using UnityEngine.Animations;

public class ResetIsInteracting : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        animator.SetBool("IsInteracting", false);
    }
    
}
