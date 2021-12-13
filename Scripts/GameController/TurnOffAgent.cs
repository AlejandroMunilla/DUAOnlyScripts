using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public class TurnOffAgent : MonoBehaviour {


    public bool offLeft = true;
    private GameObject player;
    
	// Use this for initialization
	void Start ()
    {
		
	}


    private void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Player")
        {
            Execute(other);
        }
    }

    private void OnTriggerStay (Collider other)
    {
        if (other.tag == "Player")
        {
            Execute(other);
        }
    }

    private void Execute(Collider other)
    {
        Debug.Log("Player");
        player = other.gameObject;
        player.GetComponent<NavMeshAgent>().enabled = false;
        PlayerStats ps = player.GetComponent<PlayerStats>();
        
        if (offLeft == true)
        {
            ps.jumpPos = new Vector3((player.transform.position.x - 3), player.transform.position.y + 3, player.transform.position.z);
        }
        else
        {
            ps.jumpPos = new Vector3((player.transform.position.x + 3), player.transform.position.y + 2, player.transform.position.z);

        }
    }



}
