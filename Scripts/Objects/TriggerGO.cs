using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGO : MonoBehaviour
{
    public GameObject activateGO;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log(other.name);
            activateGO.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log(other.name);
        }

    }


}
