using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddShield : MonoBehaviour {



	void Start ()
    {
        transform.root.gameObject.GetComponent<PlayerStats>().shield = gameObject;
	}

}
