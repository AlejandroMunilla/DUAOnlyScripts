using System.Collections;
using Rewired;
using UnityEngine;

public class CursorMouseRPG : MonoBehaviour
{
    private Camera cam;
    private RPGMenuController rpgController;
    private Player playerR;
    void OnEnable ()
    {
        cam = transform.Find("Camera").GetComponent<Camera>();
        rpgController = GetComponent<RPGMenuController>();
        playerR = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().player1.GetComponent<ThirdPersonUserControl>().playerR;
        Debug.Log(gameObject.name);
    }


    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 500))
        {
            Debug.Log(hit.transform.name + "/");
            if (playerR.GetButtonUp("Fire"))
            {
                rpgController.ColliderReceived(hit.collider, true);
                /*
                bool callScript = false;
                if (hit.collider.tag == "Head")
                {
                    callScript = true;
                }
                else if (hit.collider.name == "Right")
                {
                    callScript = true;
                }
                else if (hit.collider.name == "Left")
                {
                    callScript = true;
                }

                if (callScript == true)
                {
                    rpgController.ColliderReceived(hit.collider);
                }*/

            }

        }
    }
}

//     item1*qt*2*item2*item3*qt*1
