using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnObject : MonoBehaviour
{

    private GameController gc;

    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.name);
        if ( other.tag == "Player")
        {
            PlayerStats ps = other.gameObject.GetComponent<PlayerStats>();
            if (ps.timerBurning > 1)
            {
                ps.timerBurning = 0;
                GameObject go = other.gameObject;
                go.GetComponent<PlayerStats>().AddjustHealth(-1, gameObject, false);

                
                if (other.gameObject.transform.Find ("Attention") != null)
                {
                    GameObject attention = other.gameObject.transform.Find("Attention").gameObject;
                    if (attention.activeSelf != true)
                    {
                        attention.SetActive(true);
                    }
                    
                    attention.GetComponent<DestroyObj_timer>().timer = 2;
                }
                else
                {
                    GameObject effectGO = Instantiate(Resources.Load("Effects/Attention"), go.transform.position, go.transform.rotation) as GameObject;
                    effectGO.transform.parent = go.transform;
                    effectGO.GetComponent<DestroyObj_timer>().timer = 2;
                    effectGO.SetActive(true);

                }
            }
            else
            {
                ps.timerBurning = ps.timerBurning + Time.deltaTime;
            }
            
        }
        else if (other.tag == "Enemy")
        {
            PlayerStats ps = other.gameObject.GetComponent<PlayerStats>();
            if (ps.timerBurning > 1)
            {
                ps.timerBurning = 0;
                GameObject go = other.gameObject;
                go.GetComponent<PlayerStats>().AddjustHealth(-1, gameObject, false);
                foreach (GameObject player in gc.players)
                {
                    player.GetComponent<PlayerStats>().AddjustExp(10);
                }                
            }
            else
            {
                ps.timerBurning = ps.timerBurning + Time.deltaTime;
            }

        }
    }
}
