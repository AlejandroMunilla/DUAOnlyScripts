using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHUD : MonoBehaviour
{

    private PlayerStats ps;
    private Texture2D greenBar;
    private Texture2D redBar;
    private Texture2D regenBar;

    // Start is called before the first frame update
    void Start()
    {
        greenBar = (Texture2D)(Resources.Load("GUI/greenBar"));
        redBar = (Texture2D)(Resources.Load("GUI/redBar"));
        regenBar = (Texture2D)(Resources.Load("GUI/Empty"));
        ps = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void OnGUI()
    {

        GUI.Label (new Rect(Screen.width * 0.3f, Screen.height * 0.01f, Screen.width * 0.3f, Screen.height * 0.02f), gameObject.name);
        GUI.DrawTexture(new Rect(Screen.width * 0.3f, Screen.height * 0.04f, Screen.width * 0.3f, Screen.height * 0.025f), redBar);
        GUI.DrawTexture(new Rect(Screen.width * 0.3f, Screen.height * 0.04f, ps.greenBar, Screen.height * 0.025f),greenBar);
        GUI.DrawTexture(new Rect((Screen.width * 0.3f + ps.regenBar), Screen.height * 0.039f, Screen.width * 0.01f, Screen.height * 0.027f), regenBar);

  }
}
