using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPreGame : MonoBehaviour {

    private PlayerPreGame ppGame;
    private string namePlayer;
    private float timer;


    // Use this for initialization
    void Start ()
    {
        Transform parentButton = transform.parent;
        Debug.Log(parentButton);
        ppGame = parentButton.gameObject.GetComponent<PlayerPreGame>();
        namePlayer = transform.parent.gameObject.name;
        timer = Time.timeSinceLevelLoad;
    }

    /*
    private void OnMouseUp()
    {
        Debug.Log("Button");
       if (timer < Time.timeSinceLevelLoad + 2)
        {
            if (gameObject.name == "Right")
            {

                ppGame.ChangeModel(true);
            }
            else if (gameObject.name == "Left")
            {
                ppGame.ChangeModel(false);
            }

            else
            {
                Debug.Log("Missing button");
            }
           timer = Time.timeSinceLevelLoad;
        }

    }*/
}
