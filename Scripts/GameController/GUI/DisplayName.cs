using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayName : MonoBehaviour
{

    private Texture2D input;
    private GUISkin mySkin;
    private GUIStyle myStyle;
    private Rect rect;
    private Rect labelRect;
    public float timer = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        this.enabled = false;
        input = (Texture2D)(Resources.Load("GUI/Small_Decor"));
        mySkin = (GUISkin)(Resources.Load("GUI/JRPG2 Dialogue GUI Skin 1"));
        myStyle = mySkin.GetStyle("label");
        myStyle.fontSize = (int)(Screen.height * 0.026f);
        myStyle.normal.textColor = Color.white;
        myStyle.alignment = TextAnchor.MiddleCenter;

        rect = new Rect(Screen.width * 0.36f, Screen.height * 0.01f, Screen.width * 0.26f, Screen.height * 0.07f);
        labelRect = new Rect(Screen.width * 0.37f, Screen.height * 0.02f, Screen.width * 0.25f, Screen.height * 0.05f);


    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            this.enabled = false;
            timer = 0.2f;
        }
    }

    private void OnGUI()
    {
        GUI.DrawTexture(rect, input);
        GUI.Label(labelRect, gameObject.name, myStyle);
    }
}
