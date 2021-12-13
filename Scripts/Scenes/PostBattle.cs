using System.Collections;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using UnityEngine;

public class PostBattle : MonoBehaviour
{
    public Texture2D background;
    public Texture2D logo;
    public GUIStyle myStyle;
    public GUIStyle styleButton;
    public GUISkin mySkin;
    public GUISkin smallSkin;

    private bool showStats = false;
    private string namePC = "Hero";
    private string expString = "Experience";
    private string coinsString = "Coins";
    private string deathString = "Death blows";
    private string reviveString = "Revives";
    private string fallNoString = "Fell down";
    private string headShotString = "Head Shots";
    

    private GUIStyle labelStyle;
    private GUIStyle smallLabel;
    private LoadGame loadGame;
    private List<string> players = new List<string>();
    

    private Texture2D overlay;
    // Start is called before the first frame update
    void Start()
    {
        background = (Texture2D)(Resources.Load("GUI/background"));
        logo = (Texture2D)(Resources.Load("GUI/Logo"));
        overlay = (Texture2D)(Resources.Load("GUI/ActiveButton"));
        myStyle = mySkin.GetStyle("label");
        myStyle.fontSize = (int)(Screen.height * 0.045f);
        myStyle.normal.textColor = Color.white;
        myStyle.alignment = TextAnchor.MiddleCenter;

        styleButton = mySkin.GetStyle("button");
        styleButton.fontSize = (int)(Screen.height * 0.022f);
        styleButton.normal.textColor = Color.white;
        styleButton.alignment = TextAnchor.MiddleCenter;

        labelStyle = mySkin.GetStyle("Text Area");
        labelStyle.fontSize = (int)(Screen.height * 0.022f);
        labelStyle.normal.textColor = Color.white;
        labelStyle.alignment = TextAnchor.MiddleLeft;


        loadGame = GetComponent<LoadGame>();
        Invoke("CheckLoadedData", 0.01f);
    }

    private void OnGUI()
    {
        Background();

        if (showStats == true)
        {
            DisplayStats();
        }
        else
        {
            ShowMessage();
        }
        
    }

    private void Background()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);
        GUI.DrawTexture(new Rect(Screen.width * 0.01f, Screen.height * 0.02f, Screen.width * 0.8f, Screen.width * 0.13f), logo);

    }

    private void DisplayStats ()
    {
        GUI.Label(new Rect(Screen.width * 0.3f, Screen.height * 0.01f, Screen.width * 0.4f, Screen.height * 0.08f), "POSTBATTLE", myStyle);

        GUI.Label(new Rect(Screen.width * 0.1f, Screen.height * 0.10f, Screen.width * 0.4f, Screen.width * 0.08f), namePC, labelStyle);


        for (int cnt = 0; cnt < players.Count; cnt ++)
        {
            GUI.Label(new Rect(Screen.width * 0.1f, Screen.height * 0.20f + (cnt * Screen.height * 0.2f), Screen.width * 0.4f, Screen.width * 0.08f), namePC, labelStyle);

        }
    }

    private void ShowMessage ()
    {
        GUI.Label(new Rect(Screen.width * 0.3f, Screen.height * 0.01f, Screen.width * 0.4f, Screen.width * 0.12f), "LOADING DATA", myStyle);

    }

    private void CheckLoadedData ()
    {
        if (loadGame.questLoaded == false)
        {
            Invoke("CheckLoadedData", 0.01f);
        }
        else
        {
            Invoke("ChangeBool", 0.01f);
        }
    }

    private void ChangeBool ()
    {
        showStats = true;
    }

    private void GetData ()
    {
        /*
        string chosen1 = DialogueLua.GetActorField("Player1", "chosen").asString;
        string chosen2 = DialogueLua.GetActorField("Player2", "chosen").asString;
        string chosen3 = DialogueLua.GetActorField("Player3", "chosen").asString;
        string chosen4 = DialogueLua.GetActorField("Player4", "chosen").asString;*/

        string chosen1 = "Rose";
        string chosen2 = "Balder";
        string chosen3 = "Fred";
        string chosen4 = "Oleg";

        players.Clear();
        if (chosen1 != "None")
        {
            players.Add(chosen1);
        }

        if (chosen2 != "None")
        {
            players.Add(chosen2);
        }

        if (chosen3 != "None")
        {
            players.Add(chosen3);
        }

        if (chosen4 != "None")
        {
            players.Add(chosen4);
        }

        foreach (string st in players)
        {
            Debug.Log(st);
        }

    }

}
