using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class VirtualCursor : MonoBehaviour
{

    public Transform cursor;
    private Player playerR;
    private Camera cam;
    private Collider currentOther;
    private SphereCollider sc;
    private RPGMenuController rpgController;
    // Start is called before the first frame update
    void OnEnable ()
    {
        cam = transform.parent.Find("Camera").gameObject.GetComponent<Camera>();
        rpgController = transform.parent.GetComponent<RPGMenuController>();
        sc = GetComponent<SphereCollider>();
        playerR = GameObject.FindGameObjectWithTag("GameController"). GetComponent<GameController>().player1.GetComponent<ThirdPersonUserControl>().playerR;
    }

    private void Update()
    {
  //      Debug.Log("hey");
        
        float h = playerR.GetAxis("AxisX2");
        float v = playerR.GetAxis("AxisY2");
        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);

        if (viewPos.y < 0.005f)
        {
            v = 1;
        }

        if (viewPos.y > 0.995f)
        {
            v = -1;
        }

        if (viewPos.x < 0.005f)
        {
            h = 1;
        }
        else if (viewPos.x > 0.995f)
        {
            h = -1;
        }
        float xMove = h * Time.unscaledDeltaTime * 10;
        float yMove = v * Time.unscaledDeltaTime * 10;
        transform.position = new Vector3(transform.position.x + xMove, transform.position.y, transform.position.z + yMove);

        if (playerR.GetButtonUp("Fire") || playerR.GetButtonUp ("Fire2"))
        {

            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.2f);

            foreach (Collider col in colliders)
            {
          //      Debug.Log(col.name + "/" + col.transform.parent.name);

                rpgController.ColliderReceived(col, true);
            }

        }

        /*
        if (rpgController.state == RPGMenuController.State.Carrying)
        {
            if (playerR.GetButtonUp("Fire") || playerR.GetButtonUp("Fire2"))
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, 0.2f);
                foreach (Collider col in colliders)
                {
                    if (col.transform.parent.name == "QuickSlots")
                    {
                        rpgController.SwitchSprite(col.transform);
                    }
                }
            }
        }*/
        Cursor.visible = false;
    }


    private void LateUpdate()
    {
        Cursor.visible = false;
    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.name);

    }*/
}
