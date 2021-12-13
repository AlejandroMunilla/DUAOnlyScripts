using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarnRedMark : MonoBehaviour
{
    private Camera cam;
    private Transform parentObj;
    private Texture2D myTexture;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        parentObj= transform.root;
        myTexture = (Texture2D)(Resources.Load("GUI/RedWarning"));
    }


    private void OnGUI()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(parentObj.position);
        GUI.DrawTexture(new Rect(screenPos.x, screenPos.y, 100, 100), myTexture);
    }
}
