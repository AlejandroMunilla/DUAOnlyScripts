using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureCounter : MonoBehaviour
{

    private PlayerStats ps;
    private Texture2D redBar;
    private Transform myTransform;
    private Camera cam;
    public Rect center;
    public int counter = 5;
    public AudioClip healing;
    public AudioClip healed;

    private bool loaded = false;
    private GUIStyle myStyle;
    

    // Start is called before the first frame update
    void OnEnable ()
    {
        if (loaded == false)
        {
            loaded = true;
            redBar = (Texture2D)(Resources.Load("GUI/redBar"));
            ps = GetComponent<PlayerStats>();
            cam = GetComponent<PlayerAttack>().cam;

            myStyle = new GUISkin().GetStyle("label");
            myStyle.fontSize = (int)(Screen.height * 0.035f);
            myStyle.normal.textColor = Color.white;
            myStyle.alignment = TextAnchor.UpperLeft;

            healed = (AudioClip)(Resources.Load("Audio/sfx_shield"));
            healing = (AudioClip)(Resources.Load("Audio/gale_wind"));
        }       
    }



    // Update is called once per frame
    void OnGUI()
    {
            GUI.Label(center, counter.ToString(), myStyle);
    }
}
