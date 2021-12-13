using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticule : MonoBehaviour
{
    public Texture reticule;
    public Texture redReticule;
    public Texture greyReticule;
    public Rect reticuleRect;
    public Vector2 reticuleRectPos;
    // Start is called before the first frame update
    void Start()
    {
        reticuleRect = new Rect(Screen.width * 0.48f, Screen.height * 0.24f, Screen.width * 0.014f, Screen.width * 0.014f);
        reticuleRectPos = new Vector2(Screen.width * 0.51f, Screen.height * 0.51f);

        greyReticule = (Texture2D)(Resources.Load("GUI/GreyReticle"));
        redReticule = (Texture2D)(Resources.Load("GUI/RedReticle"));
    }

    private void OnGUI()
    {

        GUI.DrawTexture(reticuleRect, reticule);
    }

}
