using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PedastalForGate : MonoBehaviour
{

 //   private float timer = 0;
    private float maxHigh = 3;
    private float initialHigh;
    public Material inactiveMaterial;
    private Transform gateTa;
    private GameObject player = null;
    private bool opened = false;
    private AudioSource audio;
    public GameObject gate;

    // Start is called before the first frame update
    void Start()
    {
        gateTa = gate.transform;
        initialHigh = gateTa.transform.position.y;
        audio = gate.GetComponent<AudioSource>();
        gate.GetComponent<NavMeshObstacle>().enabled = true;
        gate.GetComponent<BoxCollider>().enabled = true;
        gate.GetComponent<MeshRenderer>().enabled = true;
   //     Debug.Log(gateTa.transform.position.y);

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {

           if (player == null)
            {
                player = other.gameObject;
            }


            Debug.Log(gateTa.transform.position.y);
            if (gateTa.transform.position.y > 8f)
            {
                GetComponent<BoxCollider>().enabled = false;
                gate.transform.Find("Obstacle").gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
                gate.GetComponent<NavMeshObstacle>().enabled = false;
                gate.GetComponent<BoxCollider>().enabled = false;
                GetComponent<MeshRenderer>().material = inactiveMaterial;
                audio.Stop();
                audio.enabled = false;
            }
            else
            {
                float goY = gateTa.transform.position.y + Time.deltaTime;
                gateTa.position = new Vector3(gateTa.position.x, goY, gateTa.position.z);
            }

            if (audio.isPlaying == false)
            {
                audio.Play();
            }
            
    

        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        player = null;
        audio.Stop();

    }

    /*
    private void NoPlayer ()
    {
        Debug.Log(gateTa.transform.localPosition.y +"/" +  initialHigh + "/" + player);
        float goY = gateTa.transform.position.y - Time.deltaTime;
        gateTa.position = new Vector3(gateTa.position.x, goY, gateTa.position.z);

        if (audio.isPlaying == false)
        {
            audio.Play();
        }

        if (gateTa.transform.localPosition.y <= initialHigh)
        {
            CancelInvoke("NoPlayer");
     //       gate.transform.Find("Obstacle").gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
            audio.Stop();
        }

    }

    private void CheckPlayerInside ()
    {
        
    }*/
}
