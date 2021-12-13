using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDead : StateMachineBehaviour
{
    // Start is called before the first frame update

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<PlayerStats>().checkedDead = true;
   //     Debug.Log("Checked");
    }

}
