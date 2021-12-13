using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class SetForge : MonoBehaviour
{

    public bool reInitialize = false;
    public bool setOnFire = false;
    [SerializeField] int minDam= 1;
    [SerializeField] int maxDam = 10;
    [SerializeField] int addDam = 3;
    [SerializeField] float range = 4;

    private GameController gc;
    private GameObject explosion;
    private Collider col;
    private bool armed = true;
    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        explosion = transform.Find("Fire").gameObject;
        col = transform.Find ("Forge").gameObject.GetComponent<BoxCollider>();
     //   explosion.transform.parent = null;

    }



    public void TriggerExplosion ()
    {
        if (armed == true)
        {
            armed = false;
            explosion.SetActive(true);
            transform.Find("Effect").gameObject.SetActive(false);
            //    gameObject.SetActive(false);
            foreach (GameObject go in gc.enemies)
            {
                CheckDamage(go);
            }

            foreach (GameObject go in gc.players)
            {
                CheckDamage(go);
            }
            Invoke("ReArmForge", 15);
        }
        else
        {
            DialogueManager.ShowAlert("The forge needs time to accumulate heat again");
        }


        
 
    }

    private void CheckDamage (GameObject go)
    {
        float distToEnemie = Vector3.Distance(go.transform.position, transform.position);

        if (col.bounds.Contains(go.transform.position))
        {
            Debug.Log(go.name);
            int randomDam = Random.Range(minDam, maxDam) + addDam;
            go.GetComponent<PlayerStats>().AddjustHealth(-randomDam, gameObject, true);
            go.GetComponent<PlayerStats>().AddjustRegen(-randomDam, gameObject, false);

            if (setOnFire == true)
            {
                if (go.transform.Find("Fire") == null)
                {
                    GameObject fireTemp = Instantiate(Resources.Load("Effects/Fire"), go.transform.position, go.transform.rotation) as GameObject;
                    fireTemp.name = "Fire";
                    fireTemp.transform.parent = go.transform;
                    fireTemp.SetActive(true);
                }
                else
                {
                    go.transform.Find("Fire").gameObject.SetActive(true);
                }


            }
        }
    }

    private void ReArmForge ()
    {
        armed = true;
    }

}
