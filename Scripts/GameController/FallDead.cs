using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDead : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Execute(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.tag == "Player")
        {
            Execute(other);
        }
    }

    private void Execute (Collider other)
    {
        GameObject go = other.gameObject;
        PlayerStats ps = go.GetComponent<PlayerStats>();
        go.GetComponent<ThirdPersonUserControl>().CheckGround();
        ps.InstantiateCoffin();
        
    }
}
