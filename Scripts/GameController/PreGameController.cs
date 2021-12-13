using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PreGameController : MonoBehaviour
{
    public string control = null;
    public bool ip = false;
    private PreGame preGame;


    private void Start()
    {
        preGame = transform.parent.gameObject.GetComponent<PreGame>();
    }

    // Update is called once per frame

    /*
    void Update()
    {
        if (gameObject.name == "Player1")
        {
            preGame.Controller1(control);
        }
        else if (gameObject.name == "Player2")
        {
       //     Debug.Log("P2");
            preGame.Controller2(control, 0, 0);
        }
        else if (gameObject.name == "Player3")
        {
            preGame.Controller3(control,0,0);
        }
        else if (gameObject.name == "Player4")
        {
            preGame.Controller4(control, 0, 0);
        }
    }*/
}
