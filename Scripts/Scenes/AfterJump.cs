using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterJump : MonoBehaviour {



    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            GameObject go = other.gameObject;
            go.GetComponent<ThirdPersonUserControl>(). CancelInvoke("CheckGround");
            go.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
            go.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = false;
            go.GetComponent<ThirdPersonUserControl>().jumping = false;
        }
    }
}
