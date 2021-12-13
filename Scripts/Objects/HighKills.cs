using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public class HighKills : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player");
            GameObject go = other.gameObject;
            PlayerStats ps = go.GetComponent<PlayerStats>();
            NavMeshAgent nav = go.GetComponent<NavMeshAgent>();
            ThirdPersonUserControl tpc = go.GetComponent<ThirdPersonUserControl>();
            tpc. CancelInvoke("CheckGround");
            nav.enabled = true;
            nav.isStopped = false;
            tpc.jumping = false;
            go.transform.position = ps.jumpPos;
            go.GetComponent<ThirdPersonCharacter>().enabled = false;
            tpc.enabled = false;
            //    GetComponent<ThirdPersonUserControl>().enabled = false;
            nav.isStopped = true;
            go.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            ps.health = 0;
            ps.ResetDeath();
        }
    }


    /*
    private void OnTriggerStay (Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player");
            GameObject go = other.gameObject;
            PlayerStats ps = go.GetComponent<PlayerStats>();
            NavMeshAgent nav = go.GetComponent<NavMeshAgent>();
            ThirdPersonUserControl tpc = go.GetComponent<ThirdPersonUserControl>();
            tpc.CancelInvoke("CheckGround");
            nav.enabled = true;
            nav.isStopped = false;
            tpc.jumping = false;
            transform.position = ps.jumpPos;
            GetComponent<ThirdPersonCharacter>().enabled = false;
            tpc.enabled = false;
            //    GetComponent<ThirdPersonUserControl>().enabled = false;
            nav.isStopped = true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            ps.health = 0;
            ps.ResetDeath();
        }
    }*/


}
