using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using PixelCrushers.DialogueSystem;
using Rewired;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;

public class RPGMenuController : MonoBehaviour
{

    public string activeMenu;
    public Player playerR;
    private bool loaded = false;
    private bool alive = true;
    private string activePC;
    private string profile;
    private string sceneToLoad;
    private string slotNo;
    private float timer;
    private List<string> characters = new List<string>();
    private List<string> savedGames = new List<string>();
    private List<string> dateSavedGames = new List<string>();
    private GameController gc;
    private LoadGameImp loadgame;
    private GameObject chars;
    private GameObject PC;
    private Transform virtualCursor;
    private Transform tranToCarry;
    private Transform qtToCarry;
    private Transform originBeforeCarry;
    private Transform saveSlotsTa;
    private Transform optionsTra;
    private Transform otherItemsSlots;
    private Transform questPositions;
    private GameObject descriptionGO;
    private Text questDescription;
    private GameObject warningGo;
    private GameObject cam;
    private GameObject camRPG;
    private GameObject gcon;
    private GameObject autosaveText;
    private GameObject activeButton;
    private Collider invCollider;
    private AudioSource audio;
    private AudioSource audioSecond;
    public enum State
    {
        Idle,
        Carrying,
        Map,
        Confirm             //confirm Overwrite save file or load saved game
    }
    public State state;


    private void Awake()
    {
        gameObject.tag = "RPGMenu";
        virtualCursor = transform.Find("VirtualCursor");
        virtualCursor.gameObject.SetActive(false);
        gcon = GameObject.FindGameObjectWithTag("GameController");
        gcon.GetComponent<LoadGameImp>().rpgMenu = gameObject;
       

    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void OnEnable ()
    {
        if (loaded == false)
        {
            loaded = true;
            gameObject.tag = "RPGMenu";
            
            gc = gcon.GetComponent<GameController>();
            loadgame = gcon.GetComponent<LoadGameImp>();
            chars = transform.Find("Plane/PC/Chars").gameObject;
            PC = transform.Find("Plane/PC").gameObject;
            virtualCursor = transform.Find("VirtualCursor");
            state = State.Idle;
            StartCoroutine("FSM");
            cam = transform.Find("Camera").gameObject;
            camRPG = gcon.transform.Find("CameraMap").gameObject;
     //       playerR = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().player1.GetComponent<ThirdPersonUserControl>().playerR;
            invCollider = transform.Find("Plane/Menu/Active/Inventory").gameObject.GetComponent<BoxCollider>();
            audio = GetComponent<AudioSource>();
            audioSecond = virtualCursor.transform.Find ("Sword").GetComponent<AudioSource>();
            saveSlotsTa = transform.Find("Plane/Menu/Active/Options/Canvas");
            autosaveText = transform.Find("Plane/Menu/Active/Options/Active/LoadActive/Canvas/Autosave_label").gameObject;
            optionsTra = transform.Find("Plane/Menu/Active/Options");
            warningGo = transform.Find("Plane/Menu/Active/Options/WarningBox").gameObject;
            questPositions = transform.Find("Plane/Menu/Active/Quests/QuestPositions");
            descriptionGO = transform.Find("Plane/Menu/Active/Quests/Canvas/Description").gameObject;
            otherItemsSlots = transform.Find("Plane/Menu/Active/Other_Items/Active/Books/Slots");
            questDescription = descriptionGO.GetComponent<Text>();

            Transform taActiveButton = questPositions.transform.Find("0");
            activeButton = Instantiate(Resources.Load("GUI/RPG/ActiveButton")) as GameObject;
            activeButton.transform.position = taActiveButton.position;
            activeButton.transform.parent = questPositions;
            activeButton.SetActive(false);
        }

        if (gc.activePlayer != null)
        {
       
            activePC = gc.activePlayer.name;
    //        Debug.Log(activePC);

            foreach (GameObject go in gc.players)
            {
                characters.Add(go.name);
            }
            foreach (Transform ta in chars.transform)
            {
                if (ta.name != activePC)
                {
                    ta.gameObject.SetActive(false);
                }
            }

            if (Rewired.ReInput.controllers.Joysticks.Count > 0)
            {
                Cursor.visible = false;
            }
            GetSavedGames();
            state = State.Idle;
            StartCoroutine("FSM");
            Time.timeScale = 0;
            playerR = gc.activePlayer.GetComponent<ThirdPersonUserControl>().playerR;
         
        }
        timer = Time.realtimeSinceStartup + 0.5f;
   

    }


    private IEnumerator FSM()
    {
        while (alive)
        {
            switch (state)
            {
                case State.Idle:

                    yield return new WaitForSecondsRealtime(1f);
                    break;

                case State.Carrying:
                    Carrying();
                    yield return new WaitForSecondsRealtime(0.02f);
                    break;

                case State.Map:
                    Map();
                    yield return new WaitForSecondsRealtime(0);
                    break;

                case State.Confirm:
                   
                    yield return new WaitForSecondsRealtime(0.2f);
                    break;
            }
        }
        yield return null;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }


    public void ColliderReceived (Collider col, bool leftButton)
    {
   //     Debug.Log(col.name + "/" + col.transform.parent.name + "/" + leftButton);
  //      Debug.Log(state + "/" + col.name);

        if (leftButton)
        {
            if (col.tag == "Head" && state != State.Confirm)
            {
                ChangeActiveHead(col);
            }
            else if (col.name == "Right" && state != State.Confirm)
            {
                ChangeChar(true);
            }
            else if (col.name == "Left" && state != State.Confirm)
            {
                ChangeChar(false);
            }
            else if (col.name == "Autosave" && state != State.Confirm)
            {
                Debug.Log("LoadGame");
                slotNo = "AutoSave";
                LoadGame();
            }
            else if (col.name == "Cancel" && state == State.Confirm)
            {
                audio.Play();
                Debug.Log("Cancel");
                state = State.Idle;
                optionsTra.Find("WarningBox").gameObject.SetActive(false);                
                optionsTra.Find("SaveWarning").gameObject.SetActive(false);
                optionsTra.Find("Overwrite").gameObject.SetActive(false);
                optionsTra.Find("Canvas/Overwrite").gameObject.SetActive(false);
                optionsTra.Find("Canvas/OverwriteLabel").gameObject.SetActive(false);
                optionsTra.Find("Canvas/Cancel").gameObject.SetActive(false);
                optionsTra.Find("Canvas/Labels").gameObject.SetActive(true);
                optionsTra.Find("LoadScene").gameObject.SetActive(false);
                optionsTra.Find("Cancel").gameObject.SetActive(false);
                optionsTra.Find("Canvas/Load?").gameObject.SetActive(false);
                optionsTra.Find("Canvas/LoadLabel").gameObject.SetActive(false);
                optionsTra.Find("Canvas/Cancel").gameObject.SetActive(false);
                optionsTra.Find("Canvas/Labels").gameObject.SetActive(true);
            }
            else if (col.transform.parent.name == "SaveSlots" && state != State.Confirm)
            {
                Transform parentTa = col.transform.parent;
                if (parentTa.parent.Find ("Active/SaveActive").gameObject.activeSelf)       //Save game, dont load
                {
                    Debug.Log(col.name);
                    slotNo = col.name;
                    SaveGame();
                }
                else if (parentTa.parent.Find("Active/LoadActive").gameObject.activeSelf)      //Load
                {
                    Debug.Log("Load Game");
                    slotNo = col.name;
                    LoadGame();
                }
            }
            else if (col.name == "LoadInactive" && state != State.Confirm)
            {
                Transform parentTa = col.transform.parent;
                Debug.Log(parentTa);
                parentTa.parent.Find("Active/LoadActive").gameObject.SetActive(true);
                parentTa.parent.Find("Active/SaveActive").gameObject.SetActive(false);
                //    col.gameObject.SetActive(false);
            }
            else if (col.name == "SaveInactive" && state != State.Confirm)
            {
                Transform parentTa = col.transform.parent;
                Debug.Log(parentTa);
                parentTa.parent.Find("Active/SaveActive").gameObject.SetActive(true);
                parentTa.parent.Find("Active/LoadActive").gameObject.SetActive(false);
                //      col.gameObject.SetActive(false);
            }
            else if (col.name == "Overwrite" && state == State.Confirm)
            {
                ExecuteSave();               
            }
            else if (col.name == "LoadScene" && state == State.Confirm)
            {
                ExecuteLoad();
            }

            else if (state == State.Idle)
            {
                if (col.transform.parent.name == "Slots" || col.transform.parent.name == "QuickSlots")
                {

          //         Debug.Log(state);
                    if (state == State.Idle)
                    {
        //                int slotNo = Convert.ToInt32(col.name);
                        Debug.Log(slotNo);

                        //there is something on that slot. 
                        for (int cnt = 0; cnt < col.transform.childCount; cnt++)
                        {
                            Debug.Log(col.transform.GetChild(cnt) + "/" + col.transform.GetChild(cnt));

                        }                        

                        if (col.transform.GetChild(0).name != "None")
                        {
                            audio.Play();
                            string qtString = col.transform.Find("Qt/Qt/Canvas/Text").gameObject.GetComponent<UnityEngine.UI.Text>().text;
                            int qtInt = Convert.ToInt32(qtString);
                            Debug.Log(qtInt);

                            tranToCarry = col.transform.GetChild(0);
                            qtToCarry = col.transform.GetChild(1);
                            Debug.Log(tranToCarry.name + "/" + qtToCarry.name);
                            tranToCarry.parent = null;
                            qtToCarry.parent = null;

                            GameObject go = new GameObject();
                            go.name = "None";
                            go.transform.position = col.transform.position;
                            go.transform.rotation = col.transform.rotation;
                            go.transform.parent = col.transform;
                            state = State.Carrying;
                        }
                        else
                        {
                            audioSecond.Play();
                        }
                    }
                }
            }       

            else if (col.transform.parent.name == "QuickSlots" || col.transform.parent.name == "Slots")
            {
                SwitchSprite(col.transform);
                
            }
            else if (col.transform.parent.parent.name == "QuestPositions")
            {
                Debug.Log(col.name);
                ChangeQuest(col.transform.parent.name, col.name);
            }
            else
            {
                audioSecond.Play();
            }
        }
    }


    private void ChangeActiveHead (Collider col)
    {
        audio.Play();

        if (col.transform.parent.name == "Inactive")
        {
            col.transform.parent.parent.Find("Active/" + col.gameObject.name).gameObject.SetActive(true);
            activeMenu = col.name;
            Debug.Log(activeMenu + col.transform.name + "/" + col.transform.parent.name);

            GameObject goPC = transform.Find("Plane/PC").gameObject;
            if (col.name == "Char" || col.name == "Inventory")
            {
                goPC.SetActive(true);
                if (col.name == "Inventory")
                {
                    loadgame.LoadQuickInventory();
                    loadgame.LoadInventory();
                }
            }
            else
            {
                if (goPC.activeSelf)
                {
                    goPC.SetActive(false);
                }
            }

            GameObject plane = transform.Find("Plane/Plane").gameObject;
            if (col.name == "Map")
            {
                if (plane.activeSelf )
                {
                    plane.SetActive(false);
                }
                cam.SetActive(false);
                camRPG.SetActive(true);
                state = State.Map;
                virtualCursor.gameObject.SetActive(false);
            }

            if (col.name == "Quests")
            {
                GetQuests();
            }

            if (col.name == "Other_Items")
            {
                loadgame.LoadBooks();
       //         Debug.Log(otherItemsSlots.name);

                if (otherItemsSlots.transform.Find ("0").childCount == 0)
                {
                    Debug.Log(otherItemsSlots.transform.Find ("0").name);
                }
                else
                {
                    string bookName = otherItemsSlots.transform.Find("0").GetChild(0).name;
                    otherItemsSlots.transform.parent.parent.parent.transform.Find("Canvas/Description").gameObject.GetComponent<Text>().text = GetBookDescription(bookName);
                }

            }


            foreach (Transform ta in transform.Find("Plane/Menu/Active"))
            {
                if (ta.name != col.name)
                {
                    ta.gameObject.SetActive(false);
                    Debug.Log(ta.name + "/" + ta.parent.name);
                }
                else
                {
                    Debug.Log(ta.name + "/" + ta.parent.name);
                }
            }


        }
    }

    private void ChangeChar (bool right)
    {
        audio.Play();
        int tempCNT = 0;

        for (int cnt = 0; cnt < characters.Count; cnt++)
        {
            if (characters[cnt] == activePC)
            {
                tempCNT = cnt;
                foreach (Transform ta in chars.transform)
                {
                    if (ta.name == characters[cnt])
                    {
                        ta.gameObject.SetActive(false);
                    }
                }
            }
        }
        if (right == true)
        {
            
            if (tempCNT >= characters.Count - 1 )
            {
                tempCNT = 0;
            }
            else
            {
                tempCNT++;
            }           
        }
        else
        {
            if (tempCNT <= 0)
            {
                tempCNT = characters.Count - 1;
            }
            else
            {
                tempCNT--;
            }
        }

        foreach (Transform ta in chars.transform)
        {
            if (ta.name == characters[tempCNT])
            {
                ta.gameObject.SetActive(true);
                activePC = ta.name;
            }
        }
    }


    private void Carrying ()
    {
        Debug.Log("Carry");
        tranToCarry.position = virtualCursor.position;
        qtToCarry.position = virtualCursor.position;
    }

    public void SwitchSprite (Transform homeDestiny)
    {
        audio.Play();
        Debug.Log(homeDestiny.childCount);
        if (homeDestiny.childCount > 0)
        {
            foreach (Transform ta in homeDestiny)
            {
                ta.parent = null;
                ta.gameObject.SetActive(false);
            }
        }
        
        tranToCarry.position = homeDestiny.position;
        tranToCarry.parent = homeDestiny;
        qtToCarry.position = homeDestiny.position;
        qtToCarry.parent = homeDestiny;
   //     tranToCarry = null;
        state = State.Idle;
    }

    private void Map ()
    {
        Debug.Log("Map");
        if (playerR.GetButtonUp ("Fire") || playerR.GetButtonUp("Special"))
        {
            audio.Play();
     //       Debug.Log("Fire");
            ChangeActiveHead(invCollider);
            transform.Find("Plane/Menu/Active/Map").gameObject.SetActive(false);
            transform.Find("Plane/Menu/Active/Inventory").gameObject.SetActive(true);
            state = State.Idle;
            virtualCursor.gameObject.SetActive(true);
            GameObject plane = transform.Find("Plane/Plane").gameObject;
            GameObject goPC = transform.Find("Plane/PC").gameObject;
            goPC.SetActive(true);
            if (plane.activeSelf == false)
            {
                plane.SetActive(true);
            }
            cam.SetActive(true);
            camRPG.SetActive(false);
            if (state == State.Map)
            {
                state = State.Idle;
            }
        }
    }

    /*
    private void LateUpdate()
    {
    //    Debug.Log("nkjdsf");
        if (Time.realtimeSinceStartup > timer)
        {
            if (playerR != null)
            {
                Debug.Log("nkjdsdsfwefgf");
                if (playerR.GetButtonUp("Escape"))
                {
                    Debug.Log("escapef");
                    gc.activePlayer.GetComponent<ThirdPersonUserControl>().TurnOffMenu();
                }
            }
        }

    }*/

    private void GetSavedGames ()
    {
        string path = Application.persistentDataPath + "/DandU";
        //     string fileName = path + "/" + profile + "/" + fileDirectory + ".dat";
        profile = gcon.GetComponent<LoadGameImp>().profile;
        string currentDirName = path + "/" + profile;
   //     Debug.Log(path + "/" + gcon.GetComponent<LoadGameImp>().profile);
        DirectoryInfo di = new DirectoryInfo(currentDirName);
        FileInfo[] smFiles = di.GetFiles("*.dat*");

        savedGames.Clear();
        dateSavedGames.Clear();
        List<string> tempNames = new List<string>();
        string autosaveDate = "None!";

        foreach (FileInfo fi in smFiles)
        {            
            string creationTime = File.GetLastWriteTime(currentDirName + "/" + fi.Name).ToString();
//            Debug.Log(fi.Name + "/" + creationTime);            
      
            if (fi.Name != "AutoSave.dat")
            {
                savedGames.Add(Path.GetFileNameWithoutExtension(fi.Name));
                dateSavedGames.Add(creationTime);
            }
            else
            { 
                autosaveDate = creationTime;
                autosaveText.GetComponent<Text>().text = creationTime;
            }            
        }

        Transform slotsTran = saveSlotsTa.Find("Slots");
        Transform labelsTran = saveSlotsTa.Find("Labels");

        for (int cnt = 0; cnt < dateSavedGames.Count; cnt++)
        {
            string fileName = currentDirName + "/" + cnt.ToString() + ".dat";
         
            if (File.Exists(fileName))
            {
         //       slotsTran.GetChild(cnt).gameObject.GetComponent<Text>().text = savedGames[cnt];
                labelsTran.GetChild(cnt).gameObject.GetComponent<Text>().text = dateSavedGames[cnt];
     //           Debug.Log(cnt + "/" + dateSavedGames[cnt]);
            }
        }
    }

    private void SaveGame ()
    {
        audio.Play();
        string path = Application.persistentDataPath + "/DandU";

        profile = gcon.GetComponent<LoadGameImp>().profile;
        string currentDirName = path + "/" + profile;
        string fileName = currentDirName + "/" + slotNo + ".dat";
        Debug.Log(slotNo);
        if (File.Exists(fileName))
        {
            warningGo.SetActive(true);
            optionsTra.Find("SaveWarning").gameObject.SetActive(true);
            optionsTra.Find("Overwrite").gameObject.SetActive(true);
            optionsTra.Find("Cancel").gameObject.SetActive(true);
            optionsTra.Find("Canvas/Overwrite").gameObject.SetActive(true);
            optionsTra.Find("Canvas/OverwriteLabel").gameObject.SetActive(true);
            optionsTra.Find("Canvas/Cancel").gameObject.SetActive(true);
            optionsTra.Find("Canvas/Labels").gameObject.SetActive(false);
            state = State.Confirm;

        }
        else
        {
            FinalSave();
        }
    //    GetSavedGames();

    }

    private void LoadGame ()
    {

        audio.Play();
        string path = Application.persistentDataPath + "/DandU";
        //     string fileName = path + "/" + profile + "/" + fileDirectory + ".dat";
        profile = gcon.GetComponent<LoadGameImp>().profile;
        string currentDirName = path + "/" + profile;
        string fileName = currentDirName + "/" + slotNo + ".dat";
        if (File.Exists(fileName))
        {
            warningGo.SetActive(true);
       //     optionsTra.Find("SaveWarning").gameObject.SetActive(true);
            optionsTra.Find("LoadScene").gameObject.SetActive(true);
            optionsTra.Find("Cancel").gameObject.SetActive(true);
            optionsTra.Find("Canvas/Load?").gameObject.SetActive(true);
            optionsTra.Find("Canvas/LoadLabel").gameObject.SetActive(true);
            optionsTra.Find("Canvas/Cancel").gameObject.SetActive(true);
            optionsTra.Find("Canvas/Labels").gameObject.SetActive(false);
            state = State.Confirm;

        }
    }

    private void ExecuteSave ()
    {
        FinalSave();
        warningGo.SetActive(false);
        optionsTra.Find("SaveWarning").gameObject.SetActive(false);
        optionsTra.Find("Overwrite").gameObject.SetActive(false);
        optionsTra.Find("Canvas/Overwrite").gameObject.SetActive(false);
        optionsTra.Find("Canvas/OverwriteLabel").gameObject.SetActive(false);
        optionsTra.Find("Canvas/Cancel").gameObject.SetActive(false);
        optionsTra.Find("Canvas/Labels").gameObject.SetActive(true);
        optionsTra.Find("Cancel").gameObject.SetActive(false);
        state = State.Idle;


    }

    private void FinalSave ()
    {
        Debug.Log(slotNo + "/" + profile);
        gcon.GetComponent<SaveGame>().saveAndExit = false;
        gcon.GetComponent<SaveGame>().SaveProfile(slotNo, UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, true);
        GetSavedGames();

        string lan = DialogueLua.GetVariable("language").asString;
        string varLua = "Game_Saved " + lan;
        string translation = DialogueLua.GetActorField("Dictionary", varLua).asString;
        DialogueManager.ShowAlert(translation);
    }

    private void ExecuteLoad ()
    {
        Debug.Log(slotNo);
        warningGo.SetActive(false);
        optionsTra.Find("SaveWarning").gameObject.SetActive(false);
        optionsTra.Find("Overwrite").gameObject.SetActive(false);
        optionsTra.Find("Canvas/Overwrite").gameObject.SetActive(false);

        optionsTra.Find("LoadScene").gameObject.SetActive(false);
        optionsTra.Find("Cancel").gameObject.SetActive(false);
        optionsTra.Find("Canvas/Load?").gameObject.SetActive(false);
        optionsTra.Find("Canvas/LoadLabel").gameObject.SetActive(false);
        optionsTra.Find("Canvas/Cancel").gameObject.SetActive(false);
        optionsTra.Find("Canvas/Labels").gameObject.SetActive(true);
        alive = false;

        string path = Application.persistentDataPath + "/DandU";
        string fileName = path + "/" + profile + "/" + slotNo + ".dat";


        if (File.Exists(fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(fileName, FileMode.Open);
            SaveQuest data = (SaveQuest)bf.Deserialize(file);
            file.Close();
            string saveData = data.saveData;
            sceneToLoad = data.currentScene;
            Debug.Log(sceneToLoad);


            gcon.GetComponent<SaveGame>().SaveProfile(slotNo, sceneToLoad, false);
            Time.timeScale = 1;
            Invoke("ExitToLoad", 0.5f);
        }
    }

    private void ExitToLoad ()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }

    private void GetQuests ()
    {
        string[] quests = QuestLog.GetAllQuests();
        if (quests != null)
        {
            if (quests.Length > 0)
            {
                for (int cnt = 0; cnt < quests.Length; cnt++)
                {
                    Transform ta = questPositions.transform.Find(cnt.ToString());
                    GameObject questGO = Instantiate(Resources.Load("GUI/RPG/Model"), ta.position, ta.rotation) as GameObject;
                    string displayName = DialogueLua.GetQuestField(quests[cnt], "Display Name").asString;
                    Debug.Log(displayName);
                    questGO.name = quests[cnt];
                    questGO.transform.parent = ta;
                    questGO.transform.Find("Canvas/Quest").gameObject.GetComponent<Text>().text = displayName;

                    if (cnt == 0)
                    {
                        string description = DialogueLua.GetQuestField(quests[cnt], "Description").asString;
                        descriptionGO.SetActive(true);
                        Debug.Log(description);
                        questDescription.text = description;
                        activeButton.transform.position = new Vector3(ta.position.x, ta.position.y + 0.15f, ta.position.z);
                        activeButton.SetActive(true);
                    }

                }


            }
        }
    }

    private void ChangeQuest (string position, string quest)
    {

    }

    public string GetBookDescription(string nameID)
    {
        string lan = DialogueLua.GetVariable("language").asString;
        string localized = nameID + lan;
        string resourcePath = "Text/Books/" + lan + "/" + nameID;
        TextAsset descriptionTextAsset = (TextAsset)(Resources.Load(resourcePath));
        string descriptionString = descriptionTextAsset.text;
     //   Debug.Log(descriptionString);
        return descriptionString;
    }
}
