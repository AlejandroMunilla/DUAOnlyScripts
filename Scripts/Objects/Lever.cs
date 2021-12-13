using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{

    private bool timeOut = true;

    public GameObject enableGO;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            EnableGO();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.name);
        if (other.tag == "Weapon")
        {
            
            if (timeOut == true)
            {
                EnableGO();
            }
        }

    }

    public void EnableGO()
    {
        enableGO.SetActive(true);
    }

    private void Animate ()
    {
    //    transform.rotation = 
    }

    /*
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.O))
        {
   //         Debug.Log("Enable");
            EnableGO();
        }
    }*/
}
