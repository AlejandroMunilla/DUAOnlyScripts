using UnityStandardAssets.CrossPlatformInput;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class MenuInGame : MonoBehaviour {


    //MAIN MENU
    public Texture2D background;
    public Texture2D logo;
    private Texture2D englishPic;
    private Texture2D spanishPic;
    private Texture2D frenchPic;

    private string showControl = "main";

    private string english = "English";
    private string french = "French";
    private string spanish = "Spanish";
    private string language = "en";

    public GUIStyle myStyle;
    public GUIStyle styleButton;
    public GUISkin mySkin;
    
    private string profile;
    private string scene;
    private string save = "SAVE GAME";
    private string pause = "GAME PAUSED";
    private string quit = "EXIT";
    private string showAlert = "";
    private string back = "BACK TO GAME";
    private string controlString = "CONTROLS";
    private string helpTip = "IMPORTANT: PRESS BLOCK CLOSE TO A FALLEN PARTY MEMBER TO REVIVE";
    private string backTip = "Press any attack button now to go back to Menu";
    private int buttonHeight;
    private int buttonWidthLoad;
    private int buttonWidth;

    /// <summary>
    /// Specific for consoles controllers without mouse / Keyboard
    /// </summary>
    private bool axisXBack = true;
    private string masterController = "joystick 1";
    private Texture2D overlay;
    private Rect overlayRect;
    private Rect englishFlag;
    private Rect frenchFlag;
    private Rect spanishFlag;
    private Rect saveRect;
    private Rect backRect;
    private Rect exitRect;
    private Rect controlRect;
    private float timer;


    private int textureHeight;
    public Texture2D myTexture;
    private GameController gc;
    private Camera camRPG;
    private Camera mainCam;
    private bool loaded = false;


    void OnEnable ()
    {
        if (loaded == false)
        {
            loaded = true;
            GameObject gcon = GameObject.FindGameObjectWithTag("GameController");
            gc = gcon.GetComponent<GameController>();
            if (gc.isRPG == true)
            {
                showControl = "rpg";
                foreach (Camera ca in Camera.allCameras)
                {
                    Debug.Log(ca.name);
                    if (ca.name == "Camera1")
                    {
                        mainCam = ca;
                    }
                }

                camRPG = gcon.transform.Find("CameraMap").gameObject.GetComponent<Camera>();
            }
            scene = "01MainMenu";
            background = (Texture2D)(Resources.Load("GUI/background"));
            logo = (Texture2D)(Resources.Load("GUI/Logo"));
            englishPic = (Texture2D)(Resources.Load("GUI/Flags/English"));
            frenchPic = (Texture2D)(Resources.Load("GUI/Flags/French"));
            spanishPic = (Texture2D)(Resources.Load("GUI/Flags/Spanish"));
            overlay = (Texture2D)(Resources.Load("GUI/ActiveButton"));
            myTexture = (Texture2D)(Resources.Load("GUI/mapping"));
            //    skelleton = (Texture2D)(Resources.Load("GUI/Skelleton"));
            myStyle = mySkin.GetStyle("label");
            myStyle.fontSize = (int)(Screen.height * 0.045f);
            myStyle.normal.textColor = Color.white;
            myStyle.alignment = TextAnchor.MiddleCenter;

            styleButton = mySkin.GetStyle("button");
            styleButton.fontSize = (int)(Screen.height * 0.022f);
            styleButton.normal.textColor = Color.white;
            styleButton.alignment = TextAnchor.MiddleCenter;

            buttonHeight = (int)(Screen.height * 0.06f);
            buttonWidth = (int)(Screen.width * 0.2f);
            buttonWidthLoad = (int)(Screen.width * 0.4f);

            englishFlag = new Rect(Screen.width * 0.02f, Screen.height * 0.19f, Screen.width * 0.06f, Screen.width * 0.04f);
            spanishFlag = new Rect(Screen.width * 0.07f, Screen.height * 0.19f, Screen.width * 0.06f, Screen.width * 0.04f);
            frenchFlag = new Rect(Screen.width * 0.12f, Screen.height * 0.19f, Screen.width * 0.06f, Screen.width * 0.04f);
            saveRect = new Rect(Screen.width * 0.3f, Screen.height * 0.55f, Screen.width * 0.2f, Screen.height * 0.08f);
            exitRect = new Rect(Screen.width * 0.3f, Screen.height * 0.63f, Screen.width * 0.2f, Screen.height * 0.08f);
            backRect = new Rect(Screen.width * 0.3f, Screen.height * 0.71f, Screen.width * 0.2f, Screen.height * 0.08f);
            controlRect = new Rect(Screen.width * 0.3f, Screen.height * 0.80f, Screen.width * 0.2f, Screen.height * 0.08f);

            overlayRect = backRect;

            textureHeight = (int)(Screen.width * 0.5f);


            ThirdPersonUserControl tpc = GetComponent<GameController>().player1.GetComponent<ThirdPersonUserControl>();
            if (Application.platform == RuntimePlatform.XboxOne || tpc.mobileControl == true)
            {

            }
        }


    }


    private void CallControllers ()
    {
        string[] tempControllers = Input.GetJoystickNames();
        if(tempControllers.Length > 0 || Application.platform == RuntimePlatform.XboxOne)
        {

        }

        if (Application.platform == RuntimePlatform.XboxOne)
        {

        }
    }


    private void OnGUI()
    {
        //    Debug.Log(showControl);



        if (gc.isRPG == false)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);
            GUI.DrawTexture(new Rect(Screen.width * 0.1f, Screen.height * 0.02f, Screen.width * 0.8f, Screen.width * 0.13f), logo);
            GUI.Label(new Rect(Screen.width * 0.1f, Screen.height * 0.15f, Screen.width * 0.8f, Screen.width * 0.12f), pause, myStyle);
            if (GUI.Button(saveRect, save, styleButton))
            {
                showControl = "save";
            }

            if (GUI.Button(exitRect, quit, styleButton))
            {
                SceneManager.LoadScene("MainMenu");
            }
            if (GUI.Button(backRect, back, styleButton))
            {
                GetComponent<GameController>().TogglePause();

            }
            if (GUI.Button(controlRect, controlString, styleButton))
            {
                timer = Time.realtimeSinceStartup + 2;
                showControl = "controls";
            }

            Flags();
            GUI.DrawTexture(overlayRect, overlay);
        }

        else if (showControl == "rpg")
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);
            GUI.Label(new Rect(Screen.width * 0.1f, Screen.height * 0.15f, Screen.width * 0.8f, Screen.width * 0.12f), pause, myStyle);
            if (GUI.Button(saveRect, save, styleButton))
            {
                showControl = "save";
            }

            if (GUI.Button(exitRect, quit, styleButton))
            {
                SceneManager.LoadScene("MainMenu");
            }
            if (GUI.Button(backRect, back, styleButton))
            {
                GetComponent<GameController>().TogglePause();

            }
            if (GUI.Button(controlRect, controlString, styleButton))
            {
                timer = Time.realtimeSinceStartup + 2;
                showControl = "controls";
            }

            GUI.DrawTexture(overlayRect, overlay);
        }
        else if (showControl == "save")
        {

        }
        else if (showControl == "controls")
        {
            Debug.Log("control");
            GUI.DrawTexture(new Rect(0, 0, Screen.width, textureHeight), myTexture);
            GUI.Label(new Rect(Screen.width * 0.25f, textureHeight, Screen.width * 0.5f, Screen.height * 0.08f), helpTip);
            GUI.Label(new Rect(Screen.width * 0.25f, textureHeight + (Screen.height * 0.08f), Screen.width * 0.5f, Screen.height * 0.08f), backTip);
        }
        


    }



    private void Update ()
    {
        if (showControl  == "main")
        {
            foreach (Player p in ReInput.players.Players)
            {
                //       Debug.Log(timer + "/" + Time.realtimeSinceStartup);
                if (p.GetButtonUp("Fire") && timer < Time.realtimeSinceStartup)
                {
                    //       showControls = false;
                    if (overlayRect == englishFlag)
                    {
                        ChangeLanguage("en");
                    }
                    else if (overlayRect == spanishFlag)
                    {
                        ChangeLanguage("es");
                    }
                    else if (overlayRect == frenchFlag)
                    {
                        ChangeLanguage("fr");
                    }

                    else if (overlayRect == exitRect)
                    {
                        SceneManager.LoadScene("MainMenu");
                    }
                    else if (overlayRect == backRect)
                    {
                        GetComponent<GameController>().TogglePause();
                    }
                    else if (overlayRect == controlRect)
                    {
                        timer = Time.realtimeSinceStartup + 2;
                        showControl = "controls";
                    }
                }

                if (p.GetAxis("Move Horizontal") < -0.9f && timer < Time.realtimeSinceStartup)
                {
                    axisXBack = false;
                    timer = Time.realtimeSinceStartup + 0.2f;
                    if (overlayRect == backRect)
                    {

                        overlayRect = exitRect;
                    }
                    else if (overlayRect == exitRect)
                    {
                        overlayRect = frenchFlag;

                    }
                    else if (overlayRect == frenchFlag)
                    {
                        overlayRect = spanishFlag;

                    }
                    else if (overlayRect == spanishFlag)
                    {
                        overlayRect = englishFlag;

                    }
                    else if (overlayRect == englishFlag)
                    {
                        overlayRect = controlRect;

                    }
                    else if (overlayRect == controlRect)
                    {
                        overlayRect = backRect;
                    }

                }

                else if (p.GetAxis("Move Horizontal") > 0.9f && timer < Time.realtimeSinceStartup)
                {
                    axisXBack = false;
                    timer = Time.realtimeSinceStartup + 0.2f;
                    if (overlayRect == controlRect)
                    {
                        overlayRect = englishFlag;
                    }
                    else if (overlayRect == englishFlag)
                    {
                        overlayRect = spanishFlag;
                    }
                    else if (overlayRect == spanishFlag)
                    {
                        overlayRect = frenchFlag;
                    }
                    else if (overlayRect == frenchFlag)
                    {
                        overlayRect = exitRect;
                    }
                    else if (overlayRect == exitRect)
                    {
                        overlayRect = backRect;
                    }
                    else if (overlayRect == backRect)
                    {
                        overlayRect = controlRect;
                    }
                }
            }
        }

        else if (showControl == "controls" && timer + 2 < Time.realtimeSinceStartup )
        {
            foreach (Player p in ReInput.players.Players)
            {
                if (p.GetAnyButtonUp())
                {
                    showControl = "main";
                }
            }
        }

   


    
    }


    private void Flags ()
    {
        if (GUI.Button(englishFlag, new GUIContent(englishPic, english), myStyle))
        {
            if (language != "en")
            {
                ChangeLanguage("en");
            }

        }
        if (GUI.Button(spanishFlag, new GUIContent(spanishPic, spanish), myStyle))
        {
            if (language != "es")
            {
                ChangeLanguage("es");
            }

        }
        if (GUI.Button(frenchFlag, new GUIContent(frenchPic, french), myStyle))
        {
            if (language != "fr")
            {
                ChangeLanguage("fr");
            }

        }

    }

    private void ChangeLanguage (string lan)
    {
        english = DialogueLua.GetActorField("Dictionary", "English " + lan).asString;
        french = DialogueLua.GetActorField("Dictionary", "English " + lan).asString;
        spanish = DialogueLua.GetActorField("Dictionary", "English " + lan).asString;

        if (lan == "en" && language != "en")
        {
            pause = "PAUSED GAME";
            quit = "EXIT";
            back = "BACK TO GAME";
            controlString = "CONTROLS";
            helpTip = "IMPORTANT: PRESS BLOCK BUTTON CLOSE TO FALLEN PARTY MEMBER TO REVIVE THEM";
            backTip = "Press now any attack button to go back to Menu";
            language = "en";
            save = "SAVE GAME";
            DialogueManager.SetLanguage("en");
    //        DialogueManager.SetLanguage(Localization.GetLanguage(SystemLanguage.English));
            

        }
        else if (lan == "es" && language != "es")
        {
            pause = "JUEGO EN PAUSA";
            quit = "SALIR";
            back = "VOLVER AL JUEGO";
            controlString = "CONTROLES";
            helpTip = "IMPORTANTE: PRESIONA BLOQUEAR CERCA DE UN COMPAÑERO CAIDO PARA REVIVIRLO";
            backTip = "Presiona ahora cualquier botton de ataque para volver al menu de pausa";
            language = "es";
            save = "GUARDAR PARTIDA";
            DialogueManager.SetLanguage("es");
    //        DialogueManager.SetLanguage(Localization.GetLanguage(SystemLanguage.English));



        }
        else if (lan == "fr" && language != "fr")
        {
            pause = "JEU EN PAUSE";
            quit = "QUITTER";
            back = "RETOUR AU JEU";
            controlString = "CONTROLÊS";
            language = "fr";
            helpTip = "APPUYER SUR LE BOUTON DE BLOC POUR REVIVRE UN JOUEUR TOMBÉ";
            backTip = "Appuyez maintenant sur n'importe quel bouton d'attaque pour revenir au menu";
            DialogueManager.SetLanguage("fr");
            save = "SAUVEGARDER LA PARTIE";
     //       DialogueManager.SetLanguage(Localization.GetLanguage(SystemLanguage.English));
        }

        DialogueLua.SetVariable("language", lan);
    }

    private void Save ()
    {
    //    Debug.Log(GetComponent<SaveGame>().save); 
        SceneManager.LoadScene(scene);

    }

    public void ToggleOnOff ()
    {
    //    Debug.Log(timer + 1 + "/" + Time.timeSinceLevelLoad);
        if (timer + 1 < Time.timeSinceLevelLoad)
        {
    //        Debug.Log(timer + 1 + "/" + Time.timeSinceLevelLoad);
            timer = Time.timeSinceLevelLoad;
            this.enabled = !this.enabled;
        }

        if (gc == null)
        {
            gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }

        if (gc.isRPG == true)
        {
            mainCam.enabled = !mainCam.enabled;
            camRPG.enabled = !camRPG.enabled;
        }
    }

}
