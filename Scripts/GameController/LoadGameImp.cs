using UnityEngine;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Rewired;
using System;
using UnityEngine.UI;

public class LoadGameImp : MonoBehaviour {
    public bool loadWorld = true;
    public bool questLoaded = false;
    public bool gameLoaded = false;
    public List<string> controllers = new List<string>();


    private GameController gc;
    private string saveFileName;            //Use this to import data for a game already in course
    private string fileDirectory;
    public string profile;
    private string loadPosition = "No";
    private bool loadPosBool = false;
    private myGUI myGui;
    private bool waitForPlayers = false;
    

    //multiplayer
    private int multiID;
    private string multiplayer;
    private GameObject characterGO;
    private Server server;

    //RPG
    //  List<InventoryRPG> inventoryRPG = new List<InventoryRPG>();
    public GameObject rpgMenu;

    private void Awake()
    {
        // Subscribe to events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnect;
       
 //      rpgMenu = GameObject.FindGameObjectWithTag("RPGMenu");

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
   //     rpgMenu = GameObject.FindGameObjectWithTag("RPGMenu");
   //     Debug.Log(rpgMenu);
        Time.timeScale = 1;
        gc = GetComponent<GameController>();
        myGui = GetComponent<myGUI>();
        LoadCurrentProfile();
    }

    public void LoadCurrentProfile()
    {
              
        string path = Application.persistentDataPath + "/DandU";
        string fileName = path + "/CurrentProfile.dat";
        
     //    Debug.Log(fileName);
        if (File.Exists(fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(fileName, FileMode.Open);
            SaveNames data = (SaveNames)bf.Deserialize(file);
            file.Close();
            if (gc != null)
            {
                gc.profile = data.currentProfile;
                
            }
            profile = data.currentProfile;
            DialogueLua.SetVariable("profile", profile);
            fileDirectory = data.saveFileName;
       //     Debug.Log(data.currentProfile);
            Invoke("LoadQuests", 0.1f);
        }
        else
        {
            myGui.debugGUI = myGui.debugGUI + "/LoadProfileNull/";
        }

        
    }



    public void LoadQuests()
    {
        string path = Application.persistentDataPath + "/DandU";
        string fileName = path + "/" + profile + "/" + fileDirectory + ".dat";
        
        if (File.Exists(fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(fileName, FileMode.Open);
            SaveQuest data = (SaveQuest)bf.Deserialize(file);
            file.Close();
            string saveData = data.saveData;
            PersistentDataManager.ApplySaveData(saveData);
     //       Debug.Log(saveData);
            //    		DialogueManager.Instance.GetComponent<LevelManager>().LoadGame(saveData);

            if (loadWorld == true)
            {
                Invoke("LoadWorld", 0.1f);
            }
            else
            {
                Invoke ("Test", 0.1f);
            }

       
        }
        else
        {
            myGui.debugGUI = myGui.debugGUI + "/LoadQuestNull/";
        }
    }

    private void LoadWorld ()
    {
        string[] quests = QuestLog.GetAllQuests();
        /*
        foreach (string st in quests)
        {
            Debug.Log(st);
        }*/

        int playerNos = ReInput.players.Players.Count;
        int joystickNos = ReInput.controllers.Joysticks.Count;
        Invoke("ForTestingOnly", 3);
    //    Debug.Log(playerNos);

        if (joystickNos > playerNos)
        {
            waitForPlayers = true;
        }

        string sceneName = SceneManager.GetActiveScene().name;
        string languageOnLoad = DialogueLua.GetVariable("language").AsString;
        string isSceneFP = DialogueLua.GetVariable("sceneFP").asString;
        multiplayer = DialogueLua.GetVariable("multiplayer").asString;
        string isRPG = DialogueLua.GetVariable("sceneRPG").asString; 
        string arenaMode = DialogueLua.GetVariable("arenaMode").asString;
        loadPosition  = DialogueLua.GetVariable("nextPos").asString;

    //    Debug.Log(isRPG + "/RPG");

        if (isSceneFP == "Yes")
        {
            gc.sceneFP = true;
        }

        if (isRPG == "Yes")
        {
            gc.isRPG = true;
            gc.safeArea = true;
            LoadAllInventory();
        }
        else
        {
            gc.isRPG = false;
        }

        if (arenaMode == "Yes")
        {
            Debug.Log(arenaMode);
            gc.arenaMode = true;
            GetComponent<ArenaVillage>().enabled = true;
        }

        foreach (Camera ca in Camera.allCameras)
        {
            if (ca.name == "CameraMap")
            {
                ca.enabled = false;
            }
            if (ca.name == "Camera1")
            {
                ca.enabled = true;
            }
        }

        if (multiplayer == "Yes")
        {
            gc.multiplayer = true;
            string serverString = DialogueLua.GetVariable("server").asString;
            if (serverString == "Yes")
            {
                gc.server = true;
            }
            else
            {
                gc.server = false;
            }

            multiID = DialogueLua.GetVariable("multiID").asInt;
            gc.playerID = multiID;
            if (GameObject.FindGameObjectWithTag("SteamManager").GetComponent<Server>() != null)
            {
                server = GameObject.FindGameObjectWithTag("SteamManager").GetComponent<Server>();
                server.gc = gc;
            }

        }
        if (loadPosition != "No" && loadPosition != "" && loadPosition != "nil")
        {
   //         Debug.Log(loadPosition);
            loadPosBool = true;
        }

        DialogueManager.SetLanguage(languageOnLoad);
        DialogueLua.SetVariable("sceneCurrent", sceneName);

        LoadCharacters();     

        myGui.enabled = true;
        gc.RegenerateMana();

    }

    private void LoadCharacters ()
    {
        Transform pos01;
        if (loadPosBool == true)
        {
     //       Debug.Log("Positions/" + loadPosition + "/01");
            if (transform.Find("Positions/" + loadPosition + "/01") != null)
            {
                pos01 = transform.Find("Positions/" + loadPosition + "/01");
            }
            else
            {
                pos01 = transform.Find("Positions/Pos01");
            }
        }
        else
        {
            pos01 = transform.Find("Positions/Pos01");
        }
       
        string chosen1 = DialogueLua.GetActorField("Player1", "chosen").asString;
    //    Debug.Log(chosen1);
        myGui.debugGUI = myGui.debugGUI + "/" + chosen1 + "/";
        GameObject go1 = Instantiate(Resources.Load("PC/" + chosen1), pos01.position, pos01.rotation) as GameObject;
        if (gc != null)
        {
            gc.player1 = go1;
        }
        else
        {
            /////////////////////////////////////////////////////////////////////////////
        }
        go1.name = chosen1;
        gc.activePlayer = go1;
        ThirdPersonUserControl tpu = go1.GetComponent<ThirdPersonUserControl>();
        int playerID1 = DialogueLua.GetActorField("Player1", "controller").asInt;
        string control1 = DialogueLua.GetActorField("Player1", "control").asString;
        tpu.playerR = ReInput.players.Players[playerID1];
        gc.player1 = go1;
        gc.players.Add(go1);
        myGui.pic1 = (Texture2D)(Resources.Load("Portraits/" + chosen1));
        int multiID1 = 0;

        if (multiplayer == "Yes")
        {
            go1.GetComponent<PlayerStats>().multiplayerID = multiID1;
            if (multiID == multiID1)
            {
                characterGO = go1;
                if (go1.GetComponent<MultiplayerSendInfo>() == null)
                {
                    go1.AddComponent<MultiplayerSendInfo>();
                }
                else
                {
                    go1.GetComponent<MultiplayerSendInfo>().enabled = true;
                }
                myGui.pic1 = (Texture2D)(Resources.Load("Portraits/" + chosen1));

            }
            else
            {
                go1.GetComponent<ThirdPersonCharacter>().enabled = false;
                go1.GetComponent<ThirdPersonUserControl>().enabled = false;
                go1.GetComponent<PlayerAttack>().ghostAvatar = true;
                EnemyAI ea = go1.GetComponent<EnemyAI>();
                ea.ghostAvatarBool = true;
                ea.enabled = false;
                ea.enabled = true;
                myGui.pic1 = null;
            }   
        }


        //       string control1 = DialogueLua.GetActorField("Player", "control").asString;
        tpu.playerR = ReInput.players.Players[0];
        Player player1R = tpu.playerR;
        if (control1 == "Mouse&Keyboard" || gc.isRPG == true || gc.multiplayer == true)
        {
    //        Debug.Log("Moise");
            player1R.controllers.hasKeyboard = true;
            player1R.controllers.hasMouse = true;
        }
        else
        {
            player1R.controllers.hasKeyboard = false;
            player1R.controllers.hasMouse = false;
        }

    //    Debug.Log(gc.isRPG + "/" + chosen1);
        if (gc.isRPG== true)
        {
            DialogueLua.SetActorField(chosen1, "inParty", "Yes");
            if (chosen1 == "Nanna")
            {
                //       DialogueManager.SetPortrait("Player1", "Portraits/Nanna");
                DialogueManager.SetPortrait("Player1", "Portraits/Nanna");
                Invoke("CallPortrait", 3);
            }
                

            foreach (Joystick j in ReInput.controllers.Joysticks)
            {
                tpu.playerR.controllers.AddController(j, true);
            }
            
        }

        gc.player1.GetComponent<PlayerStats>().internalCNT = 0;
        string skill1 = DialogueLua.GetActorField(chosen1, "skill1").asString;
        string skill2 = DialogueLua.GetActorField(chosen1, "skill2").asString;

        if (Resources.Load("Skills/" + skill1 + "/3"))
        {
            myGui.skill1Tex.Add((Texture2D)(Resources.Load("Skills/" + skill1 + "/3")));
            myGui.skill1TexActive.Add((Texture2D)(Resources.Load("Skills/" + skill1 + "/3")));
            myGui.skill1TexNo.Add((Texture2D)(Resources.Load("Skills/" + skill1 + "/3no")));
            myGui.skill1Cool.Add(0);
        }

        if (Resources.Load("Skills/" + skill2 + "/3"))
        {
            myGui.skill2Tex.Add((Texture2D)(Resources.Load("Skills/" + skill2 + "/3")));
            myGui.skill2TexActive.Add((Texture2D)(Resources.Load("Skills/" + skill2 + "/3")));
            myGui.skill2TexNo.Add((Texture2D)(Resources.Load("Skills/" + skill2 + "/3no")));
            //       Debug.Log("here");
            myGui.skill2Cool.Add(0);
        }        
        go1.SetActive(true);


        string chosen2 = DialogueLua.GetActorField("Player2", "chosen").asString;
    //    Debug.Log(chosen2);
        GameObject go2 = null;
        ThirdPersonUserControl tpu2 = null;
        if (chosen2 != "None")
        {
            Transform pos02 = null;
            if (loadPosBool == true)
            {
                if (transform.Find("Positions/" + loadPosition + "/02") != null)
                {
                    pos02 = transform.Find("Positions/" + loadPosition + "/02");
                }
                else
                {
                    pos02 = transform.Find("Positions/Pos02");
                }
                
            }
            else
            {
                pos02 = transform.Find("Positions/Pos02");
            }
    //        Debug.Log(chosen2);
       //     Debug.Log(loadPosBool);
      //      Debug.Log("Positions/" + loadPosition + "/02");

            go2 = Instantiate(Resources.Load("PC/" + chosen2), pos02.position, pos02.rotation) as GameObject;
            go2.name = chosen2;
            myGui.debugGUI = myGui.debugGUI + "/" + chosen2 + "/";
            tpu2 = go2.GetComponent<ThirdPersonUserControl>();
            int playerID2 = DialogueLua.GetActorField("Player2", "controller").asInt;
            tpu2.playerR = ReInput.players.Players[playerID2];
            int multiID2 = 1;
            gc.player2 = go2;
            gc.players.Add(go2);

            myGui.pic2 = (Texture2D)(Resources.Load("Portraits/" + chosen2));

            string control2 = DialogueLua.GetActorField("Player2", "control").asString;
            //       Debug.Log(control2);
            Player player2R = tpu2.playerR;
            if (control2 == "Mouse&Keyboard")
            {

                player2R.controllers.hasKeyboard = true;
                player2R.controllers.hasMouse = true;
            }
            else
            {
                player2R.controllers.hasKeyboard = false;
                player2R.controllers.hasMouse = false;
            }

            gc.player2.GetComponent<PlayerStats>().internalCNT = 1;
            string skill1P2 = DialogueLua.GetActorField(chosen2, "skill1").asString;
            string skill2P2 = DialogueLua.GetActorField(chosen2, "skill2").asString;

            if (Resources.Load("Skills/" + skill1P2 + "/3"))
            {
                myGui.skill1Tex.Add((Texture2D)(Resources.Load("Skills/" + skill1P2 + "/3")));
                myGui.skill1TexActive.Add((Texture2D)(Resources.Load("Skills/" + skill1P2 + "/3")));
                myGui.skill1TexNo.Add((Texture2D)(Resources.Load("Skills/" + skill1P2 + "/3no")));
                myGui.skill1Cool.Add(0);
            }

            if (Resources.Load("Skills/" + skill2P2 + "/3"))
            {
                myGui.skill2Tex.Add((Texture2D)(Resources.Load("Skills/" + skill2P2 + "/3")));
                myGui.skill2TexActive.Add((Texture2D)(Resources.Load("Skills/" + skill2P2 + "/3")));
                myGui.skill2TexNo.Add((Texture2D)(Resources.Load("Skills/" + skill2P2 + "/3no")));
                myGui.skill2Cool.Add(0);
            }
            go2.SetActive(true);

            if (multiplayer == "Yes")
            {
                go2.GetComponent<PlayerStats>().multiplayerID = multiID2;
                if (multiID == multiID2)
                {
                    characterGO = go2;
                    myGui.pic2 = (Texture2D)(Resources.Load("Portraits/" + chosen2));

                    if (go2.GetComponent<MultiplayerSendInfo>() == null)
                    {
                        go2.AddComponent<MultiplayerSendInfo>();
                    }
                    else
                    {
                        go2.GetComponent<MultiplayerSendInfo>().enabled = true;
                    }
                }
                else
                {
                    go2.GetComponent<ThirdPersonCharacter>().enabled = false;
                    go2.GetComponent<ThirdPersonUserControl>().enabled = false;
                    go2.GetComponent<PlayerAttack>().ghostAvatar = true;
                    EnemyAI ea = go2.GetComponent<EnemyAI>();
                    ea.ghostAvatarBool = true;
                    ea.enabled = false;
                    ea.enabled = true;
                    myGui.pic2 = null;
                }
            }
            if (gc.isRPG == true)
            {
                DialogueLua.SetActorField(chosen2, "inParty", "Yes");
            }

        }
        else
        {
            myGui.pic2 = null;
        }

        string chosen3 = DialogueLua.GetActorField("Player3", "chosen").asString;
        //    Debug.Log(chosen3);
        GameObject go3 = null;
        ThirdPersonUserControl tpu3 = null;
        if (chosen3 != "None")
        {
            Transform pos03;
            if (loadPosBool == true && transform.Find("Positions/" + loadPosition + "/03") != null)
            {
                pos03 = transform.Find("Positions/" + loadPosition + "/03");
            }
            else
            {
                pos03 = transform.Find("Positions/Pos03");
            }
            
            go3 = Instantiate(Resources.Load("PC/" + chosen3), pos03.position, pos03.rotation) as GameObject;
            go3.name = chosen3;
            myGui.debugGUI = myGui.debugGUI + "/" + chosen3 + "/";
            tpu3 = go3.GetComponent<ThirdPersonUserControl>();
            int playerID3 = DialogueLua.GetActorField("Player3", "controller").asInt;
            tpu3.playerR = ReInput.players.Players[playerID3];
            gc.player3 = go3;
            gc.players.Add(go3);


            myGui.pic3 = (Texture2D)(Resources.Load("Portraits/" + DialogueLua.GetActorField("Player3", "chosen").asString));
            int multiID3 = 2;
            if (multiplayer == "Yes")
            {
                go3.GetComponent<PlayerStats>().multiplayerID = multiID3;
                if (multiID == multiID3)
                {
                    characterGO = go3;
                    myGui.pic3 = (Texture2D)(Resources.Load("Portraits/" + chosen3));

                    if (go3.GetComponent<MultiplayerSendInfo>() == null)
                    {
                        go3.AddComponent<MultiplayerSendInfo>();
                    }
                    else
                    {
                        go3.GetComponent<MultiplayerSendInfo>().enabled = true;
                    }
                }
                else
                {
                    go3.GetComponent<ThirdPersonCharacter>().enabled = false;
                    go3.GetComponent<ThirdPersonUserControl>().enabled = false;
                    go3.GetComponent<PlayerAttack>().ghostAvatar = true;
                    if (go3.GetComponent<EnemyAI>() == null)
                    {
                        go3.AddComponent<EnemyAI>();

                    }
                    EnemyAI ea = go3.GetComponent<EnemyAI>();
                    ea.ghostAvatarBool = true;
                    ea.enabled = false;
                    ea.enabled = true;
                    myGui.pic3 = null;
                }
            }
            if (gc.isRPG == true)
            {
                DialogueLua.SetActorField(chosen3, "inParty", "Yes");
            }

            string control3 = DialogueLua.GetActorField("Player3", "control").asString;
            Player player3R = tpu3.playerR;
    //        Debug.Log(control3);
            if (control3 == "Mouse&Keyboard")
            {
                //             Debug.Log("Mouse");


                player3R.controllers.hasKeyboard = true;
                player3R.controllers.hasMouse = true;
            }
            else
            {
                player3R.controllers.hasKeyboard = false;
                player3R.controllers.hasMouse = false;
            }

            gc.player3.GetComponent<PlayerStats>().internalCNT = 2;
            string skill1P3 = DialogueLua.GetActorField(chosen3, "skill1").asString;
            string skill2P3 = DialogueLua.GetActorField(chosen3, "skill2").asString;

            if (Resources.Load("Skills/" + skill1P3 + "/3"))
            {
                myGui.skill1Tex.Add((Texture2D)(Resources.Load("Skills/" + skill1P3 + "/3")));
                myGui.skill1TexActive.Add((Texture2D)(Resources.Load("Skills/" + skill1P3 + "/3")));
                myGui.skill1TexNo.Add((Texture2D)(Resources.Load("Skills/" + skill1P3 + "/3no")));
                myGui.skill1Cool.Add(0);
            }

            if (Resources.Load("Skills/" + skill2P3 + "/3"))
            {
                myGui.skill2Tex.Add((Texture2D)(Resources.Load("Skills/" + skill2P3 + "/3")));
                myGui.skill2TexActive.Add((Texture2D)(Resources.Load("Skills/" + skill2P3 + "/3")));
                myGui.skill2TexNo.Add((Texture2D)(Resources.Load("Skills/" + skill2P3 + "/3no")));
                myGui.skill2Cool.Add(0);
            }
            go3.SetActive(true);
        }
        else
        {
            myGui.pic3 = null;
        }

        string chosen4 = DialogueLua.GetActorField("Player4", "chosen").asString;
        //   Debug.Log(chosen4);
        GameObject go4 = null;
        ThirdPersonUserControl tpu4 = null;
        if (chosen4 != "None")
        {
            Transform pos04;
            if (loadPosBool == true)
            {
                pos04 = transform.Find("Positions/" + loadPosition + "/04");
            }
            else
            {
                pos04 = transform.Find("Positions/Pos04");
            }

            go4 = Instantiate(Resources.Load("PC/" + chosen4), pos04.position, pos04.rotation) as GameObject;
            go4.name = chosen4;
            tpu4 = go2.GetComponent<ThirdPersonUserControl>();
            int playerID4 = DialogueLua.GetActorField("Player4", "controller").asInt;
            tpu4.playerR = ReInput.players.Players[playerID4];
            gc.player4 = go4;
            gc.players.Add(go4);

            myGui.pic4 = (Texture2D)(Resources.Load("Portraits/" + DialogueLua.GetActorField("Player4", "chosen").asString));
            int multiID4 = 3;
            if (multiplayer == "Yes")
            {
                go4.GetComponent<PlayerStats>().multiplayerID = multiID4;
                if (multiID == multiID4)
                {
                    characterGO = go1;
                    myGui.pic4 = (Texture2D)(Resources.Load("Portraits/" + chosen4));
                    if (go4.GetComponent<MultiplayerSendInfo>() == null)
                    {
                        go4.AddComponent<MultiplayerSendInfo>();
                    }
                    else
                    {
                        go4.GetComponent<MultiplayerSendInfo>().enabled = true;
                    }
                }
                else
                {
                    go4.GetComponent<ThirdPersonCharacter>().enabled = false;
                    go4.GetComponent<ThirdPersonUserControl>().enabled = false;
                    go4.GetComponent<PlayerAttack>().ghostAvatar = true;
                    if (go4.GetComponent<EnemyAI>() == null)
                    {
                        go4.AddComponent<EnemyAI>();

                    }
                    EnemyAI ea = go4.GetComponent<EnemyAI>();
                    ea.ghostAvatarBool = true;
                    ea.enabled = false;
                    ea.enabled = true;
                    myGui.pic4 = null;
                }
            }
            if (gc.isRPG == true)
            {
                DialogueLua.SetActorField(chosen4, "inParty", "Yes");
            }

            string control4 = DialogueLua.GetActorField("Player4", "control").asString;
            Player player4R = tpu4.playerR;
            Debug.Log(control4);
            if (control4 == "Mouse&Keyboard")
            {
                //             Debug.Log("Mouse");


                player4R.controllers.hasKeyboard = true;
                player4R.controllers.hasMouse = true;
            }
            else
            {
                player4R.controllers.hasKeyboard = false;
                player4R.controllers.hasMouse = false;
            }

            gc.player4.GetComponent<PlayerStats>().internalCNT = 3;
            string skill1P4 = DialogueLua.GetActorField(chosen4, "skill1").asString;
            string skill2P4 = DialogueLua.GetActorField(chosen4, "skill2").asString;

            if (Resources.Load("Skills/" + skill1P4 + "/3"))
            {
                myGui.skill1Tex.Add((Texture2D)(Resources.Load("Skills/" + skill1P4 + "/3")));
                myGui.skill1TexActive.Add((Texture2D)(Resources.Load("Skills/" + skill1P4 + "/3")));
                myGui.skill1TexNo.Add((Texture2D)(Resources.Load("Skills/" + skill1P4 + "/3no")));
                myGui.skill1Cool.Add(0);
            }

            if (Resources.Load("Skills/" + skill2P4 + "/3"))
            {
                myGui.skill2Tex.Add((Texture2D)(Resources.Load("Skills/" + skill2P4 + "/3")));
                myGui.skill2TexActive.Add((Texture2D)(Resources.Load("Skills/" + skill2P4 + "/3")));
                myGui.skill2TexNo.Add((Texture2D)(Resources.Load("Skills/" + skill2P4 + "/3no")));
                myGui.skill2Cool.Add(0);
            }
            go4.SetActive(true);
        }
        else
        {
            myGui.pic4 = null;
        }


        if (gc.sceneFP == true)
        {
            gc.FPCamera();
            
            if (multiplayer == "Yes" || gc.isRPG== true)
            {
     //           Debug.Log(characterGO.name);
                characterGO = gc.players[multiID];
     //           Debug.Log(characterGO.name);

                tpu = characterGO.GetComponent<ThirdPersonUserControl>();
                tpu.playerR = ReInput.players.Players[0];

                foreach (Camera ca in Camera.allCameras)
                {
                    if (ca.name == "Camera1")
                    {
                        MouseOrbitImproved mo = ca.GetComponent<MouseOrbitImproved>();
                        mo.target = characterGO.transform.Find("Camera");
                        mo.playerR = tpu.playerR;
                        tpu.cam = ca;
                        mo.transform.position = mo.target.position;
                        mo.transform.rotation = mo.target.rotation;
                        tpu.grayReticule = ca.transform.Find("GreyReticle").gameObject;
                        tpu.redReticule = ca.transform.Find("RedReticle").gameObject;
                        mo.enabled = true;
                        myGui.debugGUI = myGui.debugGUI + "/" + ca.name + "/";
                    }
                }
            }
            else
            {
                foreach (Camera ca in Camera.allCameras)
                {
                    if (ca.name == "Camera1")
                    {

                        MouseOrbitImproved mo = ca.GetComponent<MouseOrbitImproved>();
                        mo.target = go1.transform.Find("Camera");
                        mo.playerR = tpu.playerR;
                        tpu.cam = ca;
                        mo.transform.position = mo.target.position;
                        mo.transform.rotation = mo.target.rotation;
                        tpu.grayReticule = ca.transform.Find("GreyReticle").gameObject;
                        tpu.redReticule = ca.transform.Find("RedReticle").gameObject;
                        mo.enabled = true;
                        myGui.debugGUI = myGui.debugGUI + "/" + ca.name + "/";


                    }
                    else if (ca.name == "Camera2")
                    {
                        if (chosen2 != "None")
                        {
                            MouseOrbitImproved mo = ca.GetComponent<MouseOrbitImproved>();
                            mo.target = go2.transform.Find("Camera");
                            mo.playerR = tpu2.playerR;
                            tpu2.cam = ca;
                            mo.transform.position = mo.target.position;
                            mo.transform.rotation = mo.target.rotation;
                            tpu2.grayReticule = ca.transform.Find("GreyReticle").gameObject;
                            tpu2.redReticule = ca.transform.Find("RedReticle").gameObject;
                            mo.enabled = true;
                            myGui.debugGUI = myGui.debugGUI + "/" + ca.name + "/";

                        }
                        else
                        {
                            ca.enabled = false;
                        }
                    }

                    else if (ca.name == "Camera3")
                    {
                        if (chosen3 != "None")
                        {
                            MouseOrbitImproved mo = ca.GetComponent<MouseOrbitImproved>();
                            mo.target = go3.transform.Find("Camera");
                            mo.playerR = tpu3.playerR;
                            tpu3.cam = ca;
                            mo.transform.position = mo.target.position;
                            mo.transform.rotation = mo.target.rotation;
                            tpu3.grayReticule = ca.transform.Find("GreyReticle").gameObject;
                            tpu3.redReticule = ca.transform.Find("RedReticle").gameObject;
                            mo.enabled = true;
                            myGui.debugGUI = myGui.debugGUI + "/" + ca.name + "/";

                        }
                        else
                        {
                            ca.enabled = false;
                        }
                    }

                    else if (ca.name == "Camera4")
                    {
                        if (chosen4 != "None")
                        {
                            MouseOrbitImproved mo = ca.GetComponent<MouseOrbitImproved>();
                            mo.target = go4.transform.Find("Camera");
                            mo.playerR = tpu4.playerR;
                            tpu4.cam = ca;
                            mo.transform.position = mo.target.position;
                            mo.transform.rotation = mo.target.rotation;
                            tpu4.grayReticule = ca.transform.Find("GreyReticle").gameObject;
                            tpu4.redReticule = ca.transform.Find("RedReticle").gameObject;
                            mo.enabled = true;
                            myGui.debugGUI = myGui.debugGUI + "/" + ca.name + "/";
                        }
                        else
                        {
                            ca.enabled = false;
                        }
                    }


                }
            }

    
        }
        else
        {
            foreach (Camera ca in Camera.allCameras)
            {
                if (ca.name == "Camera1")
                {
                    ca.GetComponent<CameraController>().enabled = true;
                }
            }

            gc.hideSide.SetActive(false);
        }

        if (gc.isRPG)
        {
            /*
            foreach (GameObject pa in gc.players)
            {
                pa.GetComponent<ThirdPersonUserControl>().rpgMenu = rpgMenu;
            }*/
     //       Debug.Log(rpgMenu);
     //       Debug.Log(gc.player1);
            rpgMenu.GetComponent<RPGMenuController>().playerR = gc.player1.GetComponent<ThirdPersonUserControl>().playerR;
            GetComponent<myGUI>().rpgMenu = rpgMenu;
            rpgMenu.transform.Find("VirtualCursor").gameObject.SetActive(true);
        }

        ///Evaluate if possble to always asign rpgMenu variable on PlayerStats scripts even though it is not a RPG mode. 
        foreach (GameObject pa in gc.players)
        {
            pa.GetComponent<ThirdPersonUserControl>().rpgMenu = rpgMenu;
        }
        Invoke("AllDone", 1);
    }

    public void LoadAllInventory ()
    {
        string data = DialogueLua.GetVariable("rpgInventoryQuick").asString;
        char[] separators = new char[] { '/' };
        string[] split = data.Split(separators);

        for (int cnt = 0; cnt < split.Length; cnt++)
        {
            char[] separators2 = new char[] { '*' };
            string[] split2 = split[cnt].Split(separators2);

    //        Debug.Log(split2[0] + "/" +  split2[1] + "/" + split2[2]);           
            
            int slotPos = Convert.ToInt32(split2[0]);
            int qt = Convert.ToInt32(split2[1]);
            string itemToAdd = split2[2];
            //        Debug.Log(itemToAdd + "/" + qt);
            //        DialogueLua.SetActorField("QuickInventory", itemToAdd, qt);
            gc.quickInventoryRPG.Add(new InventoryRPG(itemToAdd, qt, slotPos));
        }

        string dataSlots = DialogueLua.GetVariable("rpgInventory").asString;
        //         Debug.Log("datasSlots:" + dataSlots);
        if (dataSlots != "None" && dataSlots != "" && dataSlots != "nill")
        {
            char[] separatorsInv = new char[] { '/' };
            string[] splitInv = dataSlots.Split(separatorsInv);

            for (int cnt = 0; cnt < splitInv.Length; cnt++)
            {
                char[] separators2 = new char[] { '*' };
                string[] split2 = splitInv[cnt].Split(separators2);
  
                int qt = Convert.ToInt32(split2[1]);
                string itemToAdd = split2[2];
                int slotPos = Convert.ToInt32(split2[0]);
   //             DialogueLua.SetActorField("QuickInventory", itemToAdd, qt);
                gc.inventoryRPG.Add(new InventoryRPG(itemToAdd, qt, slotPos));
            }
        }

        
        string dataBooks = DialogueLua.GetVariable("books").asString;
   //     dataBooks = "1";
        Debug.Log(dataBooks);
        if (dataBooks != "None" && dataBooks != "" && dataBooks != "nill")
        {
            char[] separatorsBooks = new char[] { '/' };
            string[] splitBooks = dataBooks.Split(separatorsBooks);


            for (int cnt = 0; cnt < splitBooks.Length; cnt++)
            {
                char[] separators2 = new char[] { '*' };
                string[] split2 = splitBooks[cnt].Split(separators2);
                foreach (string nm in split2)
                {
                    Debug.Log(nm);
                }
                int qt = Convert.ToInt32(split2[1]);
                string itemToAdd = split2[0];
                int slotPos = Convert.ToInt32(split2[2]);
                Debug.Log(itemToAdd + "/" + qt);
                //        DialogueLua.SetActorField("QuickInventory", itemToAdd, qt);
                gc.books.Add(new InventoryRPG(itemToAdd, qt, slotPos));
            }
        }



        LoadQuickInventory();
        LoadInventory();
    //    Debug.Log("Load Books");
        LoadBooks();
        rpgMenu.gameObject.SetActive(false);
        rpgMenu.transform.Find("VirtualCursor").gameObject.SetActive(false);
        rpgMenu.transform.Find("Camera").gameObject.SetActive(false);
    }

    public void LoadQuickInventory ()
    {             

        if (gc.quickInventoryRPG.Count > 0)
        {
            Transform quickSlots = rpgMenu.transform.Find("Plane/Menu/Active/Inventory/QuickSlots");

            for (int cnt = 0; cnt < gc.quickInventoryRPG.Count; cnt++)
            {
                Transform ta = quickSlots.transform.Find(gc.quickInventoryRPG[cnt].slot.ToString());
                if (ta.transform.childCount == 0)
                {
           //         Debug.Log(gc.quickInventoryRPG[cnt].name);
                    if (gc.quickInventoryRPG[cnt].name == "None")
                    {
                        GameObject go = new GameObject();
                        go.transform.position = ta.position;
                        go.transform.rotation = ta.rotation;
                        go.name = "None";
                        go.transform.parent = ta;
                    }
                    else
                    {
                        GameObject go = Instantiate(Resources.Load("Inventory/RPG/" + gc.quickInventoryRPG[cnt].name), ta.position, ta.rotation) as GameObject;
                        go.name = gc.quickInventoryRPG[cnt].name;
                        go.transform.parent = ta;
                    }

                }
                else
                {
                    if (ta.transform.GetChild(0).name != gc.quickInventoryRPG[cnt].name)
                    {
                        Transform toBin = ta.transform.GetChild(0);
                        toBin.parent = null;
                        toBin.gameObject.SetActive(false);
                        GameObject go = Instantiate(Resources.Load("Inventory/RPG/" + gc.quickInventoryRPG[cnt].name), ta.position, ta.rotation) as GameObject;
                        go.name = gc.quickInventoryRPG[cnt].name;
                        go.transform.parent = ta;
                    }
                }
                                

                if (gc.quickInventoryRPG[cnt].qt > 1)
                {
                    if (ta.transform.childCount == 1)
                    {
                        GameObject go2 = Instantiate(Resources.Load("Inventory/RPG/Qt"), ta.position, ta.rotation) as GameObject;
                        go2.transform.Find("Qt/Canvas/Text").gameObject.GetComponent<Text>().text = gc.quickInventoryRPG[cnt].qt.ToString();
                        go2.name = "Qt";
                        go2.transform.parent = ta;
                    }
                    else
                    {
                        if (ta.transform.GetChild (1).name == "Qt")
                        {
                            if (ta.transform.GetChild(1).gameObject.GetComponent<Text>() == null)
                            {
                                Debug.Log(cnt + "/null");
                            }
                            else
                            {
                                Debug.Log(cnt);
                                if (gc.quickInventoryRPG[cnt] != null)
                                {
                                    ta.transform.GetChild(1).gameObject.GetComponent<Text>().text = gc.quickInventoryRPG[cnt].qt.ToString();
                                }
                                else
                                {
                                    ta.transform.GetChild(1).gameObject.GetComponent<Text>().text = "";
                                }
                                
                            }
                            
                        }
                        else
                        {
                            Debug.Log(cnt);
                            Transform toBin = ta.transform.GetChild(1);
                            toBin.parent = null;
                            toBin.gameObject.SetActive(false);

                            GameObject go2 = Instantiate(Resources.Load("Inventory/RPG/Qt"), ta.position, ta.rotation) as GameObject;
                            go2.transform.Find("Qt/Canvas/Text").gameObject.GetComponent<Text>().text = gc.quickInventoryRPG[cnt].qt.ToString();
                            go2.name = "Qt";
                            go2.transform.parent = ta;
                        }
                    }

                }
            }

            int quickSlotsLeft = 4 - gc.quickInventoryRPG.Count;
            //       Debug.Log(quickSlotsLeft);
            if (quickSlotsLeft > 0)
            {
             
                for (int cnt = gc.quickInventoryRPG.Count - 1; cnt < (gc.quickInventoryRPG.Count + quickSlotsLeft); cnt++)
                {
         //           Debug.Log(cnt);
                    Transform ta = quickSlots.transform.Find(cnt.ToString());
                    GameObject go = new GameObject();
                    go.transform.position = ta.position;
                    go.transform.rotation = ta.rotation;
                    go.name = "None";
                    go.transform.parent = ta;
                }
            }

        }






    }

    public void LoadInventory ()
    {
        Transform slots = rpgMenu.transform.Find("Plane/Menu/Active/Inventory/Slots");
        for (int cnt = 0; cnt < gc.inventoryRPG.Count; cnt++)
        {
            Transform ta = slots.transform.Find(gc.inventoryRPG[cnt].slot.ToString());
            if (ta.transform.childCount == 0)
            {
                GameObject go = Instantiate(Resources.Load("Inventory/RPG/" + gc.inventoryRPG[cnt].name), ta.position, ta.rotation) as GameObject;
                go.name = gc.inventoryRPG[cnt].name;
                go.transform.parent = ta;
            }
            else
            {
                if (ta.transform.GetChild(0).name != gc.inventoryRPG[cnt].name)
                {
                    Transform toBin = ta.transform.GetChild(0);
                    toBin.parent = null;
                    toBin.gameObject.SetActive(false);
                    GameObject go = Instantiate(Resources.Load("Inventory/RPG/" + gc.inventoryRPG[cnt].name), ta.position, ta.rotation) as GameObject;
                    go.name = gc.inventoryRPG[cnt].name;
                    go.transform.parent = ta;
                }
            }


            if (gc.inventoryRPG[cnt].qt > 1)
            {
                if (ta.transform.Find ("Qt") == null)
                {
                    GameObject go2 = Instantiate(Resources.Load("Inventory/RPG/Qt"), ta.position, ta.rotation) as GameObject;
                    go2.transform.Find("Qt/Canvas/Text").gameObject.GetComponent<Text>().text = gc.inventoryRPG[cnt].qt.ToString();
                    go2.name = "Qt";
                    go2.transform.parent = ta;
                }
                else
                {
                    if (ta.transform.GetChild(1).name == "Qt")
                    {
                        ta.transform.GetChild(1).gameObject.GetComponent<Text>().text = gc.inventoryRPG[cnt].qt.ToString();
                    }
                    else
                    {
                        Transform toBin = ta.transform.GetChild(1);
                        toBin.parent = null;
                        toBin.gameObject.SetActive(false);

                        GameObject go2 = Instantiate(Resources.Load("Inventory/RPG/Qt"), ta.position, ta.rotation) as GameObject;
                        go2.transform.Find("Qt/Canvas/Text").gameObject.GetComponent<Text>().text = gc.inventoryRPG[cnt].qt.ToString();
                        go2.name = "Qt";
                        go2.transform.parent = ta;
                    }
                }

            }
            else if (gc.inventoryRPG[cnt].qt == 0)
            {
                if (ta.transform.Find("Qt") != null)
                {

                    Transform tempTa = ta.transform.Find("Qt");
                    tempTa.parent = null;
                    tempTa.gameObject.SetActive(false);
                }
            }
        }

        int emptySlots = 72 - gc.inventoryRPG.Count;
        if (emptySlots > 0)
        {
            for (int cnt = gc.inventoryRPG.Count; cnt < emptySlots; cnt++)
            {
       //         Debug.Log(cnt);
                Transform ta = slots.transform.Find(cnt.ToString());
                GameObject go = new GameObject();
                go.transform.position = ta.position;
                go.transform.rotation = ta.rotation;
                go.name = "None";
                go.transform.parent = ta;

                /*
                GameObject go2 = Instantiate(Resources.Load("Inventory/RPG/Qt"), ta.position, ta.rotation) as GameObject;
                go2.transform.Find("Qt/Canvas/Text").gameObject.GetComponent<UnityEngine.UI.Text>().text = "";
                go2.name = "Qt";
                go2.transform.parent = ta;*/

            }
        }


    }

    public void LoadBooks ()
    {
        for (int cnt = 0; cnt < gc.books.Count; cnt ++)
        {
            Debug.Log(gc.books[cnt].name);
        }
        if (gc.books.Count > 0)
        {
            Transform slots = rpgMenu.transform.Find("Plane/Menu/Active/Other_Items/Active/Books/Slots");
            Debug.Log(gc.books.Count );
            for (int cnt = 0; cnt < gc.books.Count; cnt++)
            {
                Debug.Log(cnt + "/" + gc.books[cnt].name);
                Transform ta = slots.transform.Find(cnt.ToString());
                if (ta.transform.childCount == 0)
                {
                    GameObject go = Instantiate(Resources.Load("Inventory/Books/" + gc.books[cnt].name), ta.position, ta.rotation) as GameObject;
                    go.name = gc.books[cnt].name;
                    go.transform.parent = ta;
                    rpgMenu.GetComponent<RPGMenuController>().GetBookDescription(gc.books[cnt].name);
                }
                else
                {
                    if (ta.transform.GetChild(0).name != gc.books[cnt].name)
                    {
                        Transform toBin = ta.transform.GetChild(0);
                        toBin.parent = null;
                        toBin.gameObject.SetActive(false);
                        GameObject go = Instantiate(Resources.Load("Inventory/Books/" + gc.books[cnt].name), ta.position, ta.rotation) as GameObject;
                        go.name = gc.books[cnt].name;
                        go.transform.parent = ta;
                    }
                }     
            }

            
        }
    }

    private void ForTestingOnly ()
    {
        /*
        foreach (GameObject go in gc.players)
        {
            if (go.name == "Balder")
            {
                go.GetComponent<PlayerStats>().AddjustHealth(-10, go, false);
            }
            else
            {
                go.GetComponent<PlayerStats>().AddjustMana(-20, go);
            }
        }*/
    }

    private void CallPortrait ()
    {
 //       Debug.Log("Portrait");
        
   //     var bob = DialogueManager.MasterDatabase.GetActor("Player1");
     //   var pic2 = bob.alternatePortraits[0];

        //     DialogueManager.SetPortrait("Player1", "pic=2");
        DialogueManager.SetPortrait("Player1", "Portraits/Nanna");
        DialogueManager.SendUpdateTracker();
        //     Invoke("CallPortrait", 0.1f);
       

    }

    private void AllDone ()
    {
        gc.allDone = true;
    }

    //From here on it is used after Loading, in game

    public void AddOneItem (string itemToAdd)
    {
        bool existItem = false;
        int cntExist = 10000;
        for (int cnt = 0; cnt < gc.inventoryRPG.Count; cnt++)
        {
            if (gc.inventoryRPG[cnt].name == itemToAdd)
            {
                cntExist = cnt;
                break;
            }

            Debug.Log(cnt + "/" + gc.inventoryRPG[cnt].name);
        }

        if (existItem == false)
        {
            for (int cnt = 0; cnt < gc.inventoryRPG.Count; cnt++)
            {
                if (gc.inventoryRPG[cnt].name == "None")
                {
                    Transform slots = rpgMenu.transform.Find("Plane/Menu/Active/Inventory/Slots");
                    Transform ta = slots.transform.Find(gc.inventoryRPG[cnt].slot.ToString());
                    gc.inventoryRPG[cnt].name = itemToAdd;

                    if (ta.transform.childCount == 0)
                    {
                        GameObject go = Instantiate(Resources.Load("Inventory/RPG/" + itemToAdd), ta.position, ta.rotation) as GameObject;
                        go.name = itemToAdd;                        
                        go.transform.parent = ta;
                    }
                    else
                    {
                        if (ta.transform.GetChild(0).name != gc.inventoryRPG[cnt].name)
                        {
                            Transform toBin = ta.transform.GetChild(0);
                            toBin.parent = null;
                            toBin.gameObject.SetActive(false);
                            GameObject go = Instantiate(Resources.Load("Inventory/RPG/" + itemToAdd), ta.position, ta.rotation) as GameObject;
                            go.name = itemToAdd;
                            go.transform.parent = ta;
                        }
                    }

                    break;
                }
            }
        }
        else
        {
            gc.inventoryRPG[cntExist].qt++;
            GetComponent<LoadGameImp>().LoadInventory();
        }
    }
}



