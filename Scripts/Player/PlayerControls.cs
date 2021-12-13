using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControls : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}

    private void Update()
    {
        if (Input.GetKeyUp ("joystick 1 button 0"))
        {
            Debug.Log("button 0 / 1");
        }

        if (Input.GetKeyUp("joystick 2 button 0"))
        {
            Debug.Log("button 0 / 2");
        }

        if (Input.GetKeyUp("joystick 2 button 1"))
        {
            Debug.Log("button 1 / 2");
        }


        if (Input.GetKeyUp("joystick button 1"))
        {
            Debug.Log("button 1");
        }
        if (Input.GetKeyUp("joystick button 2"))
        {
            Debug.Log("button 2");
        }
        if (Input.GetKeyUp("joystick button 3"))
        {
            Debug.Log("button 3");
        }
        if (Input.GetKeyUp("joystick button 4"))
        {
            Debug.Log("button 4");
        }
        if (Input.GetKeyUp("joystick button 5"))
        {
            Debug.Log("button 5");
        }
        if (Input.GetKeyUp("joystick button 6"))
        {
            Debug.Log("button 6");
        }
        if (Input.GetKeyUp("space"))
        {
            Debug.Log("button 7");
        }
        if (Input.GetKeyUp("backspace"))
        {
            Debug.Log("button 8");
        }

    //    Debug.Log(CrossPlatformInputManager.GetAxis("joystick 1_X"));
    //    Debug.Log(CrossPlatformInputManager.GetAxis("joystick 2_Y"));


        //    Debug.Log(CrossPlatformInputManager.GetAxis("Horizontal"));

    }


}
