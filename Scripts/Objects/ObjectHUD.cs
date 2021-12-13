using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHUD : MonoBehaviour
{

    private ObjectStats os;
    private Texture2D greenBar;
    private Texture2D redBar;


    // Start is called before the first frame update
    void OnEnable ()
    {
        greenBar = (Texture2D)(Resources.Load("GUI/greenBar"));
        redBar = (Texture2D)(Resources.Load("GUI/redBar"));

        os = GetComponent<ObjectStats>();
    }

    // Update is called once per frame
    void OnGUI()
    {

        GUI.Label (new Rect(Screen.width * 0.3f, Screen.height * 0.01f, Screen.width * 0.3f, Screen.height * 0.02f), gameObject.name);
        GUI.DrawTexture(new Rect(Screen.width * 0.3f, Screen.height * 0.04f, Screen.width * 0.3f, Screen.height * 0.025f), redBar);
        GUI.DrawTexture(new Rect(Screen.width * 0.3f, Screen.height * 0.04f, os.greenBar, Screen.height * 0.025f),greenBar);

  }
}
