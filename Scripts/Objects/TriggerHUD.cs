using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHUD : MonoBehaviour
{
    public GameObject objectToTrigger;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        objectToTrigger.GetComponent<ObjectHUD>().enabled = true;
        gameObject.SetActive(false);
    }
}
