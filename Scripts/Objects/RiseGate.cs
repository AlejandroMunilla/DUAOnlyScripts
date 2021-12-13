using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseGate : MonoBehaviour
{
    private AudioSource audio;
    public Transform gateTa;
    private float timer = 0;


    private void Start()
    {
        audio = gateTa.gameObject.GetComponent<AudioSource>();
        if (transform.Find("Fire"))
        {
            transform.Find("Fire").gameObject.SetActive(false);
        }

        gateTa.gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = true;
        gateTa.gameObject.GetComponent<BoxCollider>().enabled = true;
        gateTa.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
    public void LightFire()
    {
        transform.Find("Fire").gameObject.SetActive(true);
        GetComponent<BoxCollider>().enabled = false;
        Invoke ("RiseAction", 0);
        audio.Play();
        if (transform.Find ("Effect") != null)
        {
            transform.Find("Effect").gameObject.SetActive(false);
        }
    }



    private void RiseAction ()
    {
     //   Debug.Log("riseaction");
        float goY = gateTa.transform.position.y + (0.2f * Time.deltaTime);
        gateTa.position = new Vector3(gateTa.position.x, goY, gateTa.position.z);

        timer += Time.deltaTime;

   //     Debug.Log(timer);
        
        if (gateTa.position.y > 8.2f || timer > 14)
        {
            EndEffect();
        }
        
        else
        {
            Invoke("RiseAction", 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            LightFire();
        }
    }

    private void EndEffect ()
    {
        CancelInvoke("RiseAction");
        gateTa.gameObject.GetComponent<BoxCollider>().enabled = false;
        gateTa.gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
        gateTa.gameObject.GetComponent<BoxCollider>().enabled = false;
        //      GetComponent<MeshRenderer>().material = inactiveMaterial;
        audio.Stop();
        audio.enabled = false;
    }
}
