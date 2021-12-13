using PixelCrushers.DialogueSystem;
using UnityEngine;

public class AddBook : MonoBehaviour
{
    GameController gc;
    LoadGameImp loadgame;
    private int exp = 50;
    // Start is called before the first frame update
    void Start()
    {
        GameObject gcon = GameObject.FindGameObjectWithTag("GameController");
        gc = gcon.GetComponent<GameController>();
        loadgame = gcon.GetComponent<LoadGameImp>();
        InvokeRepeating("CheckDone", 0.01f, 0.02f);
    }

    public void NewBook ()
    {
        int newSlot = gc.books.Count;
        gc.books.Add(new InventoryRPG(gameObject.name, 1, newSlot));
        DialogueLua.SetActorField("Books", gameObject.name, "Yes");
        gameObject.SetActive(false);
        string lan = DialogueLua.GetVariable("language").asString;
        string book = DialogueLua.GetActorField("Books", gameObject.name + " " + lan).asString;
        string addedToBook = "addedToBook " + lan;
        string dictionary = DialogueLua.GetActorField("Dictionary", addedToBook).asString;
        DialogueManager.ShowAlert(book + " " + dictionary + ". +" + exp + " exp");
        loadgame.LoadBooks();
        foreach (GameObject go in gc.players)
        {
            go.GetComponent<PlayerStats>().AddjustExp(exp);
            go.GetComponent<PlayerStats>().Invoke("DelayedGetExperience", 0.1f);
            
        }

    }

    private void CheckDone ()
    {
        if (gc.player1 != null)
        {
            string alreadyTaken = DialogueLua.GetActorField("Books", gameObject.name).asString;
            if (alreadyTaken == "Yes")
            {
                gameObject.SetActive(false);
            }
            CancelInvoke("CheckDone");
        }
        
    }


}
