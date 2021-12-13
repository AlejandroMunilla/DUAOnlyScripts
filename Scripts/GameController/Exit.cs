
using UnityEngine;

public class Exit : MonoBehaviour
{
    public string targetScene;
    public string loadPosition = "No";
    private SaveGame savegame;

    private void Start()
    {
        savegame = GameObject.FindGameObjectWithTag("GameController").GetComponent<SaveGame>();
    }

    public void SaveAndExit ()
    {

        if (loadPosition != "No")
        {
     //       Debug.Log(loadPosition);
            PixelCrushers.DialogueSystem.DialogueLua.SetVariable("nextPos", loadPosition);
        }
        Debug.Log(targetScene);
        savegame.sceneToExit = targetScene;
        savegame.SaveProfile("Autosave", targetScene, true);
    }
}
