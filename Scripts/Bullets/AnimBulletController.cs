using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimBulletController : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject caster = animator.gameObject;
        Debug.Log(caster);
        caster.GetComponent<PlayerAttack>().WaitForAnimation();
    }
}
