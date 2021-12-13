using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimStop : MonoBehaviour
{
    public string trigger = "No";
    private Animator anim;
    
	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
		if (trigger != "No")
        {
            anim.SetTrigger(trigger);
        }
	}

}
