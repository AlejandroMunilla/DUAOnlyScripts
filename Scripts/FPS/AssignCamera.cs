using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class AssignCamera : MonoBehaviour
{


    // Start is called before the first frame update
    void Awake ()
    {
    //    transform.root.GetComponent<PlayerStats>().camGO = gameObject;
    }

    private void OnEnable()
    {
        if (transform.root.GetComponent<PlayerStats>() != null)
        {
            transform.root.GetComponent<PlayerStats>().camGO = gameObject;
        }   
        
        
    }


}
