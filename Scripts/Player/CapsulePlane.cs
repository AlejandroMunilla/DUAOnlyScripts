using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsulePlane : MonoBehaviour {

    private CameraController cc;
    // Use this for initialization
    void Start ()
    {
   //     Debug.Log(gameObject.name);
     //   cc = Camera.main.GetComponent<CameraController>();
    }

    private void OnTriggerEnter(Collider other)
    {
   //     Debug.Log(gameObject.name);
        if (other.name == "PlaneD")
        {
            cc.planeD = Time.timeSinceLevelLoad;
            cc.planeI = Time.timeSinceLevelLoad;


        }
    }
}
