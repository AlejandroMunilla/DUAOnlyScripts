using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrto : MonoBehaviour
{
    bool MaintainWidth = true;
    Vector3 CameraPos;
    float DefaultWidth;
    float DefaultHeight;
    void Start()
    {
        CameraPos = transform.position;

        DefaultWidth = GetComponent<Camera>().orthographicSize; //* the resolution the game was designed for;
        DefaultHeight = GetComponent<Camera>().orthographicSize;
      
        if (MaintainWidth)
            {
                GetComponent<Camera>().orthographicSize = DefaultWidth / GetComponent<Camera>().aspect;
            }
        transform.position = new Vector3(CameraPos.x, -1 * (DefaultHeight - GetComponent<Camera>().orthographicSize), CameraPos.z);
    }
}
