using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnTime : MonoBehaviour {

    public float timer = 0.8f;
	// Use this for initialization
	void OnEnable ()
    {

            Invoke("InvokeDisable", timer);

        
	}
	
    private void InvokeDisable ()
    {
        gameObject.SetActive(false);
        Invoke("CheckDisable", 0);
    }


    private void CheckDisable ()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            Invoke("CheckDisable", 0);
        }

    }


}
