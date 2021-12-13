using UnityEngine;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Rewired;


public class LoadGame : MonoBehaviour {
    public bool loadWorld = true;
    public bool questLoaded = false;
    public bool gameLoaded = false;

    public List<string> controllers = new List<string>();
    private GameController gc;
    private string saveFileName;            //Use this to import data for a game already in course
    private string fileDirectory;
    private string profile;
    private myGUI myGui;
    private bool waitForPlayers = false;

    private void Awake()
    {
        // Subscribe to events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnect;

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
        Time.timeScale = 1;
        gc = GetComponent<GameController>();
        myGui = GetComponent<myGUI>();
        LoadCurrentProfile();
        CallControllers();
    }

    public void LoadCurrentProfile()
    {
              
        string path = Application.persistentDataPath + "/DandU";
        string fileName = path + "/CurrentProfile.dat";
    //    myGui.debugGUI = myGui.debugGUI + "/LoadCurrentProfile";
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

        
    }



    public void LoadQuests()
    {
        string path = Application.persistentDataPath + "/DandU";
        string fileName = path + "/" + profile + "/" + fileDirectory + ".dat";
   //     myGui.debugGUI = myGui.debugGUI + "/" + fileDirectory;
        if (File.Exists(fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(fileName, FileMode.Open);
            SaveQuest data = (SaveQuest)bf.Deserialize(file);
            file.Close();
            string saveData = data.saveData;

            PersistentDataManager.ApplySaveData(saveData);
      //      Debug.Log(saveData);
            //    		DialogueManager.Instance.GetComponent<LevelManager>().LoadGame(saveData);

            if (loadWorld == true)
            {
                Invoke("LoadWorld", 0.1f);
            }
            else
            {
                Invoke ("Test", 0.1f);
            }

            Invoke("QuestLoaded", 0.01f);
        }
    }

    private void LoadWorld ()
    {

        int playerNos = ReInput.players.Players.Count;
        int joystickNos = ReInput.controllers.Joysticks.Count;
        Debug.Log(playerNos);

        if (joystickNos > playerNos)
        {
            waitForPlayers = true;
        }


        string sceneName = SceneManager.GetActiveScene().name;
        string languageOnLoad = DialogueLua.GetVariable("language").AsString;
        string isSceneFP = DialogueLua.GetVariable("sceneFP").asString;



        if (isSceneFP == "Yes")
        {
            gc.sceneFP = true;
        }
        //   gc.StartServer();
        //    Debug.Log(languageOnLoad);
        DialogueManager.SetLanguage(languageOnLoad);
        DialogueLua.SetVariable("sceneCurrent", sceneName);



        Transform pos01 = transform.Find("Positions/Pos01");
        string chosen1 = DialogueLua.GetActorField("Player1", "chosen").asString;
        Debug.Log(chosen1);
        gc.player1 = Instantiate(Resources.Load("PC/" + chosen1), pos01.position, pos01.rotation) as GameObject;
        gc.player1.name = chosen1;
        //   Debug.Log(chosen1 + "/" + ReInput.players.GetPlayer(0).name);
        gc.player1.GetComponent<PlayerStats>().gc = GetComponent<GameController>();
        gc.player1.GetComponent<PlayerStats>().player = "Player1";
        string control1 = DialogueLua.GetActorField("Player1", "control").AsString;

        

        int controllerNo = 0;
        if (control1 == "Mouse&Keyboard")
        {
            gc.player1.GetComponent<ThirdPersonUserControl>().controller = control1;

        }
        else
        {
            int tempInt = DialogueLua.GetActorField("Player1", "controller").asInt;
            Debug.Log(tempInt);
            gc.player1.GetComponent<ThirdPersonUserControl>().playerR = ReInput.players.Players[0];
  //          myGui.debugGUI = myGui.debugGUI + "/con1: " + tempInt + ReInput.players.Players[0].name;

        }
        gc.player1.SetActive(true);
        gc.players.Add(gc.player1);
        myGui.pic1 = (Texture2D)(Resources.Load("Portraits/" + chosen1));
        //   Debug.Log(chosen1 + "/" + gc.player1.name);
        

        Transform pos02 = transform.Find("Positions/Pos02");
        string chosen2 = DialogueLua.GetActorField("Player2", "chosen").asString;
       

        if (chosen2 != "None")
        {
            gc.player2 = Instantiate(Resources.Load("PC/" + chosen2), pos02.position, pos02.rotation) as GameObject;
            gc.player2.name = chosen2;
            gc.player2.GetComponent<PlayerStats>().gc = GetComponent<GameController>();
            gc.player2.GetComponent<PlayerStats>().player = "Player2";
            string control2 = DialogueLua.GetActorField("Player2", "control").AsString;
            Debug.Log(control2);

            myGui.debugGUI = myGui.debugGUI + "/" + control2;
            gc.player2.GetComponent<ThirdPersonUserControl>().controller = controllers[1];
            int tempInt = DialogueLua.GetActorField("Player2", "controller").asInt;
            Debug.Log(tempInt);
            myGui.debugGUI = myGui.debugGUI + "/RPlayer: " + (ReInput.players.Players[1].name);
            gc.player2.GetComponent<ThirdPersonUserControl>().playerR = ReInput.players.Players[1];

            gc.player2.SetActive(true);
            gc.players.Add(gc.player2);
            //        Debug.Log(DialogueLua.GetActorField("Player2", "control").asString);
            myGui.pic2 = (Texture2D)(Resources.Load("Portraits/" + DialogueLua.GetActorField("Player2", "chosen").asString));





            //    GetComponent<Scores>().playerPuppet = gc.player2;
        }
        else
        {
            myGui.pic2 = null;

        }

        Transform pos03 = transform.Find("Positions/Pos03");
        string chosen3 = DialogueLua.GetActorField("Player3", "chosen").asString;
        //    Debug.Log(chosen3);


        if (chosen3 != "None")
        {
            gc.player3 = Instantiate(Resources.Load("PC/" + chosen3), pos03.position, pos03.rotation) as GameObject;
            gc.player3.name = chosen3;
            gc.player3.GetComponent<PlayerStats>().gc = GetComponent<GameController>();
            gc.player3.GetComponent<PlayerStats>().player = "Player3";
            gc.player3.SetActive(true);

            string control3 = DialogueLua.GetActorField("Player3", "control").AsString;
            //      Debug.Log(control3);
            if (control3 == "mobile")
            {

                string ip3 = DialogueLua.GetActorField("Player3", "ip").AsString;
                Debug.Log(ip3);
                Scores scores2 = GetComponent<Scores>();
                scores2.tpc3 = gc.player3.GetComponent<ThirdPersonUserControl>();
                scores2.ip3 = ip3;
                scores2.tpc3.mobileControl = true;
                gc.player3.GetComponent<PlayerStats>().mobileController = true;

            }
            else if (control3 == "Mouse&Keyboard")
            {
                gc.player3.GetComponent<ThirdPersonUserControl>().controller = control3;
            }
            else
            {

                gc.player3.GetComponent<ThirdPersonUserControl>().controller = controllers[controllerNo];
            }

            gc.players.Add(gc.player3);
            myGui.pic3 = (Texture2D)(Resources.Load("Portraits/" + DialogueLua.GetActorField("Player3", "chosen").asString));



        }
        else
        {
            myGui.pic3 = null;

        }

        Transform pos04 = transform.Find("Positions/Pos04");
        string chosen4 = DialogueLua.GetActorField("Player4", "chosen").asString;

        if (DialogueLua.GetActorField("Player4", "chosen").asString != "None")
        {
            gc.player4 = Instantiate(Resources.Load("PC/" + chosen4), pos04.position, pos04.rotation) as GameObject;
            gc.player4.name = chosen4;
            gc.player4.GetComponent<PlayerStats>().gc = GetComponent<GameController>();
            gc.player4.GetComponent<PlayerStats>().player = "Player4";
            gc.player4.SetActive(true);
            gc.players.Add(gc.player4);
            myGui.pic4 = (Texture2D)(Resources.Load("Portraits/" + DialogueLua.GetActorField("Player4", "chosen").asString));

            string control4 = DialogueLua.GetActorField("Player4", "control").AsString;
            if (control4 == "mobile")
            {

                string ip4 = DialogueLua.GetActorField("Player4", "ip").AsString;
                Debug.Log(ip4);
                Scores scores2 = GetComponent<Scores>();
                scores2.tpc4 = gc.player4.GetComponent<ThirdPersonUserControl>();
                scores2.ip4 = ip4;
                scores2.tpc4.mobileControl = true;
                gc.player4.GetComponent<PlayerStats>().mobileController = true;

            }
            else if (control4 == "Mouse&Keyboard")
            {
                gc.player4.GetComponent<ThirdPersonUserControl>().controller = control4;
            }
            else
            {

                gc.player4.GetComponent<ThirdPersonUserControl>().controller = controllers[controllerNo];
            }

            gc.players.Add(gc.player3);
            myGui.pic3 = (Texture2D)(Resources.Load("Portraits/" + DialogueLua.GetActorField("Player3", "chosen").asString));

        }
        else
        {
            myGui.pic4 = null;

        }

        Scene scene = SceneManager.GetActiveScene();

        if (gc.sceneFP == true || scene.name == "ArenaCross")
        {
            gc.FPCamera();

            if (scene.name == "ArenaCross")
            {
                gc.arenaMode = true;
            }

            Invoke("DelayCameras", 0.1f);
            Invoke("CheckCameras", 1f);

      
            if (gc.arenaMode == true)
            {
                foreach (GameObject go in gc.players)
                {
                    if (go.name == "Fred")
                    {
                        ThirdPersonCharacter tpc = go.GetComponent<ThirdPersonCharacter>();
                        tpc.m_AnimSpeedMultiplier = 1.3f;
                        tpc.m_MoveSpeedMultiplier = 3;
                        tpc.m_StationaryTurnSpeed = 60;
                    }
                    else if (go.name == "Oleg")
                    {
                        ThirdPersonCharacter tpc = go.GetComponent<ThirdPersonCharacter>();
                        tpc.m_AnimSpeedMultiplier = 1;
                        tpc.m_MoveSpeedMultiplier = 1.4f;
                        tpc.m_StationaryTurnSpeed = 60;

                    }
                    else if (go.name == "Rose")
                    {
                        ThirdPersonCharacter tpc = go.GetComponent<ThirdPersonCharacter>();
                        tpc.m_AnimSpeedMultiplier = 1.2f;
                        tpc.m_MoveSpeedMultiplier = 1.8f;
                        tpc.m_StationaryTurnSpeed = 60;
                    }
                }
            }


        }
        else
        {
            foreach (Camera ca in Camera.allCameras)
            {
                if (ca.name != "Camera1")
                {
                    ca.enabled = false;
                }
                else
                {
                    if (ca.enabled == false)
                    {
                        ca.enabled = true;
                    }

                    if (ca.GetComponent<CameraController>().enabled == false)
                    {
                        ca.GetComponent<CameraController>().enabled = true;
                    }

                }
            }

            if (gc.hideSide != null)
            {
                gc.hideSide.SetActive(false);
            }
        }

        myGui.debugGUI = myGui.debugGUI + "/chosen1:" + chosen1 + "/chosen2 " + chosen2;

        myGui.enabled = true;



    }

    private void LoadControllers ()
    {

    }


    private void QuestLoaded ()
    {
        questLoaded = true;
    }
    
    private void Test ()
    {
        
        string sceneName = SceneManager.GetActiveScene().name;
        string languageOnLoad = DialogueLua.GetVariable("language").AsString;
        string isSceneFP = DialogueLua.GetVariable("sceneFP").asString;
   //     Debug.Log(isSceneFP + "/" + DialogueLua.GetVariable("arenaMode").asString +  "/" + DialogueLua.GetVariable("test").asString);
        gameLoaded = true;
    }

    private void DelayCameras ()
    {
        string totalCams = "";
        foreach (Camera ca in Camera.allCameras)
        {
            
            //         Debug.Log(ca.name);
            if (ca.name == "Camera1")
            {
                SetUpCamera(ca, gc.player1);
                if (ca.enabled == false)
                {
                    ca.enabled = true;
                }
                totalCams = totalCams + "/" + ca.name;
            }
            else if (ca.name == "Camera2")
            {
                if (gc.player2 != null)
                {
                    SetUpCamera(ca, gc.player2);
                    if (ca.enabled == false)
                    {
                        ca.enabled = true;
                    }
                }
                else
                {
                    ca.enabled = false;
                }

                totalCams = totalCams + "/" + ca.name;
            }
            else if (ca.name == "Camera3")
            {
                if (gc.player3 != null)
                {
                    SetUpCamera(ca, gc.player3);
                    if (ca.enabled == false)
                    {
                        ca.enabled = true;
                    }
                }
                else
                {
                    ca.enabled = false;
                }

                totalCams = totalCams + "/" + ca.name;
            }
            else if (ca.name == "Camera4")
            {
                if (gc.player4 != null)
                {
                    SetUpCamera(ca, gc.player4);
                    if (ca.enabled == false)
                    {
                        ca.enabled = true;
                    }
                }
                else
                {
                    ca.enabled = false;
                }

                totalCams = totalCams + "/" + ca.name;
            }
        }

        DialogueManager.ShowAlert(totalCams);

    }

    private void SetUpCamera (Camera ca, GameObject tempPlayer)
    {

    //    Debug.Log(ca.name + "/" + tempPlayer);
        ThirdPersonUserControl tpu = tempPlayer.GetComponent<ThirdPersonUserControl>();
        tpu.cam = ca;
        tpu.redReticule = ca.transform.Find("RedReticle").gameObject;
        tpu.grayReticule = ca.transform.Find("GreyReticle").gameObject;

        Transform taGO = tempPlayer.transform.Find("Camera");
        ca.transform.position = taGO.position;
        ca.transform.rotation = taGO.rotation;
        //      ca.transform.parent = taGO;
        MouseOrbitImproved mo = ca.GetComponent<MouseOrbitImproved>();
        mo.target = taGO;
        tpu.LoadLua();
        
        mo.axisX = tpu.axisX;
        mo.axisY = tpu.axisY;
        mo.enabled = true;
        tempPlayer.GetComponent<PlayerAttack>().cam = ca;
        tempPlayer.GetComponent<PlayerAttack>().mouseOr = ca.GetComponent<MouseOrbitImproved>();
    }

    private void CheckCameras ()
    {
        bool allOK = true;
        foreach (GameObject go in gc.players)
        {
            ThirdPersonUserControl tpu = go.GetComponent<ThirdPersonUserControl>();
            
            if (tpu.cam != null)
            {
                MouseOrbitImproved mo = tpu.cam.GetComponent<MouseOrbitImproved>();
                if (tpu.cam.GetComponent<MouseOrbitImproved>().target != go.transform.Find("Camera"))
                {
                    Transform taGO = go.transform.Find("Camera");
                    tpu.cam.transform.position = taGO.position;
                    tpu.cam.transform.rotation = taGO.rotation;
                    //      ca.transform.parent = taGO;

                    mo.target = go.transform.Find("Camera");
                    allOK = false;
                }
            }
            else
            {

                myGui.debugGUI = myGui.debugGUI + "/cam&Go :" + go.name + " No cam";
            }

            
           
        }
        DelayCameras();
        Invoke("CheckCameras", 0.3f);

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
         //               Debug.Log("joystick " + tempCNT.ToString());           

                    }
                }
            }


        }
        else
        {
            Debug.Log("null");
        }


    }

}



