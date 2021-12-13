using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTransparency : MonoBehaviour
{

    public GameObject[] objectsControl;
    private bool changed = false;
    private Color originalColor;
    private GameController gc;
    private Camera mainCamera;

    private void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        mainCamera = Camera.main;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (changed == false)
        {
            
            if (other.tag == "Player")
            {

                Color color = GetComponent<MeshRenderer>().material.color;
                originalColor = color;
                color.a = 0.2f;
                foreach (GameObject go in objectsControl)
                {
                    go.GetComponent<MeshRenderer>().material.color = color;
                    GetComponent<BoxCollider>().enabled = false;
                }

                
             //  Invoke("CheckBackSolid", 1);
            }
        }


    }

    private void CheckBackSolid ()
    {

        bool allThrough = true;

        foreach (GameObject go in gc.players)
        {
           if (go.transform.position.x < transform.position.x + 3)
            {
                allThrough = false;
            }

        }

        if (mainCamera.GetComponent<CameraController>().move == true)
        {
            if (mainCamera.transform.position.x >= transform.position.x - 1)
            {
                mainCamera.GetComponent<CameraController>().move = false;
            }
        }

        if (allThrough == true)
        {
            Debug.Log(allThrough);
            foreach (GameObject go2 in objectsControl)
            {
                originalColor.a = 1;
                go2.GetComponent<MeshRenderer>().material.color = originalColor;
                mainCamera.GetComponent<CameraController>().move = true;
            }
        }
        else
        {
            Invoke("CheckBackSolid", 1);
        }
    }

}
