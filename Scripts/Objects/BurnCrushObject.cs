using Mirror.Examples.Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnCrushObject : MonoBehaviour
{
    
   
    private int crushingDam = 8;
    private int fireDam = 10;
    private bool isColliding = false;

    /// specific for Polidori batte. 
    public GameObject trigger;
    private BossPolidori bossPolidori;

    // Start is called before the first frame update
    void Start()
    {
        if (trigger != null)
        {
            bossPolidori = trigger.GetComponent<BossPolidori>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Enemy")
        {
            if (other.gameObject.name == "Polidori")
            {
                bossPolidori.chandelierAttack = true;
            }


            if (isColliding == false)
            {
                PlayerStats ps = other.gameObject.GetComponent<PlayerStats>();
                ps.AddjustRegen(-10, gameObject, false);
                ps.AddjustHealth(-10, gameObject, true);
                isColliding = true;
                StartCoroutine(Reset());
            }          
        }
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(2);
        isColliding = false;
    }

}
