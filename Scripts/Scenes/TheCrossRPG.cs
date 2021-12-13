using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class TheCrossRPG : MonoBehaviour
{
    public GameObject smugglerPlace;
    public GameObject smugglerGui;
    private bool alive = true;
    private GameController gc;
    private Transform footprints;
    private int internalCNT = 1;
    private GameObject fred = null;
    public GameObject doorSmuggler;
    private enum State
    {
        Idle,
        CheckQuest,
        CheckFred,
        CheckFootPrint,
        Battle
    }
    private State state;


    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        footprints = transform.Find("FootPrints");
        StartCoroutine("FSM");
        state = State.Idle;
    }

    private IEnumerator FSM ()
    {
        while (alive)
        {
            switch (state)
            {
                case State.Idle:
                    Idle();
                    yield return new WaitForSeconds(1);
                    break;

                case State.CheckQuest:
                    CheckQuests();
                    yield return new WaitForSeconds(1);
                    break;

                case State.CheckFred:
                    CheckFred();
                    yield return new WaitForSeconds(1);
                    break;

                case State.CheckFootPrint:
                    CheckFootPrint();
                    yield return new WaitForSeconds(0.3f);
                    break;
            }
        }
        yield return null;
    }


    private void Idle ()
    {
        if (gc.allDone == true)
        {
            state = State.CheckQuest;

        }
    }

    private void CheckQuests ()
    {
        string canolaActive = DialogueLua.GetQuestField("Canola", "State").asString;
        string talkActive = DialogueLua.GetQuestField("Canola", "Entry_1_State").AsString;
        string seekActive = DialogueLua.GetQuestField("Canola", "Entry_2_State").AsString;
        Debug.Log(canolaActive + "/" + talkActive + "/" +  seekActive);
   //     Debug.Log(canolaActive);
        if (canolaActive == "active")
        {
            if (seekActive == "active")
            {
                state = State.CheckFred;
            }

            else if (seekActive == "success")
            {
                doorSmuggler.SetActive(true);
                smugglerGui.SetActive(true);
                smugglerPlace.transform.Find("Normal").gameObject.SetActive(false);
                StopCoroutine("FSM");
            }

            else if (seekActive == "failure")
            {
                StopCoroutine("FSM");
            }
        }
        else if (canolaActive == "success" || canolaActive == "done")
        {
            StopCoroutine("FSM");
        }

      
    }

    private void CheckFred ()
    {
        foreach (GameObject go in gc.players)
        {
            if (go.name == "Fred")
            {
                fred = go;
            }
        }
        Debug.Log(fred.name);
        if (fred != null)
        {
            state = State.CheckFootPrint;
        }        
    }

    private void CheckFootPrint ()
    {
        Transform footPrint = footprints.transform.Find(internalCNT.ToString());
        float distance = Vector3.Distance(footPrint.position, fred.transform.position);
   //     Debug.Log(distance);
        if (distance <= 4)
        {
            footprints.transform.Find(internalCNT.ToString()).gameObject.SetActive(true);
            internalCNT++;            
            if (internalCNT >= 70)
            {
                doorSmuggler.SetActive(true);
                smugglerGui.SetActive(true);
                smugglerPlace.transform.Find("Normal").gameObject.SetActive(false);
                DialogueLua.SetQuestField("Canola", "Entry_2_State", "success");
                StopCoroutine("FSM");
            }
        }
    }
}
