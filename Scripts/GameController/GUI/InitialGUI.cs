using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialGUI : MonoBehaviour
{

    private int textureHeight;
    public Texture2D myTexture;
    // Start is called before the first frame update
    void Start()
    {
        textureHeight = (int)(Screen.width * 0.5f);

    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, textureHeight), myTexture);
    }


}
