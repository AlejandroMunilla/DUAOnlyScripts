using System.Collections;
using System.Collections.Generic;
using System.IO;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using Steamworks;
using Rewired;

public class MainMenu : MonoBehaviour {


    public Texture2D balder;
    public Texture2D nanna;
    public Texture2D simpleBoard;
    public Texture2D small_decor;
    //LOGGING PROFILE
    private string mySaveDirectory;
    private List<string> directories = new List<string>();
    private bool showProfiles = true;
    private bool showRPGMenu = false;
    private bool developer = false;

    //MAIN MENU
    public Texture2D background;
    public Texture2D logo;
    private Texture2D englishPic;
    private Texture2D spanishPic;
    private Texture2D frenchPic;
    private Texture2D skelleton;
    private string english = "English";
    private string french = "French";
    private string spanish = "Spanish";
    private string language = "en";

    private GUIStyle labelStyle;
 //   private string english
    public GUIStyle myStyle;
    public GUIStyle styleButton;
    public GUISkin mySkin;
    private string[] tempfiles;
    private string profile;
    private string tempProfile;
    private string scene;
    private string localGame = "TRADITIONAL ARCADE MODE";
    private string fpMode = "ARCADE FIRST PERSON MODE";
    private string arenaMode = "ARENA FIRST PERSON MODE";
    private string rpgMode = "RPG STORY MODE";
    private string backers = "BACKERS";
    private string quit = "EXIT";
    private string createProfile = "CREATE NEW PROFILE OR CHOOSE A PREVIOUS ONE";
    private string create = "CREATE";
    private string showAlert = "Profile already exists. Please choose another name";
    private string profilelabel = "PROFILE";
    private string maxNoProfiles = "Maximum number of profiles allowed: 5";
    private string newGame = "NEW GAME";
    private string loadGame = "LOAD SAVED GAME";
    private string rpgShow = "main";
    private string chosenRPG = "Balder";
    private string venture = "VENTURE FORTH!";
    private int buttonHeight;
    private int buttonWidthLoad;
    private int buttonWidth;

   //Rect data
    private int xProfiles;
    private int yProfiles;
    private Rect tempProfileRect;
    private Rect createProfileRect;
    private Rect virtualCursorRect;
    private Vector2 virtualCursorPoint;


    // other gamecontroller Xbox specific
    //    private Texture2D cursorNormal;
    private Texture2D overlay;
    private Rect overlayRect;
    private Rect englishFlag;
    private Rect frenchFlag;
    private Rect spanishFlag;
    private Rect localGameRect;
    private Rect fpModeRect;
    private Rect arenaModeRect;
    private Rect rpgModeRect;
    private Rect backersRect;
    private Rect exitRect;
    private Rect titleRect;
    private Rect subtitleRect;
    private Rect newRPGRect;
    private Rect loadRPGRect;
    private Rect backRPGRect;
    private Rect playerRect1;
    private Rect playerRect2;
    private Rect chosenRPGRect;
    private Rect ventureRPGRect;
    private Rect backNewRPGRect;
    private List<Rect> saveRects = new List<Rect>();
    private int internalCNT = 10;
    private float h;                                //horizontal axis
    private float v;                                //vertical axis
    public string hName = null;
    private string vName;
    private bool consolePlatform = true;                //A cursor created with a texture, not the Windows one.
    private bool xboxTimeOut = true;
    private List<string> controllers = new List<string>();
    private string firstControl;

    private CSteamID mySteamID;

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
        //    Debug.Log(ReInput.controllers.Joysticks.Count);
        // Assign each Joystick to a Player initially

        /*
        foreach (Rewired.Joystick j in ReInput.controllers.Joysticks)
        {
            if (ReInput.controllers.IsJoystickAssigned(j)) continue; // Joystick is already assigned

            // Assign Joystick to first Player that doesn't have any assigned
            AssignJoystickToNextOpenPlayer(j);
            Debug.Log(j.name );
        }*/
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

    void Start ()
    {
        AudioSource audioS = GetComponent<AudioSource>();
        audioS.time = 14.0f;
        audioS.Play();
        mySaveDirectory = Application.persistentDataPath + "/DANDU/";
    //    Debug.Log(mySaveDirectory);
        Time.timeScale = 1;
        background = (Texture2D)(Resources.Load("GUI/background"));
        logo = (Texture2D)(Resources.Load("GUI/Logo"));
        englishPic = (Texture2D)(Resources.Load("GUI/Flags/English"));
        frenchPic = (Texture2D)(Resources.Load("GUI/Flags/French"));
        spanishPic = (Texture2D)(Resources.Load("GUI/Flags/Spanish"));
        skelleton = (Texture2D)(Resources.Load("GUI/Skelleton"));
        //   cursorNormal = (Texture2D)(Resources.Load("GUI/Cursor/Normal"));
        overlay =   (Texture2D)(Resources.Load("GUI/ActiveButton"));
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

        buttonHeight = (int)(Screen.height * 0.06f);
        buttonWidth = (int)(Screen.width * 0.2f);
        buttonWidthLoad = (int)(Screen.width * 0.4f);

        virtualCursorPoint = new Vector2((int)(Screen.width * 0.5f), (int)(Screen.height * 0.5f));

        tempProfileRect = new Rect(Screen.width * 0.1f + buttonWidth, Screen.height * 0.40f, buttonWidth, buttonHeight);
        createProfileRect = new Rect(Screen.width * 0.1f, Screen.height * 0.40f, buttonWidth, buttonHeight);
        virtualCursorRect = new Rect(virtualCursorPoint.x, virtualCursorPoint.y, Screen.width * 0.05f, Screen.width * 0.05f);
        playerRect1 = new Rect(Screen.width * 0.01f, Screen.height * 0.5f, Screen.width * 0.27f, Screen.width * 0.27f);
        playerRect2 = new Rect(playerRect1.x + playerRect1.width, Screen.height * 0.5f, Screen.width * 0.27f, Screen.width * 0.27f);
        englishFlag = new Rect(Screen.width * 0.02f, Screen.height * 0.19f, Screen.width * 0.06f, Screen.width * 0.04f);
        spanishFlag = new Rect(Screen.width * 0.07f, Screen.height * 0.19f, Screen.width * 0.06f, Screen.width * 0.04f);
        frenchFlag = new Rect(Screen.width * 0.12f, Screen.height * 0.19f, Screen.width * 0.06f, Screen.width * 0.04f);
        Rect save1Rect = new Rect(Screen.width * 0.1f, Screen.height * 0.46f + (0 * buttonHeight), buttonWidthLoad, buttonHeight);
        Rect save2Rect = new Rect(Screen.width * 0.1f, Screen.height * 0.46f + (1 * buttonHeight), buttonWidthLoad, buttonHeight);
        Rect save3Rect = new Rect(Screen.width * 0.1f, Screen.height * 0.46f + (2 * buttonHeight), buttonWidthLoad, buttonHeight);
        Rect save4Rect = new Rect(Screen.width * 0.1f, Screen.height * 0.46f + (3 * buttonHeight), buttonWidthLoad, buttonHeight);

        saveRects.Add(save1Rect);
        saveRects.Add(save2Rect);
        saveRects.Add(save3Rect);
        saveRects.Add(save4Rect);

        int buttonWidthTemp = (int)(Screen.width * 0.35f);
        int buttonHeightTemp = (int)( Screen.height * 0.08f);
       
        titleRect = new Rect(Screen.width * 0.1f, Screen.height * 0.17f, Screen.width * 0.8f, Screen.width * 0.10f);
        subtitleRect = new Rect(Screen.width * 0.1f, titleRect.y + titleRect.height, Screen.width * 0.8f, Screen.width * 0.12f);
        localGameRect = new Rect(Screen.width * 0.6f, Screen.height * 0.45f, buttonWidthTemp, buttonHeightTemp);
        rpgModeRect = new Rect(Screen.width * 0.6f, localGameRect.y + buttonHeightTemp, buttonWidthTemp, buttonHeightTemp);
        //     fpModeRect = new Rect(Screen.width * 0.6f, Screen.height * 0.53f, buttonWidthTemp, Screen.height * 0.08f);
        arenaModeRect = new Rect(Screen.width * 0.6f, rpgModeRect.y + buttonHeightTemp, buttonWidthTemp, buttonHeightTemp);
        backersRect = new Rect(Screen.width * 0.6f, Screen.height * 0.68f, Screen.width * 0.35f, Screen.height * 0.08f);
        exitRect = new Rect(Screen.width * 0.6f, Screen.height * 0.76f, Screen.width * 0.35f, Screen.height * 0.08f);

        newRPGRect = new Rect(Screen.width * 0.6f, Screen.height * 0.45f, buttonWidthTemp, buttonHeightTemp);
        loadRPGRect = new Rect(Screen.width * 0.6f, localGameRect.y + buttonHeightTemp, buttonWidthTemp, buttonHeightTemp);
        backRPGRect = new Rect(Screen.width * 0.6f, Screen.height * 0.76f, Screen.width * 0.35f, Screen.height * 0.08f);

        chosenRPGRect = new Rect(Screen.width * 0.6f, Screen.height * 0.45f, buttonWidthTemp, buttonHeightTemp);
        ventureRPGRect = new Rect(Screen.width * 0.6f, localGameRect.y + buttonHeightTemp, buttonWidthTemp, buttonHeightTemp);
        backNewRPGRect = new Rect(Screen.width * 0.6f, Screen.height * 0.76f, Screen.width * 0.35f, Screen.height * 0.08f);


        overlayRect = createProfileRect;
        /*
        CallControllers();
        if (Application.platform == RuntimePlatform.XboxOne)
        {
            consolePlatform = true;
            Debug.Log(Application.platform);
            Invoke("XboxController", 0);
            overlayRect = createProfileRect;
        }

        if (controllers.Count > 0)
        {
            Invoke("XboxController", 0);
            overlayRect = createProfileRect;
        }
        //     GetControllerInfo();*/

        Invoke("XboxController", 0);

        GetFiles();

        if (SteamAPI.Init())
            Invoke("GetSteamID", 1);


        else
            Debug.Log("Steam API init -- failure ...");

    }


    private void GetSteamID()
    {

        mySteamID = SteamUser.GetSteamID();
        string steamName = SteamFriends.GetPersonaName();

     //   Debug.Log(steamName);
        if (steamName == "alexhapki" || steamName == "murkentropic")
        {
            developer = true;
        }
    }

    private void OnDisable()
    {
        CancelInvoke("XboxController2");
    }

    private void GetFiles ()
    {
        if (Directory.Exists (mySaveDirectory))
        {
            directories.Clear();            
            tempfiles = Directory.GetDirectories(mySaveDirectory);

            if (tempfiles.Length > 0)
            {
                for (int cnt = 0; cnt < tempfiles.Length; cnt++)
                {
                    directories.Add(tempfiles[cnt]);
                    //        Debug.Log(tempfiles[cnt]);
                }
            }
            else if (Application.platform == RuntimePlatform.XboxOne)
            {
                consolePlatform = true;
                Debug.Log(Application.platform);
                ///////// create 5 profiles unless Xbox can enter text as windows

            }

        }
        else
        {
            Directory.CreateDirectory(mySaveDirectory);
        }
    }


    private void OnGUI()
    {
        Background();
    //    Debug.Log(Time.timeScale);

        if (showProfiles == true)
        {
            GUI.Label(new Rect(Screen.width * 0.2f, Screen.height * 0.32f, Screen.width * 0.6f, Screen.width * 0.05f), createProfile, styleButton);
            tempProfile = GUI.TextArea(tempProfileRect, tempProfile);
            if (GUI.Button(createProfileRect, create, styleButton) && tempProfile != "" && tempProfile != null)
            {            
                CreateProfile();
            }
            if (directories.Count != 0)
            {
                for (int cnt = 0; cnt < directories.Count; cnt++)
                {
                    if (GUI.Button(new Rect(Screen.width * 0.1f, Screen.height * 0.46f + (cnt * buttonHeight), buttonWidthLoad, buttonHeight), Path.GetFileNameWithoutExtension(directories[cnt].ToString()), styleButton)  )
                    {
                        AssignProfile(cnt);
                    }
                }                
            }
        }
        else if (showRPGMenu)
        {
            GUI.Label(subtitleRect, "RPG", myStyle);
            if (rpgShow == "main")
            {

                if (GUI.Button(newRPGRect, newGame, styleButton))
                {
                    rpgShow = "new";
                    overlayRect = playerRect1;
                    chosenRPG = "Balder";
                }
                if (GUI.Button(loadRPGRect, loadGame, styleButton))
                {
     
                }
                if (GUI.Button(backRPGRect, quit, styleButton))
                {
                    showRPGMenu = false;
                    rpgShow = "main";
                }
            }
            else if (rpgShow == "new")
            {               
                GUI.DrawTexture(playerRect1, simpleBoard);
                GUI.DrawTexture(playerRect2, simpleBoard);
                if (GUI.Button(playerRect1, balder))
                {
                    chosenRPG = "Balder";
                }
                if (GUI.Button(playerRect2, nanna))
                {
                    chosenRPG = "Nanna";
                }
                GUI.DrawTexture(chosenRPGRect, small_decor);
                GUI.Label(chosenRPGRect, chosenRPG, myStyle);
                if (GUI.Button(ventureRPGRect, venture, styleButton))
                {
                    CallRPG();

                }
                if (GUI.Button(backNewRPGRect, quit, styleButton))
                {
                    rpgShow = "main";
                    showRPGMenu = false;
                }
            }
        }
        else
        {
            GUI.DrawTexture(new Rect(Screen.width * 0.02f, Screen.height * 0.3f, Screen.width * 0.45f, Screen.width * 0.41f), skelleton);
            if (GUI.Button(localGameRect, localGame, styleButton))
            {
                CallLocalGame();
                DialogueLua.SetVariable("sceneFP", "No");

            }
            
            if (GUI.Button(rpgModeRect, rpgMode, styleButton))
            {
                showRPGMenu = true;
          //      DialogueLua.SetVariable("rpgGame", "Yes");
            }


            if (GUI.Button(arenaModeRect, arenaMode, styleButton))
            {
                CallArenaMode();
       //         Debug.Log("Arena");
                DialogueLua.SetVariable("sceneFP", "Yes");
                DialogueLua.SetVariable("arenaMode", "Yes");
            }


            if (GUI.Button(backersRect, backers, styleButton))
            {
                EnableBackers();
            }

            if (GUI.Button(exitRect, quit, styleButton))
            {
                Application.Quit();
            }

            if (GUI.Button (new Rect (exitRect.x , exitRect.y + exitRect.height, exitRect.width, exitRect.height), "MULTIPLAYER", styleButton) && developer == true)
            {
                CallMultiplayer();
            }

            GUI.Label(new Rect(Screen.width * 0.2f, Screen.height * 0.85f, Screen.width * 0.2f, Screen.height * 0.1f), profilelabel + " " + profile, labelStyle);
        }        
        Flags();
        
        if (ReInput.controllers.Joysticks.Count > 0)
        {
            GUI.DrawTexture(overlayRect, overlay);
        }
    }

    public void Background ()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);
        GUI.DrawTexture(new Rect(Screen.width * 0.1f, Screen.height * 0.02f, Screen.width * 0.8f, Screen.width * 0.13f), logo);
        GUI.Label(titleRect, "DUNGEONS AND UNDEAD", myStyle);
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
            localGame = "TRADITIONAL ARCADE MODE";
            backers = "BACKERS";
            fpMode = "FIRST PERSON ARCADE MODE";
            arenaMode = "ARENA MODE (FIRST PERSON)";
            rpgMode = "RPG STORY MODE";
            quit = "EXIT";
            createProfile = "CREATE NEW PROFILE OR CHOOSE A PREVIOUS ONE";
            create = "CREATE";
            showAlert = "Profile already exists. Please choose another name";
            DialogueManager.SetLanguage("en");
            language = "en";
            profilelabel = "PROFILE";
            maxNoProfiles = "Maximum number of profiles allowed: 5";
            newGame = "NEW GAME";
            loadGame = "LOAD SAVED GAME";
            venture = "VENTURE FORTH!";

        }
        else if (lan == "es" && language != "es")
        {
            localGame = "MODO ARCADE TRADICIONAL";
            fpMode = "MODE ARCADE PRIMERA PERSONA";
            arenaMode = "MODO ARENA (PRIMERA PERSONA)";
            rpgMode = "MODO HISTORIA RPG";
            backers = "PATROCINADORES";
            quit = "SALIR";
            createProfile = "NUEVO PERFIL O ELIJA UNO YA CREADO";
            showAlert = "El perfil ya existe. Por favor elija otro nombre";
            create = "CREAR";
            language = "es";
            profilelabel = "PERFIL";
            maxNoProfiles = "Numero maximo de perfiles permitidos: 5";
            DialogueManager.SetLanguage("es");
            newGame = "JUEGO NUEVO";
            loadGame = "CARGAR JUEGO GUARDADO";
            venture = "AVENTURAOS!";
        }
        else if (lan == "fr" && language != "fr")
        {
            localGame = "MODE ARCADE TRADITIONNEL";
            fpMode = "MODE ARCADE PREMIÈRE PERSONNE";
            arenaMode = "MODE ARENA (PREMIÈRE PERSONNE)";
            rpgMode = "MODE HISTOIRE RPG";
            backers = "CONTRIBUTEURS";
            quit = "QUITTER";
            createProfile = "CRÉER UN NOUVEAU PROFIL OU CHOISIR UN PRÉCEDÉNT";
            showAlert = "Le profil existe déjà. Veuillez choisir un autre nom";
            language = "fr";
            create = "CRÉER";
            profilelabel = "PROFIL";
            maxNoProfiles = "Nombre maximum de profils autorisés: 5";
            DialogueManager.SetLanguage("fr");
            newGame = "NOUVEAU JEU";
            loadGame = "CHARGER UN JEU ENREGISTRÉ";
            venture = "AVENTUREZ-VOUS!";
        }

        DialogueLua.SetVariable("language", lan);
        Debug.Log(lan);
       
    }


    private void XboxController ()
    {
               


        for (int cnt5 = 0; cnt5 < ReInput.players.Players.Count; cnt5++)
        {
            if (ReInput.players.Players[cnt5].GetButtonUp ("Fire"))
            {
                if (overlayRect == createProfileRect)
                {
                    CreateProfile();
                }
                else if (overlayRect == englishFlag)
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
                else if (internalCNT != 10)
                {
                    AssignProfile(internalCNT);
                }
            }

            if (ReInput.players.Players[cnt5].GetAxis("Move Horizontal") > 0.9f)
            {
     //           Debug.Log(ReInput.players.Players[cnt5].GetAxis("Move Horizontal"));
                if (xboxTimeOut == true)
                {
                    xboxTimeOut = false;
                    Invoke("ChangeXboxBool", 0.3f);


                    if (overlayRect == englishFlag)
                    {
                        overlayRect = spanishFlag;
                    }
                    else if (overlayRect == spanishFlag)
                    {
                        overlayRect = frenchFlag;
                    }
                    else if (overlayRect == frenchFlag)
                    {
                        overlayRect = createProfileRect;
                    }
                    else if (overlayRect == createProfileRect)
                    {
                        if (directories.Count > 0)
                        {
                            overlayRect = saveRects[0];
                            internalCNT = 0;
          //                  Debug.Log(internalCNT);
                        }
                        else
                        {
                            internalCNT = 10;
                            overlayRect = englishFlag;
                        }

                    }
                    else if (internalCNT != 10)
                    {
                        if (internalCNT + 1 < directories.Count )
                        {
                  //          Debug.Log(internalCNT);
                            internalCNT++;
                            Debug.Log(internalCNT);
                            overlayRect = saveRects[internalCNT];
                        }
                        else if (internalCNT  + 1 >= directories.Count)
                        {
                            internalCNT = 10;
                            overlayRect = englishFlag;
                        }
                    }
                }
            }

            else if (ReInput.players.Players[cnt5].GetAxis("Move Horizontal") < - 0.9f)
            {
                //           Debug.Log(ReInput.players.Players[cnt5].GetAxis("Move Horizontal"));
                if (xboxTimeOut == true)
                {
                    xboxTimeOut = false;
                    Invoke("ChangeXboxBool", 0.3f);


                    if (overlayRect == englishFlag)
                    {
                        internalCNT = directories.Count - 1;
                        overlayRect = saveRects[internalCNT];
                    }
                    else if (overlayRect == spanishFlag)
                    {
                        overlayRect = englishFlag;
                    }
                    else if (overlayRect == frenchFlag)
                    {
                        overlayRect = spanishFlag;
                    }

                    else if (overlayRect == createProfileRect)
                    {
                        overlayRect = frenchFlag;

                    }
                    else if (internalCNT != 10)
                    {
                        if (internalCNT == 0)
                        {
                            overlayRect = createProfileRect;
                            internalCNT = 10;
                        }
                        else
                        {
                            internalCNT--;
                            overlayRect = saveRects[internalCNT];
                        }

                    }
                }
            }
        }




        if (showProfiles == true)
        {
            Invoke("XboxController", 0);
        }

        

    }

    
    public void XboxController2 ()
    {
        for (int cnt5 = 0; cnt5 < ReInput.players.Players.Count; cnt5++)
        {
            if (ReInput.players.Players[cnt5].GetButtonUp("Fire"))
            {

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

                if (showRPGMenu == false)
                { 
                    if (overlayRect == localGameRect)
                    {
                        DialogueLua.SetVariable("sceneFP", "No");
                        CallLocalGame();
                    }
                    else if (overlayRect == rpgModeRect)
                    {
                        DialogueLua.SetVariable("sceneFP", "Yes");
                        xboxTimeOut = false;
                        Invoke("ChangeXboxBool", 0.3f);
                        overlayRect = newRPGRect;
                        showRPGMenu = true;
                    }
                    else if (overlayRect == arenaModeRect)
                    {
                        CallArenaMode();
                        DialogueLua.SetVariable("sceneFP", "Yes");

                    }
                    else if (overlayRect == backersRect)
                    {
                        EnableBackers();
                    }
                    else if (overlayRect == exitRect)
                    {
                        Application.Quit();
                    }
                }
                else
                {
                    if (xboxTimeOut == true)
                    {
                        xboxTimeOut = false;
                        Invoke("ChangeXboxBool", 0.3f);


                        if (rpgShow == "main")
                        {
                            if (overlayRect == newRPGRect)
                            {
                                rpgShow = "new";
                                overlayRect = playerRect1;
                            }
                            else if (overlayRect == loadRPGRect)
                            {
                                Debug.Log("load");
                            }
                            else if (overlayRect == backRPGRect)
                            {
                                rpgShow = "main";
                                showRPGMenu = false;
                            }

                        }
                        if (rpgShow == "new")
                        {
                            if (overlayRect == playerRect1)
                            {
                      //          overlayRect = playerRect2;
                                chosenRPG = "Balder";

                            }
                            else if (overlayRect == playerRect2)
                            {
                    //            overlayRect = playerRect1;
                                chosenRPG = "Nanna";
                            }
                            else if (overlayRect == ventureRPGRect)
                            {
                                CallRPG();
                            }
                            else if (overlayRect == backNewRPGRect)
                            {
                                rpgShow = "main";
                                showRPGMenu = false;
                                overlayRect = rpgModeRect;
                            }
                        }
                    }

                    
                }

            }

            if (xboxTimeOut == true)
            {

                if (ReInput.players.Players[cnt5].GetAxis("Move Horizontal") > 0.9f)
                {
                    xboxTimeOut = false;
                    Invoke("ChangeXboxBool", 0.3f);

                    if (overlayRect == englishFlag)
                    {
                        overlayRect = spanishFlag;
                    }
                    else if (overlayRect == spanishFlag)
                    {
                        overlayRect = frenchFlag;
                    }
                    else if (overlayRect == frenchFlag)
                    {
                        if (showRPGMenu == false)
                        {
                            overlayRect = localGameRect;
                        }
                        else
                        {
                            if (rpgShow == "main")
                            {
                                overlayRect = newRPGRect;
                            }
                            else if (rpgShow == "new")
                            {
                                overlayRect = playerRect1;
                            }
                           
                        }
                        
                    }

                    if (showRPGMenu == false)
                    {
                        if (overlayRect == localGameRect)
                        {
                            overlayRect = rpgModeRect;
                        }
                        else if (overlayRect == rpgModeRect)
                        {
                            overlayRect = arenaModeRect;
                        }
                        else if (overlayRect == arenaModeRect)
                        {
                            overlayRect = backersRect;

                        }
                        else if (overlayRect == backersRect)
                        {
                            overlayRect = exitRect;
                        }
                        else if (overlayRect == exitRect)
                        {
                            overlayRect = englishFlag;
                        }
                        
                    }
                    else
                    {
         //               Debug.Log(rpgShow);
                        if (rpgShow == "main")
                        {
                            if (overlayRect == newRPGRect)
                            {
                                overlayRect = loadRPGRect;
                            }
                            else if (overlayRect == loadRPGRect)
                            {
                                overlayRect = backRPGRect;
                            }
                            else if (overlayRect == backRPGRect)
                            {
                                overlayRect = englishFlag;
                            }
                        }
                        else if (rpgShow == "new") 
                        {
                            if (overlayRect == playerRect1)
                            {
                                overlayRect = playerRect2;
                            }
                            else if (overlayRect == playerRect2)
                            {
                                overlayRect = ventureRPGRect;
                            }
                            else if (overlayRect == ventureRPGRect )
                            {
                                overlayRect = backNewRPGRect;
                            }
                            else if (overlayRect == backNewRPGRect)
                            {
                                overlayRect = playerRect1;
                            }
                        }

         
                    }


                }
                else if (ReInput.players.Players[cnt5].GetAxis("Move Horizontal") < -0.9f)
                {
                    xboxTimeOut = false;
                    Invoke("ChangeXboxBool", 0.3f);

                    if (overlayRect == englishFlag)
                    {
                        if (showRPGMenu == false)
                        {
                            overlayRect = exitRect;
                        }
                        else
                        {
                            overlayRect = backRPGRect;
                            if (rpgShow == "main")
                            {
                                overlayRect = backRPGRect;
                            }
                            else if (rpgShow == "new")
                            {
                                overlayRect = backNewRPGRect;
                            }

                        }

                    }
                    else if (overlayRect == spanishFlag)
                    {
                        overlayRect = englishFlag;
                    }
                    else if (overlayRect == frenchFlag)
                    {
                        overlayRect = spanishFlag;
                    }

                    if (showRPGMenu == false)
                    {
                        if (overlayRect == localGameRect)
                        {
                            overlayRect = frenchFlag;
                        }
                        else if (overlayRect == rpgModeRect)
                        {
                            overlayRect = localGameRect;
                        }
                        else if (overlayRect == arenaModeRect)
                        {
                            overlayRect = rpgModeRect;
                        }

                        else if (overlayRect == backersRect)
                        {
                            overlayRect = arenaModeRect;
                        }
                        else if (overlayRect == exitRect)
                        {
                            overlayRect = backersRect;
                        }

                    }
                    else
                    {
                        if (rpgShow == "main")
                        {
                            if (overlayRect == backRPGRect)
                            {
                                overlayRect = loadRPGRect;
                            }
                            else if (overlayRect == loadRPGRect)
                            {
                                overlayRect = newRPGRect;
                            }
                            else if (overlayRect == newRPGRect)
                            {
                                overlayRect = frenchFlag;
                            }
                        }
                        else if (rpgShow == "new")
                        {
                            if (overlayRect == backNewRPGRect)
                            {
                                overlayRect = ventureRPGRect;
                            }
                            else if (overlayRect == ventureRPGRect)
                            {
                                overlayRect = playerRect2;
                            }
                            else if (overlayRect == playerRect2)
                            {
                                overlayRect = playerRect1;
                            }
                            else if (overlayRect == playerRect1)
                            {
                                overlayRect = backNewRPGRect;
                            }
                        }

                    }



                }

            }


        }



        if (this.enabled == true)
        {
            Invoke("XboxController2", 0);
        }

    }

    private void CreateProfile ()
    {
        bool folderExists = false;
        Debug.Log(tempProfile);

        if (tempProfile != null && tempProfile != "")
        {
            if (directories.Count < 4)
            {
                for (int cnt = 0; cnt < directories.Count; cnt++)
                {

                    if (tempProfile == Path.GetFileNameWithoutExtension(directories[cnt].ToString()))
                    {
                        folderExists = true;
                    }
                }
                if (folderExists == true)
                {
                    GetComponent<DialogueSystemTrigger>().alertMessage = showAlert;
                    GetComponent<DialogueSystemTrigger>().OnUse();
                }
                else
                {
                    profile = tempProfile;
                    DialogueLua.SetVariable("profile", profile);
                    showProfiles = false;
                    Directory.CreateDirectory(mySaveDirectory + tempProfile);
                    Debug.Log(mySaveDirectory + tempProfile);
                }
            }
            else
            {
                DialogueManager.ShowAlert(maxNoProfiles);
            }
        }
        else
        {
            DialogueManager.ShowAlert("???");
        }



    }

    private void ChangeXboxBool ()
    {
        xboxTimeOut = true;
    }

    private void AssignProfile (int cnt)
    {
        profile = Path.GetFileNameWithoutExtension(directories[cnt].ToString());
        DialogueLua.SetVariable("profile", profile);
   //     Debug.Log (profile);
        showProfiles = false;
        overlayRect = localGameRect;
        Invoke("XboxController2", 1);
    }

    private void EnableBackers()
    {
        GetComponent<Backers>().enabled = true;
        this.enabled = false;
    }

    private void CallLocalGame ()
    {
        Debug.Log("local");
        Time.timeScale = 1;
        DialogueLua.SetVariable("language", language);
        DialogueLua.SetVariable("targetScene", "ArcadeMode");
        DialogueLua.SetVariable("test", "ArcadeMode");
        DialogueLua.SetVariable("sceneRPG", "No");
        scene = "PreLevel";
        GetComponent<SaveGame>().sceneToExit = scene;
        Invoke("Save", 1);


    }

    private void CallArenaMode ()
    {
        Time.timeScale = 1;
        Debug.Log("arena");
        DialogueLua.SetVariable("language", language);
        DialogueLua.SetVariable("targetScene", "ArenaCross");
        DialogueLua.SetVariable("test", "Yes");        
        scene = "PreLevel";
        GetComponent<SaveGame>().sceneToExit = scene;
        DialogueLua.SetVariable("sceneFP", "Yes");
        DialogueLua.SetVariable("arenaMode", "Yes");
        DialogueLua.SetVariable("sceneRPG", "No");
        Invoke("Save", 1);

 
    }

    private void CallMultiplayer ()
    {
        DialogueLua.SetVariable("language", language);
        DialogueLua.SetVariable("targetScene", "MultiMenu");
        DialogueLua.SetVariable("test", "Yes");
        scene = "MultiMenu";
        GetComponent<SaveGame>().sceneToExit = scene;
        DialogueLua.SetVariable("sceneFP", "Yes");
        DialogueLua.SetVariable("arenaMode", "No");
        DialogueLua.SetVariable("multiplayer", "Yes");
        DialogueLua.SetVariable("sceneRPG", "No");
        Invoke("Save", 1);
    }

    private void CallRPG ()
    {
        DialogueLua.SetActorField("Player1", "chosen", chosenRPG);
        DialogueLua.SetActorField("Player2", "chosen", "Rose");
        DialogueLua.SetActorField("Player3", "chosen", "Fred");
        DialogueLua.SetActorField("Player4", "chosen", "None");
        DialogueLua.SetVariable("language", language);
        DialogueLua.SetVariable("targetScene", "The Cross Inn");
        DialogueLua.SetVariable("test", "Yes");
        scene = "The Cross Inn";
        GetComponent<SaveGame>().sceneToExit = "The Cross Inn";
        DialogueLua.SetVariable("sceneFP", "Yes");
        DialogueLua.SetVariable("arenaMode", "No");
        DialogueLua.SetVariable("multiplayer", "No");
        DialogueLua.SetVariable("sceneRPG", "Yes");
        Invoke("Save", 1);
    }

    private void Save()
    {

        GetComponent<SaveGame>().SaveProfile("AutoSave", "MainMenu", true);

    }

    private void CallControllers()
    {
        controllers.Clear();
        string[] tempControllers = Input.GetJoystickNames();

        if (tempControllers != null)
        {
            //       Debug.Log(tempControllers.Length);
            if (tempControllers.Length > 0)
            {

                bool addjustValues = false;

                for (int cnt = 0; cnt < tempControllers.Length; cnt++)
                {
                    if (tempControllers[cnt] != "")
                    {

                        int tempCNT = cnt + 1;
                        controllers.Add("joystick " + tempCNT.ToString());
                        //                     Debug.Log("joystick " + tempCNT.ToString());           

                    }
                }
                firstControl = controllers[0];
    //            Debug.Log(controllers[0]);
            }


        }
    }
}
