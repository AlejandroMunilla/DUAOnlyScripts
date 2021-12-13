using PixelCrushers.DialogueSystem;
using UnityEngine;

public class SecretVisible : MonoBehaviour
{
    private GameController gc;
    private int successBase = 0;
    private string textLocalized;
    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        InvokeRepeating("Search", 1, 3);
    }


    private void Search ()
    {
       
        foreach (GameObject go in gc.players)
        {
            float distance = Vector3.Distance(go.transform.position, transform.position);
     //       Debug.Log(distance);
            if (distance < 3)
            {
                successBase++;
                if (go.name == "Fred")
                {
                    UncoverSecret(go);
                }
                else
                {
                    int diceRoll = DiceRoll();
                    if (diceRoll <= successBase)
                    {
                        UncoverSecret(go);
                    }
                }
            }
            else if (distance < 6 && go.name == "Fred")
            {
                int diceRoll = DiceRoll();
                if (diceRoll <= successBase)
                {
                    UncoverSecret(go);
                }
            }
        }
    }

    private void UncoverSecret (GameObject player)
    {
        GetComponent<BoxCollider>().enabled = true;
        transform.Find("Effect").gameObject.SetActive(true);
        string lan = DialogueLua.GetVariable("language").asString;       
        string localized = "discovered " + lan;
        Debug.Log(localized);
        DialogueManager.ShowAlert(player.name + " " + DialogueLua.GetActorField ("Dictionary", localized).asString);
        gameObject.tag = "Inter";
        CancelInvoke("Search");


    }

    private int DiceRoll ()
    {
        int diceRoll = Random.Range(0, 100);
        return diceRoll;
    }

    public void TriggerUncover ()
    {
        GetComponent<BoxCollider>().enabled = false;
        Transform uncover = transform.Find("Exit");
        uncover.parent = null;
        uncover.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
