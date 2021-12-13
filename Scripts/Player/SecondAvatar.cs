using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondAvatar : MonoBehaviour
{

    private Animator anim;
    private Animator parentAnim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        parentAnim = transform.parent.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float forward = parentAnim.GetFloat("Forward");
        float turn = parentAnim.GetFloat("Turn");
        anim.SetFloat ("Forward", forward);
        anim.SetFloat("Turn", turn);
    }
}
