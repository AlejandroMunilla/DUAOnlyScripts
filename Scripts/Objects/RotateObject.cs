using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {

    private int yRot = 0;
	// Use this for initialization
	void Start ()
    {
    //    InvokeRepeating("Rotate", 0, 0);
    
	}
	
	// Update is called once per frame
	void Update ()
    {
        yRot = yRot + 5;
        transform.rotation = Quaternion.Euler(yRot, 0, 0);
	}

    
}
