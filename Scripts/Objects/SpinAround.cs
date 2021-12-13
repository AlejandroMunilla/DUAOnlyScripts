using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAround : MonoBehaviour
{

    private Transform target ;
    // Start is called before the first frame update
    void OnEnable()
    {
        target = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(target.position, Vector3.up, 27 * Time.deltaTime);
    }
}
