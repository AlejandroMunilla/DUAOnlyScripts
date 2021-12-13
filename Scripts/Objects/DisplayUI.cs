using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayUI : MonoBehaviour
{
    public Texture2D texture;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnGUI()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        GUI.DrawTexture (new Rect(Screen.width *0.25f, Screen.height * 0.25f, Screen.width * 0.25f, Screen.height * 0.25f), texture);
        GUI.Button(new Rect(Screen.width * 0.25f, Screen.height * 0.25f, Screen.width * 0.25f, Screen.height * 0.25f), "TEST");
    }
}
