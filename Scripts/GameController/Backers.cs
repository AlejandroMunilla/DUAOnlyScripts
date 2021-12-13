using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Backers : MonoBehaviour
{
    private MainMenu mainMenu;
    private List<string> firstBackers = new List<string>();
    private string value;
    private int textSize;
    private Rect bookListRect;
    private Rect bookRect;
    private Vector2 bookRectSlider;
    private int posX;
    private int posY;
    private int bookWidth;
    private int bookHeight;
    void OnEnable ()
    {
        mainMenu = GetComponent<MainMenu>();
        bookRect = new Rect(0, 0, Screen.width * 0.90f, 1000);
        bookRect = new Rect(0, 0, Screen.width * 0.90f, 1000);
        posX = (int)(Screen.width * 0.05f);
        posY = (int)(Screen.height * 0.30f);
        bookWidth = (int)(Screen.width * 0.90f);
        bookHeight = (int)(Screen.height * 0.68f);
        textSize = (int)(Screen.width * 0.02f);
        GetBackers();

        if (mainMenu.hName != null)
        {
            Invoke("XboxController", 1);
        }
    }

    private void OnDisable()
    {
        CancelInvoke("XboxController");
    }


    private void GetBackers()
    {
        TextAsset textAsset = (TextAsset)(Resources.Load("Text/BackersText", typeof(TextAsset)));               
        value = textAsset.text;
        Debug.Log(value);
    }



    private void OnGUI()
    {
        mainMenu.Background();

        if (GUI.Button(new Rect(Screen.width * 0.02f, Screen.height * 0.2f, Screen.width * 0.2f, Screen.height * 0.08f), "BACK", mainMenu.styleButton))
        {
            DisableThis();
        }

        bookRectSlider = GUI.BeginScrollView(new Rect(posX, posY, bookWidth, Screen.height * 0.8f), bookRectSlider,
                                                          bookRect);
        GUIContent content = new GUIContent(value);
        GUIStyle style = GUI.skin.label;
        GUI.skin.label.fontSize = textSize;
        
        float textDimensions = GUI.skin.label.CalcHeight(new GUIContent(value), Screen.width * 0.7f);

        GUI.Label(new Rect(Screen.width * 0.02f, Screen.height * 0.01f, Screen.width * 0.7f, Screen.height * 1.5f), value);
        bookRect = new Rect(0, 0, Screen.width  * 0.8f, textDimensions + posY);
        /*
        for (int cnt = 0; cnt < arrayString.Length ; cnt++)
        {
            GUI.Label (new Rect(1, 20 + (cnt*25), 600 , buttonHeight), arrayString[cnt]); 
        }*/
        GUI.EndScrollView();
    }

    private void XboxController ()
    {
        if (Input.GetKeyUp("joystick 1 button 0"))
        {
            DisableThis();
        }
        else
        {
            Invoke("XboxController", 0);
        }
    }

    private void DisableThis ()
    {
        this.enabled = false;
        mainMenu.Invoke("XboxController2", 1);
        mainMenu.enabled = true;
    }
}
