using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAfter : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Invoke("ActivateCall", 3);
    }

    

    private void ActivateCall ()
    {
        transform.Find("Camera").GetComponent<Camera>().enabled = true;
    }
}
