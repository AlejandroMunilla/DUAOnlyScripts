using PixelCrushers.DialogueSystem;
using UnityEngine;

public class CrossInn : MonoBehaviour
{

    void Start()
    {
        Invoke("WaitForLua", 2);
    }


    private void WaitForLua ()
    {
        string profile = DialogueLua.GetVariable("profile").asString;

        if (profile == "")
        {
            Debug.Log("no profile");
            Invoke("WaitForLua", 0.1f);
        }
        else
        {
    //        Debug.Log("Lua!");
            SetUpScene();
        }
    }

    private void SetUpScene ()
    {
        if (DialogueLua.GetActorField("Player2", "chosen").asString == "Rose" || DialogueLua.GetActorField("Player3", "chosen").asString == "Rose")
        {
            transform.Find("Rose").gameObject.SetActive(false);
        }

        if (DialogueLua.GetActorField("Player2", "chosen").asString == "Fred" || DialogueLua.GetActorField("Player3", "chosen").asString == "Fred")
        {
            transform.Find("Fred").gameObject.SetActive(false);
        }

        if (DialogueLua.GetActorField("Player2", "chosen").asString == "Oleg" || DialogueLua.GetActorField("Player3", "chosen").asString == "Oleg")
        {
            transform.Find("Oleg").gameObject.SetActive(false);
        }

        DialogueLua.SetActorField("Peter_Connolly", "introCanola", "No");
   //     DialogueManager.StartConversation("TheCrossInn");
    }



}
