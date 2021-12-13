using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OpenRoom : MonoBehaviour
{
    private bool roomOpen = false;

    private void Start()
    {
        gameObject.AddComponent<NavMeshObstacle>();
        GetComponent<NavMeshObstacle>().carving = true;
    }

    public void OpenDoor ()
    {
        if (roomOpen == false)
        {
            roomOpen = true;
            gameObject.tag = "Untagged";
            transform.Find("OpenRoom").gameObject.SetActive(true);
            transform.Find("Door").gameObject.SetActive(true);
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<NavMeshObstacle>().enabled = false;
        }
        
    }
}
