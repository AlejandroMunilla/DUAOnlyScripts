using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPView : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.root.gameObject.GetComponent<ThirdPersonUserControl>().fpViewTransform = transform;
    }

}
