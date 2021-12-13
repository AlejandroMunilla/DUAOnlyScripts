using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisplayRPGMenu : MonoBehaviour
{
   
    public int toolbarInt = 0;
    private string character;
    private string inventory;
    private string items;
    private string books;
    private string save;
    private string load;
    private string options;
    private string quit;
    private Rect pointer;
    public Texture2D myTexture;
    public string[] toolbarStrings = new string[] { "Toolbar1", "Toolbar2", "Toolbar3" };
    private enum State
    {
        Character,
        Inventory,
    }
    private State state;



}
