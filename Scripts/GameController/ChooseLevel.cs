using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityStandardAssets.CrossPlatformInput;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChooseLevel : MonoBehaviour
{
    private bool axisXBack = true;
    private bool levelIsOpen = false;
    private string infoText = "INFO";
    private string readyText = "READY!";
    private string backText = "BACK TO MENU";
    private string language;
    private string control = "nil";
    private int levelChosen = 0;
    private float timer = 0;
    private Texture2D background;
    private GUIStyle myStyle;
    private GUIStyle styleButton;
    private GUISkin mySkin;
    private Texture2D level1;
   
    private Texture2D activeButton;
    private Rect activeButtonRect;
    private Rect level1Rect;
    private Rect level2Rect;
    private Rect level3Rect;
    private Rect level4Rect;
    private Rect level5Rect;
    private Rect goRect;
    private Rect backRect;


    private void Awake()
    {
        // Subscribe to events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnect;
        foreach (Player p in ReInput.players.Players)
        {
            p.controllers.hasKeyboard = false;
            p.controllers.hasMouse = false;

        }
   //     Debug.Log(ReInput.controllers.Joysticks.Count);
        // Assign each Joystick to a Player initially

        
        foreach (Rewired.Joystick j in ReInput.controllers.Joysticks)
        {
            if (ReInput.controllers.IsJoystickAssigned(j)) continue; // Joystick is already assigned

            // Assign Joystick to first Player that doesn't have any assigned
            AssignJoystickToNextOpenPlayer(j);
            Debug.Log(j.name );
        }
    }


    // This function will be called when a controller is connected
    // You can get information about the controller that was connected via the args parameter
    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was connected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
        AssignJoystickToNextOpenPlayer(ReInput.controllers.GetJoystick(args.controllerId));
    }

    // This function will be called when a controller is fully disconnected
    // You can get information about the controller that was disconnected via the args parameter
    void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
    }

    // This function will be called when a controller is about to be disconnected
    // You can get information about the controller that is being disconnected via the args parameter
    // You can use this event to save the controller's maps before it's disconnected
    void OnControllerPreDisconnect(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller is being disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
    }



    void AssignJoystickToNextOpenPlayer(Rewired.Joystick j)
    {
        foreach (Player p in ReInput.players.Players)
        {
            if (p.controllers.joystickCount > 0) continue; // player already has a joystick
            p.controllers.AddController(j, true); // assign joystick to player

            Debug.Log(p.name + "/" + j.name);
            return;
        }

    }


    void Start()
    {
        background = (Texture2D)(Resources.Load("GUI/background"));
        level1 = (Texture2D)(Resources.Load("Textures/TheCross"));
        activeButton = (Texture2D)(Resources.Load("GUI/ActiveButton"));
        mySkin = (GUISkin)(Resources.Load("GUI/JRPG2 Dialogue GUI Skin 1"));
        myStyle = mySkin.GetStyle("label");
        myStyle.fontSize = (int)(Screen.height * 0.024f);
        myStyle.normal.textColor = Color.white;
        myStyle.alignment = TextAnchor.UpperLeft;
        myStyle.wordWrap = true;

        styleButton = mySkin.GetStyle("button");
        styleButton.fontSize = (int)(Screen.height * 0.022f);
        styleButton.normal.textColor = Color.white;
        styleButton.alignment = TextAnchor.MiddleCenter;
        

        level1Rect = new Rect(0, 0, Screen.width * 0.2f, Screen.width * 0.2f);
        level2Rect = new Rect(Screen.width * 0.2f, 0, Screen.width * 0.2f, Screen.width * 0.2f);
        level3Rect = new Rect(Screen.width * 0.4f, 0,  Screen.width * 0.2f, Screen.width * 0.2f);
        level4Rect = new Rect(Screen.width * 0.6f, 0,  Screen.width * 0.2f, Screen.width * 0.2f);
        level5Rect = new Rect(Screen.width * 0.8f, 0,  Screen.width * 0.2f, Screen.width * 0.2f);
        goRect = new Rect(Screen.width * 0.1f, Screen.height * 0.9f, Screen.width * 0.4f, Screen.height * 0.1f);
        backRect = new Rect(Screen.width * 0.5f, Screen.height * 0.9f, Screen.width * 0.4f, Screen.height * 0.1f);

        activeButtonRect = level1Rect;
        levelChosen = 1;

        Invoke("WaitForLua", 0);

    }

    private void WaitForLua ()
    {
     //   Debug.Log(DialogueLua.GetVariable("language").AsString);
        language = DialogueLua.GetVariable("language").AsString;
        if (language == "es" || language == "en" || language == "fr")
        {
      //      Debug.Log(DialogueLua.GetVariable("profile").AsString);
            
            levelIsOpen = true;
            levelChosen = 1;
            CheckLevelLocked(1);
            ChangeLanguage("The Cross");
            control = DialogueLua.GetActorField("Player1", "control").AsString;
       //     Debug.Log(Localization.language + "/" + language);
            Debug.Log(control);
        }
        else
        {
            Invoke("WaitForLua", 0);
        }

        /*
        if (DialogueLua.GetVariable ("profile").AsString == "" || DialogueLua.GetVariable("profile").AsString == "nil")
        {
            Invoke("WaitForLua", 0.1f);
            Debug.Log(DialogueLua.GetVariable("profile").AsString);
        }
        else
        {
            Debug.Log(DialogueLua.GetVariable("profile").AsString);
            Debug.Log(DialogueLua.GetVariable("language").asString);
            language = DialogueLua.GetVariable("language").asString;
            
            if (Localization.language == "" && language == "nil")
            {
                string tempLan = "en";
                language = tempLan;
                Localization.language = tempLan;
                DialogueLua.SetVariable("language", tempLan);
                ChangeLanguage("The Cross");
                levelIsOpen = true;
                control = DialogueLua.GetActorField("Player1", "control").AsString;
                Debug.Log(Localization.language + "/" + language);
                Debug.Log(control);
                if (control == "" || control == "nil")
                {
                    control = "joystick 1";
                }
            }
            else if ( language  != "nil")
            {
                Localization.language = language;
                Debug.Log(Localization.language + "/" + language);
            }
            else
            {
                language = Localization.language;
                Debug.Log(Localization.language + "/" + language);
            }

            
        //    Debug.Log(language + "/" + Localization.language);
          

        }*/
    }
    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);
        if (GUI.Button(level1Rect, new GUIContent("", level1), styleButton))
        {
            levelChosen = 1; 
            CheckLevelLocked(1);
            activeButtonRect = level1Rect;
        }
        if (GUI.Button(level2Rect, new GUIContent("", background), styleButton))
        {
            levelChosen = 2;
            CheckLevelLocked(2);
            activeButtonRect = level2Rect;
        }
        if (GUI.Button(level3Rect, new GUIContent("", background), styleButton))
        {
            levelChosen = 3;
            CheckLevelLocked(3);
            activeButtonRect = level3Rect;
        }
        if (GUI.Button(level4Rect, new GUIContent("", background), styleButton))
        {
            levelChosen = 4;
            CheckLevelLocked(4);
            activeButtonRect = level4Rect;
        }
        if (GUI.Button(level5Rect, new GUIContent("", background), styleButton))
        {
            levelChosen = 5;
            CheckLevelLocked(5);
            activeButtonRect = level5Rect;
        }
        GUI.Label (new Rect(Screen.width * 0.05f, Screen.height * 0.35f, Screen.width * 0.9f, Screen.height * 0.45f), infoText, myStyle);
        
        if (levelIsOpen)
        {
           if (GUI.Button(goRect, readyText, styleButton))
           {
                SceneManager.LoadScene(levelChosen.ToString());
            }
        }
        if (GUI.Button(backRect, backText, styleButton))
        {
            SceneManager.LoadScene ("MainMenu");
        }
        
        GUI.DrawTexture(activeButtonRect, activeButton);

    }

    private void Update()
    {
        for (int cnt5 = 0; cnt5 < ReInput.players.Players.Count; cnt5++)
        {

            if (ReInput.players.Players[cnt5].GetAxis("Move Horizontal") > 0.9f)
            {

            }

            if (ReInput.players.Players[cnt5].GetButtonUp("Fire"))
            {
                levelChosen = 1;
                CheckLevelLocked(1);
                activeButtonRect = level1Rect;
                SceneManager.LoadScene(levelChosen.ToString());

            }
        }

        


        /*
        if (Input.GetKeyUp(control +" button 0"))
        {
            if (activeButtonRect == goRect)
            {
                Debug.Log(levelChosen);
                SceneManager.LoadScene(levelChosen.ToString());

            }
            else if (activeButtonRect == level1Rect)
            {
                activeButtonRect = goRect;
                levelChosen = 1;
            }
            else if (activeButtonRect == backRect)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
        else if (CrossPlatformInputManager.GetAxis(control + "_X") > 0.9f && axisXBack == true)
        {
            axisXBack = false;

            if (activeButtonRect == level1Rect)
            {
                activeButtonRect = level2Rect;
                CheckLevelLocked(2);
            }
            else if (activeButtonRect == level2Rect)
            {
                activeButtonRect = level3Rect;
                CheckLevelLocked(3);
            }
            else if (activeButtonRect == level3Rect)
            {
                activeButtonRect = level4Rect;
                CheckLevelLocked(4);
            }
            else if (activeButtonRect == level4Rect)
            {
                activeButtonRect = level5Rect;
                CheckLevelLocked(5);
            }
            else if (activeButtonRect == level5Rect)
            {
                if (levelIsOpen == true)
                {
                    activeButtonRect = goRect;
                }
                else
                {
                    activeButtonRect = level1Rect;
                    activeButtonRect = level1Rect;
                    ChangeLanguage("The Cross");
                    levelIsOpen = true;
                }
                

            }
            else if (activeButtonRect == goRect)
            {
                activeButtonRect = backRect;

            }
            else if (activeButtonRect == backRect)
            {
                activeButtonRect = level1Rect;
                ChangeLanguage("The Cross");
                levelIsOpen = true;
            }

        }
        else if (CrossPlatformInputManager.GetAxis("joystick 1_X") < 0.3f && CrossPlatformInputManager.GetAxis("joystick 1_X") > -0.3f)
        {
            axisXBack = true;
        }
        else if (CrossPlatformInputManager.GetAxis(control + "_X") < -0.9f && axisXBack == true)
        {
            axisXBack = false;

            if (activeButtonRect == level1Rect)
            {
                activeButtonRect = backRect;
        //        CheckLevelLocked(2);
            }
            else if (activeButtonRect == backRect)
            {
                activeButtonRect = goRect;
        //        CheckLevelLocked(3);
            }
            else if (activeButtonRect == goRect)
            {
                activeButtonRect = level5Rect;
                CheckLevelLocked(5);
            }
            else if (activeButtonRect == level5Rect)
            {
                activeButtonRect = level4Rect;
                CheckLevelLocked(4);
            }
            else if (activeButtonRect == level4Rect)
            {
                activeButtonRect = level3Rect;
                CheckLevelLocked(3);
            }
            else if (activeButtonRect == level3Rect)
            {
                activeButtonRect = level2Rect;

            }
            else if (activeButtonRect == level2Rect)
            {
                activeButtonRect = level1Rect;
                ChangeLanguage("The Cross");
                levelIsOpen = true;
            }

        }*/
    }


    private void CheckLevelLocked (int levelScene)
    {
        string levelOpen = DialogueLua.GetActorField("Level" + levelScene.ToString(), "open").AsString;
     //   Debug.Log(levelOpen);
        if (levelScene == 1)
        {
            ChangeLanguage("The Cross");
            levelIsOpen = true;
        }
        else
        {
            if (levelOpen != "Yes")
            {
                ChangeLanguage("Locked");
                levelIsOpen = false;
            }
            else
            {

            }
        }
    }

    private void ChangeLanguage(string textPath)
    {
        if (textPath == "Locked")
        {
            if (language == "en")
            {
                infoText = "This level is locked. You must fight through the previous levels";
            }
            else if (language == "es")
            {
                infoText = "No podeís accerder a este nivel. Debeís batiros en los niveles previos";
            }
            else if (language == "fr")
            {
                infoText = "Ce niveau est verrouillé. Vous devez vous battre à travers les niveaux précédents";
            }
        }
        else
        {
    //        Debug.Log("Text/Intro_" + textPath + "_" + language);
            TextAsset textAsset = (TextAsset)(Resources.Load("Text/Intro_" + textPath + "_" + language, typeof(TextAsset)));
            infoText = textAsset.text;
        }

        if (language == "en")
        {
            readyText = "READY!";
            backText = "BACK TO MENU";
        }
        else if (language == "es")
        {
            readyText = "CONTINUAD!";
            backText = "VOLVER AL MENU";
        }
        else if (language == "fr")
        {
            readyText = "PRÊT!";
            backText = "RETOUR AU MENU";
        }

    }




}
