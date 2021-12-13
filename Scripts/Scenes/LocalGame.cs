using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalGame : MonoBehaviour {

    private MainMenu mainMenu = null;

	// Use this for initialization
	private void OnEnable ()
    {
		if (mainMenu == null)
        {
            mainMenu = GetComponent<MainMenu>();
        }
	}

    private void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.45f, Screen.width * 0.2f, Screen.height * 0.08f), "NEW GAME"))
        {
           
        }
        if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.45f, Screen.width * 0.2f, Screen.height * 0.08f), "LOAD GAME"))
        {

        }
    }


}
