using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;

public class TranslateGUIText : MonoBehaviour
{
    GameController gc;
    // Start is called before the first frame update
    void OnEnable ()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        InvokeRepeating("WaitForLua", 0.1f, 0.02f);
        

    }

    private void WaitForLua ()
    {
        if (gc.player1 != null)
        {
            CancelInvoke("WaitForLua");
            Translate();
        }
    }

    private void Translate ()
    {
        string lan = DialogueLua.GetVariable("language").asString;
        Debug.Log(lan);
        string currentText = gameObject.name;

        string varLua = currentText + " " + lan;
        Debug.Log(currentText + "/" + varLua);
        string translation = DialogueLua.GetActorField("Dictionary", varLua).asString;
        Debug.Log(translation);
        GetComponent<Text>().text = translation;
    }


}
