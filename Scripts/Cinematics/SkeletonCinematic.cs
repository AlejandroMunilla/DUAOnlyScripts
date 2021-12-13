using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCinematic : MonoBehaviour
{

    public bool idle1;
    public bool triggerIdle1;
    public bool idle1Delay = false;
    public bool idle2;
    public bool triggerIdle2;
    public bool idle3;
    public bool triggerIdle3;
    public bool laugh;
    
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if (triggerIdle1 == true)
        {
            if (idle1Delay == true)
            {
                Invoke("DelayIdle1", 1);
            }
            
        }
        if (triggerIdle2 == true)
        {
            anim.SetTrigger("Idle2");
 
        }
        if (triggerIdle3 == true)
        {
            anim.SetTrigger("Idle3");

        }
        if (laugh == true)
        {
            anim.SetTrigger("Laught1");
            anim.SetBool("LaughtBool", true);
        }
    }


    private void DelayIdle1()
    {
        anim.SetBool ("Idle1Bool", true);
    }
}
