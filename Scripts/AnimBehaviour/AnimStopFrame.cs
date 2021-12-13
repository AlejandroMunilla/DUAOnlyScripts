using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimStopFrame : StateMachineBehaviour
{
    public string animation = "nil";
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(layerIndex);

        animator.PlayInFixedTime(animation, layerIndex, 0.5f);
        
        animator.applyRootMotion = false;
    }

}
