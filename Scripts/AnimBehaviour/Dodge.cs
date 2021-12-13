using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : StateMachineBehaviour
{
    GameObject player;
    ThirdPersonUserControl tpc = null;
    float timer;
    float timerControl;
    // Start is called before the first frame update
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (tpc == null)
        {
            player = animator.gameObject;
            tpc = player.GetComponent<ThirdPersonUserControl>();
        }
        timer = 0;
        if ((timerControl + 1) < Time.realtimeSinceStartup )
        {
            Jump();
        }

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Jump();
    }

    private void Jump ()
    {
        if (timer < 0.8f)
        {
            float h = tpc.h;
            float v = tpc.v;

            Vector3 direction = v * Vector3.forward + h * Vector3.right;
            player.transform.position += direction * Time.deltaTime * 2;

        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timerControl = Time.realtimeSinceStartup;
    }


}
