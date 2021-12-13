using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoseBlock : StateMachineBehaviour
{
    private float timer;
    GameObject barrier = null;
    GameObject rose = null;


    override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (barrier == null)
        {
            //    GameObject rose = animator.gameObject;
            //     Debug.Log(rose);
            rose = animator.gameObject;
            barrier = rose.transform.Find("Block").gameObject;
      //      Debug.Log(animator.gameObject);

        }

        if (barrier.activeSelf == false)
        {
            barrier.SetActive(true);
        }

        timer++;
   //     Debug.Log(timer);
    }

    override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        barrier.SetActive(false);

        if (timer > 4)
        {
            timer = 0;
            rose.GetComponent<PlayerStats>().AddjustMana(-1, rose);
        }
    }

}
