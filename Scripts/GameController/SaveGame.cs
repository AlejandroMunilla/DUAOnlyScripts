using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using System;

public class SaveGame : MonoBehaviour {

    public bool gameSaved = false;
    public bool saveAndExit = false;
    public string sceneToExit = null;
    private string profile = null;
    private string fileDirectory = null;
    private GameController gc;
    private GameObject rpgMenu;


    private void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        rpgMenu = GameObject.FindGameObjectWithTag("RPGMenu");
    }

    public void SaveProfile (string fileDirectoryString, string scene, bool saveData)
    {
        string path = Application.persistentDataPath + "/DANDU";
        //       Debug.Log(path);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);

        }
        BinaryFormatter bf2 = new BinaryFormatter();
        string fileName = path + "/CurrentProfile.dat";
        //     Debug.Log(fileName);
        FileStream file = File.Create(fileName);
        BinaryFormatter bf = new BinaryFormatter();

        SaveNames data = new SaveNames();


        //		Debug.Log (mainPlayerName);

        data.currentProfile = DialogueLua.GetVariable("profile").asString;
        profile = DialogueLua.GetVariable("profile").asString;
    //    Debug.Log(DialogueLua.GetVariable("profile").asString);
        data.saveFileName = fileDirectoryString;
        data.scene = scene;

        bf.Serialize(file, data);
        file.Close();
        fileDirectory = fileDirectoryString;
   //     Debug.Log(saveData);
        if (saveData == true)
        {

           
            if (gc != null)
            {
                if (gc.isRPG)
                {
                    SaveRPGInventory();
                }
            }
            Invoke("DelaySaveData", 0.05f);

        }
       
    }

    public void DelaySaveData ()
    {
        SaveData(profile, fileDirectory);
    }


    public void SaveData (string profile, string fileDirectory)
    {
    //   Debug.Log(profile);
        string path = Application.persistentDataPath + "/DANDU";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }       


        //	string fileName = @"C:/SaveGame/" + mainPlayerName + "/" + fileDirectory + ".dat";	
        string fileName = path + "/" + profile + "/" + fileDirectory + ".dat";
   //          Debug.Log(fileName);
        if (!Directory.Exists(path + "/" + profile))
        {
            Directory.CreateDirectory(path + "/" + profile); //+ "/" + fileDirectory);
        }

        string saveData = PersistentDataManager.GetSaveData();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(fileName);
        SaveQuest data = new SaveQuest();
        data.saveData = saveData;
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        data.currentScene = currentScene;
        bf.Serialize(file, data);
        file.Close();
        gameSaved = true;




        //     Debug.Log(saveAndExit + "/" + sceneToExit);
        if (saveAndExit == true && sceneToExit != null)
        {
            Invoke("Exit", 0.1f);
        }
    }

    private void Exit ()
    {
   //     Debug.Log(DialogueLua.GetVariable("language").AsString + "/" + DialogueLua.GetVariable ("test").asString);
        SceneManager.LoadScene(sceneToExit);
    }

    public void SaveRPGInventory ()
    {
        Transform slots = rpgMenu.transform.Find("Plane/Menu/Active/Inventory/Slots");
        string rpgInventory = "";
        for (int cnt = 0; cnt < 71; cnt++)
        {
            Transform ta = slots.GetChild(cnt);
            int itemNo = 0;

            if (ta.GetChild(0).name != "None")
            {
                if (ta.GetChild(1) != null)
                {
                    if (ta.transform.Find("Qt/Qt/Canvas/Text").gameObject.GetComponent<UnityEngine.UI.Text>().text != "")
                    {
                        itemNo = Convert.ToInt32(ta.transform.Find("Qt/Qt/Canvas/Text").gameObject.GetComponent<UnityEngine.UI.Text>().text);
                    }
                }
                //slots + qt + name
                if (cnt > 0)
                {
                    rpgInventory = rpgInventory + "/" + cnt.ToString() + "*" + itemNo + "*" + ta.GetChild(0).name;
                }
                else
                {
                    rpgInventory = cnt.ToString() + "*" + itemNo + "*" + ta.GetChild(0).name;
                }
            }
            else
            {
                if (cnt > 0)
                {
                    rpgInventory = rpgInventory + "/" + cnt.ToString() + "*0*None";
                }
                else
                {
                    rpgInventory = cnt.ToString() + "*0*None";
                }
                
            }
            Debug.Log(cnt.ToString() + "*" + itemNo + "*" + ta.GetChild(0).name);


        }
        DialogueLua.SetVariable("rpgInventory", rpgInventory);

        Transform quickSlots = rpgMenu.transform.Find("Plane/Menu/Active/Inventory/QuickSlots");
        string rpgInventoryQuick = "";
        for (int cnt = 0; cnt < 3; cnt++)
        {
            Transform ta = quickSlots.GetChild(cnt);
        //    Debug.Log(ta.name);
            int itemNo = 0;
            if (ta.GetChild(0).name != "None")
            {
                if (ta.transform.Find("Qt/Qt/Canvas/Text") != null)
                {
                    if (ta.transform.Find("Qt/Qt/Canvas/Text").gameObject.GetComponent<UnityEngine.UI.Text>().text != "")
                    {
                        itemNo = Convert.ToInt32(ta.transform.Find("Qt/Qt/Canvas/Text").gameObject.GetComponent<UnityEngine.UI.Text>().text);
                    }
                }
               

            }

            if (cnt > 0)
            {
                rpgInventoryQuick = rpgInventoryQuick + "/" + cnt.ToString() + "*" + itemNo + "*" + ta.GetChild(0).name;
            }
            else
            {
                rpgInventoryQuick = cnt.ToString() + "*" + itemNo + "*" + ta.GetChild(0).name;
            }
        }
        DialogueLua.SetVariable("rpgInventoryQuick", rpgInventoryQuick);

        string booksLua = "" ;
        if (gc.books.Count > 0)
        {
            
            for (int cnt = 0; cnt < gc.books.Count; cnt++)
            {
                if (cnt != 0)
                {
                    booksLua = "/" + gc.books[cnt].name + "*1*" + cnt.ToString();
                }
                else
                {
                    booksLua =  gc.books[cnt].name + "*1*"  + cnt.ToString();
                }
            }
        }
        else
        {
            booksLua = "None";
        }
        DialogueLua.SetVariable("books", booksLua);

    }

}

[Serializable]
class SaveNames
{
    public string currentProfile;
    public string saveFileName;
    public string scene;
    
}

[Serializable]
public class SaveQuest
{
    public string saveData;
    public string currentScene;
    public string saveName;
}
