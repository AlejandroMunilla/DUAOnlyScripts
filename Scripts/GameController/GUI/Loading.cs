using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    private GUISkin mySkin;
    private GUIStyle otherStyle;
    private Texture2D bigBoard;
    private Texture2D background;
    private Texture2D imagePic;
    private string textDisplay = "";
    private float fontSize = 0.045f;

    private void Start ()
    {
        mySkin = (GUISkin)(Resources.Load("GUI/JRPG2 Dialogue GUI Skin 1"));
        bigBoard = (Texture2D)(Resources.Load("GUI/Simple board"));
        background = (Texture2D)(Resources.Load("GUI/Black"));
        imagePic = (Texture2D)(Resources.Load("GUI/loading/Samael2"));
        otherStyle = mySkin.GetStyle("label");
        otherStyle.fontSize = (int)(Screen.height * fontSize);
        otherStyle.normal.textColor = Color.white;
        otherStyle.wordWrap = true;
        string language = PixelCrushers.DialogueSystem.DialogueLua.GetVariable("language").asString;
        Debug.Log(language);
        textDisplay = PixelCrushers.DialogueSystem.DialogueLua.GetActorField("Dictionary", "loading " + language).asString;
        Debug.Log(textDisplay);

    }

    public void ShowLoadingScreen()
    {
        otherStyle.fontSize = (int)(Screen.height * fontSize);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);
        GUI.DrawTexture(new Rect(Screen.width * 0.25f, Screen.height * 0.05f, Screen.height * 0.65f, Screen.height * 0.66f), imagePic);
        GUI.Label(new Rect(Screen.width * 0.33f, Screen.height * 0.80f, Screen.width * 0.33f, Screen.height * 0.18f), textDisplay, otherStyle);

    }

    
}
