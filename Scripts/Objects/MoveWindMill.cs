using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWindMill : MonoBehaviour
{

    private float angle;

    void Start()
    {
        InvokeRepeating("RotateObject", 1, 0.02f);
    }

    private void RotateObject ()
    {
     //   Debug.Log("Rotate");
        angle += Time.deltaTime;
   //     transform.Rotate(angle, 0.0f, 0.0f, Space.Self);
        transform.Rotate(Vector3.left, 65 * Time.deltaTime );
    }


}
