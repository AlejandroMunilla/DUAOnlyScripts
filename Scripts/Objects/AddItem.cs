using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItem : MonoBehaviour
{

    private myGUI mygui;
    private LoadGameImp loadGame;
    private GameController gc;
    // Start is called before the first frame update
    void Start()
    {
        GameObject gcon = GameObject.FindGameObjectWithTag("GameController");
        mygui = gcon.GetComponent<myGUI>();
        gc = gcon.GetComponent<GameController>();
        loadGame = gcon.GetComponent<LoadGameImp>();
    }


    private void OnTriggerEnter(Collider other)
    {
   //     Debug.Log(other.gameObject.name);

        if (other.tag == "Player")
        {
            if (gc.isRPG == false)
            {
                GetComponent<CapsuleCollider>().enabled = false;
                gameObject.SetActive(false);
                mygui.AddItem(other.gameObject, gameObject.name);
                Destroy(gameObject);
            }
            else
            {
                loadGame.AddOneItem(gameObject.name);
            }

        }
    }
}
