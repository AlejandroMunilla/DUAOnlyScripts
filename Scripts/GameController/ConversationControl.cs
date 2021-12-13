using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;
using PixelCrushers.DialogueSystem.Wrappers;

public class ConversationControl : MonoBehaviour
{

    private myGUI mGUI;
    private GameController gc;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("GameController");
        mGUI = go.GetComponent<myGUI>();
        gc = go.GetComponent<GameController>();
    }

    private void OnConversationStart(Transform actor)
    {

     //   Debug.Log(actor.name);
        mGUI.enabled = false;
        gc.inConversation = true;
        if (gc.isRPG == true)
        {
            if (gc.activePlayer == null)
            {
                gc.activePlayer = gc.player1;
            }
            player = gc.activePlayer;
            Animator anim = player.GetComponent<Animator>();
            anim.SetTrigger("Idle");
            anim.SetFloat("Forward", 0);
            anim.SetFloat("Turn", 0);
            if (gc.player1.name == "Nanna")
            {
                DialogueManager.SetPortrait("Player1", "Portraits/Nanna");
            }
            /*
            if (gc.activePlayer == null)
            {
                gc.activePlayer = gc.player1;
            }
            player = gc.activePlayer;
            player.GetComponent<ThirdPersonUserControl>().enabled = false;
            player.GetComponent<ThirdPersonCharacter>().enabled = false;
         
  
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            Camera cam = player.GetComponent<ThirdPersonUserControl>().cam;
            cam.GetComponent<MouseOrbitImproved>().enabled = false;


            */
        }
    }

    private void OnConversationEnd (Transform actor)
    {
        Invoke("DelayConversationEnd", 0.6f);
        /*
  //      Debug.Log("End");
        mGUI.enabled = true;
        gc.inConversation = false;
        gc.activePlayer.GetComponent<ThirdPersonUserControl>().enabled = true;
        gc.activePlayer.GetComponent<ThirdPersonCharacter>().enabled = true;
        player.GetComponent<ThirdPersonUserControl>().cam.GetComponent<MouseOrbitImproved>().enabled = true;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        player.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;*/
    }

    private void DelayConversationEnd ()
    {
        gc.inConversation = false;
    }
}
