using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeCoin : MonoBehaviour {

    public int coins = 10;
	// Use this for initialization
	void Start ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerStats>().coins = other.gameObject.GetComponent<PlayerStats>().coins + coins;
            gameObject.SetActive(false);
        }
    }


}
