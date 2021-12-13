using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ShortCut : MonoBehaviour
{
    public GameObject otherSide;
    private Transform destiny;
    private NavMeshAgent nav;
    public bool activeInter = true;

    // Start is called before the first frame update
    void Start()
    {
        destiny = otherSide.transform.Find("Destiny");
    }

    public void QuickTravel (GameObject traveller)
    {
        Debug.Log(traveller.name);
        if (activeInter == true)
        {
            Debug.Log(activeInter + "/" + traveller.GetComponent<PlayerStats>().smallSize);
            if (traveller.GetComponent<PlayerStats>().smallSize == true)
            {

                float distance = Vector3.Distance(traveller.transform.position, transform.position);
                Debug.Log(distance);
                if (distance <= 5)
                {
                    nav = traveller.GetComponent<UnityEngine.AI.NavMeshAgent>();
                    nav.isStopped = true;
                    nav.enabled = false;
                    traveller.transform.position = destiny.position;
                    nav.enabled = true;
                    nav.isStopped = false;
                    


                    transform.Find("Effect").gameObject.SetActive(false);
                    activeInter = false;
                    otherSide.GetComponent<ShortCut>().activeInter = false;
                    otherSide.transform.Find("Effect").gameObject.SetActive(false);

                    Invoke("Reactivate", 60);
                }
            }
        }
        else
        {
      //      PixelCrushers.DialogueSystem.DialogueManager.ShowAlert("Inactive")
        }

    }

    private void Reactivate ()
    {
        activeInter = true;
        transform.Find("Effect").gameObject.SetActive(true);
        otherSide.GetComponent<ShortCut>().activeInter = true;
        otherSide.transform.Find("Effect").gameObject.SetActive(true);
    }


}
