using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class myGUI : MonoBehaviour {

    public float portraitWidth;
    private int barWidth;
    private int barHeight;
    public Texture2D pic1;
    public Texture2D pic2;
    public Texture2D pic3;
    public Texture2D pic4;
    private Texture2D coin;
    private Texture2D bigboard;
    //   private List<Texture2D> items1 = new List<Texture2D>();
    private Texture2D item1;
    private Texture2D item2;
    private Texture2D item3;
    private Texture2D item4;

    public List<Texture2D> skill1Tex;
    public List<Texture2D> skill1TexActive;
    public List<Texture2D> skill1TexNo;

    public List<Texture2D> skill2Tex;
    public List<Texture2D> skill2TexActive;
    public List<Texture2D> skill2TexNo;

    public List<float> skill1Cool = new List<float>();
    public List<float> skill2Cool = new List<float>();

    private List<string> inventory1 = new List<string>();
    private List<string> inventory2 = new List<string>();
    private List<string> inventory3 = new List<string>();
    private List<string> inventory4 = new List<string>();
    private int item1No;
    private int item2No;
    private int item3No;
    private int item4No;

    private Texture2D redBar;
    private Texture2D greenBar;
    private Texture2D blueBar;

    public GUIStyle myStyle;
    public GUISkin mySkin;

    public Rect rect1;
    public Rect rect2;
    public Rect rect3;
    public Rect rect4;
    public Rect greenRect1;
    public Rect greenRect2;
    public Rect greenRect3;
    public Rect greenRect4;
    public Rect blueRect1;
    public Rect blueRect2;
    public Rect blueRect3;
    public Rect blueRect4;
    public Rect coins1;
    public Rect coins2;
    public Rect coins3;
    public Rect coins4;
    public Rect labelCoins1;
    public Rect labelCoins2;
    public Rect labelCoins3;
    public Rect labelCoins4;
    
    private Rect redBar1;
    private Rect redBar2;
    private Rect redBar3;
    private Rect redBar4;

    private Rect itemRect1;
    private Rect itemRect2;
    private Rect itemRect3;
    private Rect itemRect4;

    public Rect p1Skill1Rect;
    public Rect p2Skill1Rect;
    public Rect p3Skill1Rect;
    public Rect p4Skill1Rect;

    public Rect p1Skill2Rect;
    public Rect p2Skill2Rect;
    public Rect p3Skill2Rect;
    public Rect p4Skill2Rect;


    private List<float> timers = new List<float>();
    private List<string> descriptions = new List<string>();
    private List<int> itemNos = new List<int>();
    public List<Texture2D> itemTextures = new List<Texture2D>();

    private Rect desRect1;
    private Rect desRect2;
    private Rect desRect3;
    private Rect desRect4;


    private Vector2 greenBar1;
    private Vector2 greenBar2;
    private Vector2 greenBar3;
    private Vector2 greenBar4;

    private Vector2 blueBar1;
    private Vector2 blueBar2;
    private Vector2 blueBar3;
    private Vector2 blueBar4;

    public string debugGUI = "";
    private GameController gc;

    //RPG game
    private Transform inventRPG;
    private Transform quickSlots;
    public GameObject rpgMenu;

    void Start ()
    {
        gc = GetComponent<GameController>();
        barWidth = (int)(Screen.width * 0.008f);
     //   barHeight = (int) (portraitWidth);
        portraitWidth = (Screen.width * 0.08f);
        greenBar = (Texture2D)(Resources.Load("GUI/greenBar"));
        redBar = (Texture2D)(Resources.Load("GUI/redBar"));
        blueBar = (Texture2D)(Resources.Load("GUI/blueBar"));
        coin = (Texture2D)(Resources.Load("GUI/Coin"));
     

        myStyle = mySkin.GetStyle("label");
        myStyle.fontSize = (int)(Screen.height * 0.028f);
        myStyle.normal.textColor = Color.red;
        myStyle.alignment = TextAnchor.MiddleCenter;
        /*
        bigboard = (Texture2D)(Resources.Load("GUI/Bigboard"));
        items1.Add((Texture2D)(Resources.Load("GUI/Items/Healing")));
        items1.Add ((Texture2D)(Resources.Load("GUI/Items/15000")));*/

        if (gc.sceneFP == false)
        {
            CalculateRectsArcade();
        }
        else
        {
            CalculateRectsFP();
        }


        DialogueLua.SetActorField("1000", "short", "Health Potion(drink)");
        DialogueLua.SetActorField("1001", "short", "Mana Potion(drink)");

        for (int cnt = 0; cnt < gc.players.Count; cnt++)
        {
            descriptions.Add("");
            timers.Add(0);
            itemNos.Add(0);
            Texture2D tempTex = (Texture2D)(Resources.Load("Inventory/1000"));
            itemTextures.Add(tempTex);
       //     gc.players[cnt].GetComponent<PlayerStats>().inventory.Add("1000");
       //     gc.players[cnt].GetComponent<PlayerStats>().inventory.Add("1001");
        }

        if (gc.isRPG)
        {
           
            inventRPG = rpgMenu.transform.Find("Plane/Menu/Active/Inventory");
            quickSlots = inventRPG.Find("QuickSlots");
        }
        /*
        item1 = (Texture2D)(Resources.Load("Inventory/1000"));
        item2 = (Texture2D)(Resources.Load("Inventory/1000"));
        item3 = (Texture2D)(Resources.Load("Inventory/1000"));
        item4 = (Texture2D)(Resources.Load("Inventory/1000"));
        */



    }

    public void CalculateRectsArcade()
    {
        rect1 = new Rect(0, 0, portraitWidth, portraitWidth);
        redBar1 = new Rect(portraitWidth, 0, barWidth, portraitWidth);
        greenBar1 = new Vector2(portraitWidth, 0);
        blueBar1 = new Vector2(portraitWidth + barWidth, 0);
        float pos1Y = 0;
        itemRect1 = new Rect(portraitWidth + (2 * barWidth), pos1Y, portraitWidth * 0.3f, portraitWidth * 0.3f);
        desRect1 = new Rect(0, pos1Y + portraitWidth, 2 * portraitWidth, Screen.height * 0.09f);

        rect2 = new Rect(Screen.width - portraitWidth, 0, portraitWidth, portraitWidth);
        redBar2 = new Rect(Screen.width - portraitWidth - barWidth, 0, barWidth, portraitWidth);
        greenBar2 = new Vector2(Screen.width - portraitWidth - barWidth, 0);
        blueBar2 = new Vector2(Screen.width - portraitWidth - (2 * barWidth), 0);
        float pos2Y = 0;
        itemRect2 = new Rect(Screen.width - portraitWidth - (5*barWidth), pos2Y, portraitWidth * 0.3f, portraitWidth * 0.3f);
        desRect2 = new Rect(Screen.width - portraitWidth - (3 * barWidth), pos2Y + portraitWidth, 2 * portraitWidth, Screen.height * 0.09f);

        rect3 = new Rect(0, Screen.height - portraitWidth, portraitWidth, portraitWidth);
        redBar3 = new Rect(portraitWidth, Screen.height - portraitWidth, barWidth, portraitWidth);
        greenBar3 = new Vector2(portraitWidth, Screen.height - portraitWidth);
        blueBar3 = new Vector2(portraitWidth + barWidth, Screen.height - portraitWidth);
        float pos3Y = Screen.height - portraitWidth;
        itemRect3 = new Rect(portraitWidth + (2 * barWidth), pos3Y, portraitWidth * 0.3f, portraitWidth * 0.3f);
        desRect3 = new Rect(0, pos3Y - barWidth, 2 * portraitWidth, Screen.height * 0.09f);


        rect4 = new Rect(Screen.width - portraitWidth, Screen.height - portraitWidth, portraitWidth, portraitWidth);
        redBar4 = new Rect(Screen.width - portraitWidth, Screen.height - portraitWidth, barWidth, portraitWidth);
        greenBar4 = new Vector2(Screen.width - portraitWidth, Screen.height - portraitWidth);
        blueBar4 = new Vector2(Screen.width - portraitWidth + barWidth, Screen.height - portraitWidth);
        float pos4Y = pos3Y;
        itemRect4 = new Rect(Screen.width - portraitWidth - (5 * barWidth), pos4Y, portraitWidth * 0.3f, portraitWidth * 0.3f);
        desRect4 = new Rect(Screen.width - portraitWidth - (3 * barWidth), pos4Y - barWidth, 2 * portraitWidth, Screen.height * 0.09f);


    }

    public void CalculateRectsFP()
    {
   //     Debug.Log("RectFP");
        int noPlayers = gc.players.Count;

        if (gc.multiplayer == true)
        {
            float pos1Y = 0;
            rect1 = new Rect(0, pos1Y, portraitWidth, portraitWidth);
            redBar1 = new Rect(portraitWidth, pos1Y, barWidth, portraitWidth);
            greenBar1 = new Vector2(portraitWidth, pos1Y);
            blueBar1 = new Vector2(portraitWidth + barWidth, pos1Y);
            itemRect1 = new Rect(portraitWidth + (2 * barWidth), pos1Y, portraitWidth * 0.3f, portraitWidth * 0.3f);
            desRect1 = new Rect(0, pos1Y + portraitWidth, 2 * portraitWidth, Screen.height * 0.09f);
            gc.player1.GetComponent<CureCounter>().center = new Rect(Screen.width * 0.45f, Screen.height * 0.45f, Screen.width * 0.1f, Screen.width * 0.1f);
            p1Skill1Rect = new Rect(portraitWidth + (2 * barWidth), pos1Y + (portraitWidth * 0.3f), portraitWidth * 0.3f, portraitWidth * 0.3f);
            p1Skill2Rect = new Rect(portraitWidth + (2 * barWidth), pos1Y + (2 * portraitWidth * 0.3f), portraitWidth * 0.3f, portraitWidth * 0.3f);

            float pos2Y = 0;
            rect2 = new Rect(0, pos2Y, portraitWidth, portraitWidth);
            redBar2 = new Rect(portraitWidth, pos2Y, barWidth, portraitWidth);
            greenBar2 = new Vector2(portraitWidth, pos2Y);
            blueBar2 = new Vector2(portraitWidth + barWidth, pos2Y);
            itemRect2 = new Rect(portraitWidth + (2 * barWidth), pos2Y, portraitWidth * 0.3f, portraitWidth * 0.3f);
            desRect2 = new Rect(0, pos2Y + portraitWidth, 2 * portraitWidth, Screen.height * 0.09f);
            gc.player2.GetComponent<CureCounter>().center = new Rect(Screen.width * 0.45f, Screen.height * 0.45f, Screen.width * 0.1f, Screen.width * 0.1f);
            p2Skill1Rect = new Rect(portraitWidth + (2 * barWidth), pos2Y + (portraitWidth * 0.3f), portraitWidth * 0.3f, portraitWidth * 0.3f);
            p2Skill2Rect = new Rect(portraitWidth + (2 * barWidth), pos2Y + (2 * portraitWidth * 0.3f), portraitWidth * 0.3f, portraitWidth * 0.3f);

            float pos3Y = 0;
            rect3 = new Rect(0, pos3Y, portraitWidth, portraitWidth);
            redBar3 = new Rect(portraitWidth, pos3Y, barWidth, portraitWidth);
            greenBar3 = new Vector2(portraitWidth, pos3Y);
            blueBar3 = new Vector2(portraitWidth + barWidth, pos3Y);
            itemRect3 = new Rect(portraitWidth + (2 * barWidth), pos3Y, portraitWidth * 0.3f, portraitWidth * 0.3f);
            desRect3 = new Rect(0, pos3Y + portraitWidth, 2 * portraitWidth, Screen.height * 0.09f);
            gc.player3.GetComponent<CureCounter>().center = new Rect(Screen.width * 0.45f, Screen.height * 0.45f, Screen.width * 0.1f, Screen.width * 0.1f);
            p3Skill1Rect = new Rect(portraitWidth + (2 * barWidth), pos3Y + (portraitWidth * 0.3f), portraitWidth * 0.3f, portraitWidth * 0.3f);
            p3Skill2Rect = new Rect(portraitWidth + (2 * barWidth), pos3Y + (2 * portraitWidth * 0.3f), portraitWidth * 0.3f, portraitWidth * 0.3f);

            float pos4Y = 0;
            rect4 = new Rect(0, pos4Y, portraitWidth, portraitWidth);
            redBar4 = new Rect(portraitWidth, pos4Y, barWidth, portraitWidth);
            greenBar4 = new Vector2(portraitWidth, pos4Y);
            blueBar4 = new Vector2(portraitWidth + barWidth, pos4Y);
            itemRect4 = new Rect(portraitWidth + (2 * barWidth), pos4Y, portraitWidth * 0.3f, portraitWidth * 0.3f);
            desRect4 = new Rect(0, pos4Y + portraitWidth, 2 * portraitWidth, Screen.height * 0.09f);
            gc.player4.GetComponent<CureCounter>().center = new Rect(Screen.width * 0.45f, Screen.height * 0.45f, Screen.width * 0.1f, Screen.width * 0.1f);
            p4Skill1Rect = new Rect(portraitWidth + (2 * barWidth), pos4Y + (portraitWidth * 0.3f), portraitWidth * 0.3f, portraitWidth * 0.3f);
            p4Skill2Rect = new Rect(portraitWidth + (2 * barWidth), pos4Y + (2 * portraitWidth * 0.3f), portraitWidth * 0.3f, portraitWidth * 0.3f);
        }
        else if (gc.isRPG)
        {
            float pos1Y = 0;
            rect1 = new Rect(0, pos1Y, portraitWidth, portraitWidth);
            redBar1 = new Rect(portraitWidth, pos1Y, barWidth, portraitWidth);
            greenBar1 = new Vector2(portraitWidth, pos1Y);
            blueBar1 = new Vector2(portraitWidth + barWidth, pos1Y);
            itemRect1 = new Rect(portraitWidth + (2 * barWidth), pos1Y, portraitWidth * 0.3f, portraitWidth * 0.3f);
            desRect1 = new Rect(0, pos1Y + portraitWidth, 2 * portraitWidth, Screen.height * 0.09f);
            gc.player1.GetComponent<CureCounter>().center = new Rect(Screen.width * 0.45f, Screen.height * 0.45f, Screen.width * 0.1f, Screen.width * 0.1f);
            p1Skill1Rect = new Rect(portraitWidth + (2 * barWidth), pos1Y + (portraitWidth * 0.3f), portraitWidth * 0.3f, portraitWidth * 0.3f);
            p1Skill2Rect = new Rect(portraitWidth + (2 * barWidth), pos1Y + (2 * portraitWidth * 0.3f), portraitWidth * 0.3f, portraitWidth * 0.3f);

            float pos2Y = portraitWidth;
            rect2 = new Rect(0, pos2Y, portraitWidth, portraitWidth);
            redBar2 = new Rect(portraitWidth, pos2Y, barWidth, portraitWidth);
            greenBar2 = new Vector2(portraitWidth, pos2Y);
            blueBar2 = new Vector2(portraitWidth + barWidth, pos2Y);
            itemRect2 = new Rect(portraitWidth + (2 * barWidth), pos2Y, portraitWidth * 0.3f, portraitWidth * 0.3f);
            desRect2 = new Rect(0, pos2Y + portraitWidth, 2 * portraitWidth, Screen.height * 0.09f);
    //        gc.player2.GetComponent<CureCounter>().center = new Rect(Screen.width * 0.45f, Screen.height * 0.45f, Screen.width * 0.1f, Screen.width * 0.1f);
            p2Skill1Rect = new Rect(portraitWidth + (2 * barWidth), pos2Y + (portraitWidth * 0.3f), portraitWidth * 0.3f, portraitWidth * 0.3f);
            p2Skill2Rect = new Rect(portraitWidth + (2 * barWidth), pos2Y + (2 * portraitWidth * 0.3f), portraitWidth * 0.3f, portraitWidth * 0.3f);
       //     Debug.Log(itemRect2);


            float pos3Y = pos2Y + portraitWidth;
            rect3 = new Rect(0, pos3Y, portraitWidth, portraitWidth);
            redBar3 = new Rect(portraitWidth, pos3Y, barWidth, portraitWidth);
            greenBar3 = new Vector2(portraitWidth, pos3Y);
            blueBar3 = new Vector2(portraitWidth + barWidth, pos3Y);
            itemRect3 = new Rect(portraitWidth + (2 * barWidth), pos3Y, portraitWidth * 0.3f, portraitWidth * 0.3f);
            desRect3 = new Rect(0, pos3Y + portraitWidth, 2 * portraitWidth, Screen.height * 0.09f);
      //      gc.player3.GetComponent<CureCounter>().center = new Rect(Screen.width * 0.45f, Screen.height * 0.45f, Screen.width * 0.1f, Screen.width * 0.1f);
            p3Skill1Rect = new Rect(portraitWidth + (2 * barWidth), pos3Y + (portraitWidth * 0.3f), portraitWidth * 0.3f, portraitWidth * 0.3f);
            p3Skill2Rect = new Rect(portraitWidth + (2 * barWidth), pos3Y + (2 * portraitWidth * 0.3f), portraitWidth * 0.3f, portraitWidth * 0.3f);
        }
        else
        {
            if (noPlayers == 1)
            {
                float pos1Y = 0;
                rect1 = new Rect(0, pos1Y, portraitWidth, portraitWidth);
                redBar1 = new Rect(portraitWidth, pos1Y, barWidth, portraitWidth);
                greenBar1 = new Vector2(portraitWidth, pos1Y);
                blueBar1 = new Vector2(portraitWidth + barWidth, pos1Y);
                itemRect1 = new Rect(portraitWidth + (2 * barWidth), pos1Y, portraitWidth * 0.3f, portraitWidth * 0.3f);
                desRect1 = new Rect(0, pos1Y + portraitWidth, 2 * portraitWidth, Screen.height * 0.09f);
                gc.player1.GetComponent<CureCounter>().center = new Rect(Screen.width * 0.45f, Screen.height * 0.45f, Screen.width * 0.1f, Screen.width * 0.1f);
                p1Skill1Rect = new Rect(portraitWidth + (2 * barWidth), pos1Y + (portraitWidth * 0.3f), portraitWidth * 0.3f, portraitWidth * 0.3f);
                p1Skill2Rect = new Rect(portraitWidth + (2 * barWidth), pos1Y + (2 * portraitWidth * 0.3f), portraitWidth * 0.3f, portraitWidth * 0.3f);

            }
            else if (noPlayers == 2)
            {
                float pos2Y = 0;
                rect2 = new Rect(0, pos2Y, portraitWidth, portraitWidth);
                redBar2 = new Rect(portraitWidth, pos2Y, barWidth, portraitWidth);
                greenBar2 = new Vector2(portraitWidth, pos2Y);
                blueBar2 = new Vector2(portraitWidth + barWidth, pos2Y);
                itemRect2 = new Rect(portraitWidth + (2 * barWidth), pos2Y, portraitWidth * 0.3f, portraitWidth * 0.3f);
                desRect2 = new Rect(0, pos2Y + portraitWidth, 2 * portraitWidth, Screen.height * 0.09f);
                gc.player2.GetComponent<CureCounter>().center = new Rect(Screen.width * 0.45f, Screen.height * 0.23f, Screen.width * 0.09f, Screen.width * 0.09f);
                p2Skill1Rect = new Rect(portraitWidth + (2 * barWidth), pos2Y + (portraitWidth * 0.3f), portraitWidth * 0.3f, portraitWidth * 0.3f);
                p2Skill2Rect = new Rect(portraitWidth + (2 * barWidth), pos2Y + (2 * portraitWidth * 0.3f), portraitWidth * 0.3f, portraitWidth * 0.3f);


                float pos1Y = Screen.height * 0.5f;
                rect1 = new Rect(0, pos1Y, portraitWidth, portraitWidth);
                redBar1 = new Rect(portraitWidth, pos1Y, barWidth, portraitWidth);
                greenBar1 = new Vector2(portraitWidth, pos1Y);
                blueBar1 = new Vector2((portraitWidth + barWidth), pos1Y);
                itemRect1 = new Rect(portraitWidth + (2 * barWidth), pos1Y, portraitWidth * 0.3f, portraitWidth * 0.3f);
                desRect1 = new Rect(0, pos1Y + portraitWidth, 2 * portraitWidth, Screen.height * 0.09f);
                gc.player1.GetComponent<CureCounter>().center = new Rect(Screen.width * 0.45f, Screen.height * 0.73f, Screen.width * 0.09f, Screen.width * 0.09f);
                p1Skill1Rect = new Rect(portraitWidth + (2 * barWidth), pos1Y + (portraitWidth * 0.3f), portraitWidth * 0.3f, portraitWidth * 0.3f);
                p1Skill2Rect = new Rect(portraitWidth + (2 * barWidth), pos1Y + (2 * portraitWidth * 0.3f), portraitWidth * 0.3f, portraitWidth * 0.3f);

            }
            else if (noPlayers == 3)
            {
                float pos1Y = Screen.height * 0.5f;
                rect1 = new Rect(0, pos1Y, portraitWidth, portraitWidth);
                redBar1 = new Rect(portraitWidth, pos1Y, barWidth, portraitWidth);
                greenBar1 = new Vector2(portraitWidth, pos1Y);
                blueBar1 = new Vector2((portraitWidth + barWidth), pos1Y);
                gc.player1.GetComponent<CureCounter>().center = new Rect(Screen.width * 0.45f, Screen.height * 0.73f, Screen.width * 0.09f, Screen.width * 0.09f);


                float pos2Y = 0;
                rect2 = new Rect(0, pos2Y, portraitWidth, portraitWidth);
                redBar2 = new Rect(portraitWidth, pos2Y, barWidth, portraitWidth);
                greenBar2 = new Vector2(portraitWidth, pos2Y);
                blueBar2 = new Vector2(portraitWidth + barWidth, pos2Y);
                gc.player2.GetComponent<CureCounter>().center = new Rect(Screen.width * 0.23f, Screen.height * 0.23f, Screen.width * 0.08f, Screen.width * 0.09f);


                float pos3Y = 0;
                rect3 = new Rect(Screen.width * 0.5f, pos3Y, portraitWidth, portraitWidth);
                redBar3 = new Rect(Screen.width * 0.5f + portraitWidth, pos3Y, barWidth, portraitWidth);
                greenBar3 = new Vector2(Screen.width * 0.5f + portraitWidth, pos3Y);
                blueBar3 = new Vector2((Screen.width * 0.5f + portraitWidth + barWidth), pos3Y);
                gc.player3.GetComponent<CureCounter>().center = new Rect(Screen.width * 0.73f, Screen.height * 0.23f, Screen.width * 0.08f, Screen.width * 0.09f);
                //      Debug.Log(blueBar3.x);
            }
            else if (noPlayers == 4)
            {
                float pos1Y = 0;
                rect1 = new Rect(0, pos1Y, portraitWidth, portraitWidth);
                redBar1 = new Rect(portraitWidth, pos1Y, barWidth, portraitWidth);
                greenBar1 = new Vector2(portraitWidth, pos1Y);
                blueBar1 = new Vector2(portraitWidth + barWidth, pos1Y);
                gc.player3.GetComponent<CureCounter>().center = new Rect(Screen.width * 0.23f, Screen.height * 0.23f, Screen.width * 0.08f, Screen.width * 0.09f);

                float pos2Y = Screen.height * 0.5f;
                rect2 = new Rect(0, pos2Y, portraitWidth, portraitWidth);
                redBar2 = new Rect(portraitWidth, pos2Y, barWidth, portraitWidth);
                greenBar2 = new Vector2(portraitWidth, pos2Y);
                blueBar2 = new Vector2((portraitWidth + barWidth), pos2Y);
                gc.player2.GetComponent<CureCounter>().center = new Rect(Screen.width * 0.23f, Screen.height * 0.73f, Screen.width * 0.08f, Screen.width * 0.09f);


                float pos3Y = 0;
                rect3 = new Rect(Screen.width * 0.5f, pos3Y, portraitWidth, portraitWidth);
                redBar3 = new Rect(Screen.width * 0.5f + portraitWidth, pos3Y, barWidth, portraitWidth);
                greenBar3 = new Vector2(Screen.width * 0.5f + portraitWidth, pos3Y);
                blueBar3 = new Vector2((Screen.width * 0.5f + portraitWidth + barWidth), pos3Y);
                gc.player3.GetComponent<CureCounter>().center = new Rect(Screen.width * 0.73f, Screen.height * 0.23f, Screen.width * 0.08f, Screen.width * 0.09f);

                float pos4Y = Screen.height * 0.5f;
                rect4 = new Rect(Screen.width * 0.5f, pos4Y, portraitWidth, portraitWidth);
                redBar4 = new Rect(Screen.width * 0.5f + portraitWidth, pos4Y, barWidth, portraitWidth);
                greenBar4 = new Vector2(Screen.width * 0.5f + portraitWidth, pos4Y);
                blueBar4 = new Vector2((Screen.width * 0.5f + portraitWidth + barWidth), pos4Y);
                gc.player4.GetComponent<CureCounter>().center = new Rect(Screen.width * 0.73f, Screen.height * 0.73f, Screen.width * 0.08f, Screen.width * 0.09f);
            }

        }


    }



    // Update is called once per frame
    private void OnGUI()
    {
        if (pic1 != null)
        {
            PlayerStats pa1 = gc.player1.GetComponent<PlayerStats>();
            //     GUI.Button(new Rect(Screen.width * 0.1f, Screen.height * 0.92f, Screen.width * 0.9f, Screen.height * 0.8f), debugGUI, myStyle);
            GUI.DrawTexture(rect1, pic1);
            GUI.DrawTexture(redBar1, redBar);
            //   GUI.DrawTexture(new Rect(portraitWidth, 0, barWidth, pa1.greenBar), greenBar);
            GUI.DrawTexture(new Rect(greenBar1.x, greenBar1.y, barWidth, pa1.greenBar), greenBar);
            //   Debug.Log(pa1.blueBar);
            GUI.DrawTexture(new Rect(blueBar1.x, blueBar1.y, barWidth, pa1.blueBar), blueBar);
            if (itemTextures[0] != null)
            {
                GUI.DrawTexture(itemRect1, itemTextures[0]);
            }
            if (timers[0] + 2 > Time.timeSinceLevelLoad)
            {
                GUI.Label(desRect1, descriptions[0]);
            }


            if (skill1Cool[0] <= 0)
            {
                GUI.DrawTexture(p1Skill1Rect, skill1TexActive[0]);
            }
            else
            {
                GUI.DrawTexture(p1Skill1Rect, skill1TexNo[0]);
                int timeSkill = (int)(skill1Cool[0]);
                GUI.Label(p1Skill1Rect, timeSkill.ToString(), myStyle);
            }

            if (skill2Cool[0] <= 0)
            {
                GUI.DrawTexture(p1Skill2Rect, skill2TexActive[0]);
            }
            else
            {
                GUI.DrawTexture(p1Skill2Rect, skill2TexNo[0]);
                int timeSkill = (int)(skill2Cool[0]);
                GUI.Label(p1Skill2Rect, timeSkill.ToString(), myStyle);
            }
        }


        //    GUI.DrawTexture(new Rect(0, portraitWidth, Screen.width * 0.02f, Screen.width * 0.02f), coin);
        //    GUI.Label (new Rect(Screen.width * 0.03f, portraitWidth, Screen.width * 0.10f, Screen.height * 0.04f), pa1.coins.ToString(), myStyle);



        if (pic2 != null)
        {
            PlayerStats pa2 = gc.player2.GetComponent<PlayerStats>();
            GUI.DrawTexture(rect2, pic2);
            GUI.DrawTexture(redBar2, redBar);
            GUI.DrawTexture(new Rect(greenBar2.x, greenBar2.y, barWidth, pa2.greenBar), greenBar);
            GUI.DrawTexture(new Rect(blueBar2.x, blueBar2.y, barWidth, gc.player2.GetComponent<PlayerStats>().blueBar), blueBar);
            //     GUI.DrawTexture(new Rect(Screen.width - (Screen.width * 0.02f), portraitWidth, Screen.width * 0.02f, Screen.width * 0.02f), coin);
            //      GUI.Label(new Rect(Screen.width - (Screen.width * 0.05f), portraitWidth, Screen.width * 0.10f, Screen.height * 0.04f), pa2.coins.ToString(), myStyle);

            if (itemTextures[1] != null)
            {
                GUI.DrawTexture(itemRect2, itemTextures[1]);
      //          Debug.Log(itemTextures[1].name + "/" + itemRect2);
            }
            if (timers[1] + 2 > Time.timeSinceLevelLoad)
            {
                GUI.Label(desRect2, descriptions[1]);
            }

            if (skill1Cool[1] <= 0)
            {
                GUI.DrawTexture(p2Skill1Rect, skill1TexActive[1]);
            }
            else
            {
                GUI.DrawTexture(p2Skill1Rect, skill1TexNo[1]);
                int timeSkill = (int)(skill1Cool[1]);
                GUI.Label(p2Skill1Rect, timeSkill.ToString(), myStyle);
            }

            if (skill2Cool[1] <= 0)
            {
                GUI.DrawTexture(p2Skill2Rect, skill2TexActive[1]);
            }
            else
            {
                GUI.DrawTexture(p2Skill2Rect, skill2TexNo[1]);
                int timeSkill = (int)(skill2Cool[1]);
                GUI.Label(p2Skill2Rect, timeSkill.ToString(), myStyle);
            }



        }
        if (pic3 != null)
        {
            
            PlayerStats pa3 = gc.player3.GetComponent<PlayerStats>();
            GUI.DrawTexture(rect3, pic3);
            GUI.DrawTexture(redBar3, redBar);
            GUI.DrawTexture(new Rect(greenBar3.x, greenBar3.y, barWidth, pa3.greenBar), greenBar);
            GUI.DrawTexture(new Rect(blueBar3.x, blueBar3.y, barWidth, pa3.blueBar), blueBar);
            if (itemTextures[2] != null)
            {
                GUI.DrawTexture(itemRect3, itemTextures[2]);
            }
            if (timers[2] + 2 > Time.timeSinceLevelLoad)
            {
                GUI.Label(desRect3, descriptions[2]);
            }


            if (skill1Cool[2] <= 0)
            {
                GUI.DrawTexture(p3Skill1Rect, skill1TexActive[0]);
            }
            else
            {
                GUI.DrawTexture(p3Skill1Rect, skill1TexNo[2]);
                int timeSkill = (int)(skill1Cool[2]);
                GUI.Label(p3Skill1Rect, timeSkill.ToString(), myStyle);
            }

            if (skill2Cool[2] <= 0)
            {
                GUI.DrawTexture(p3Skill2Rect, skill2TexActive[2]);
            }
            else
            {
                GUI.DrawTexture(p3Skill2Rect, skill2TexNo[2]);
                int timeSkill = (int)(skill2Cool[2]);
                GUI.Label(p3Skill2Rect, timeSkill.ToString(), myStyle);
            }
        }
        if (pic4 != null)
        {
            PlayerStats pa4 = gc.player4.GetComponent<PlayerStats>();
            GUI.DrawTexture(rect4, pic4);
            GUI.DrawTexture(redBar4, redBar);
            GUI.DrawTexture(new Rect(greenBar4.x, greenBar4.y, barWidth, pa4.greenBar), greenBar);
            GUI.DrawTexture(new Rect(blueBar4.x, blueBar4.y, barWidth, pa4.blueBar), blueBar);
            if (itemTextures[3] != null)
            {
                GUI.DrawTexture(itemRect4, itemTextures[3]);
            }
            if (timers[3] + 2 > Time.timeSinceLevelLoad)
            {
                GUI.Label(desRect4, descriptions[3]);
            }


            if (skill1Cool[3] <= 0)
            {
                GUI.DrawTexture(p4Skill1Rect, skill1TexActive[3]);
            }
            else
            {
                GUI.DrawTexture(p4Skill1Rect, skill1TexNo[3]);
                int timeSkill = (int)(skill1Cool[3]);
                GUI.Label(p4Skill1Rect, timeSkill.ToString(), myStyle);
            }

            if (skill2Cool[3] <= 0)
            {
                GUI.DrawTexture(p4Skill2Rect, skill2TexActive[3]);
            }
            else
            {
                GUI.DrawTexture(p4Skill2Rect, skill2TexNo[3]);
                int timeSkill = (int)(skill2Cool[3]);
                GUI.Label(p4Skill2Rect, timeSkill.ToString(), myStyle);
            }

        }

    }

    public void ChangeTexture (GameObject go, bool right)
    {
        if (gc.isRPG == false)
        {
            for (int cnt = 0; cnt < gc.players.Count; cnt++)
            {
                if (go == gc.players[cnt])
                {
                    PlayerStats ps = go.GetComponent<PlayerStats>();

                    if (ps.inventory.Count <= 1)
                    {

                    }
                    else
                    {
                        if (right == true)
                        {
                            //              Debug.Log(itemNos[cnt]);
                            if (itemNos[cnt] == ps.inventory.Count - 1)
                            {
                                itemNos[cnt] = 0;

                            }
                            else
                            {
                                itemNos[cnt]++;

                            }
                            string tempChain = ps.inventory[itemNos[cnt]];
                            itemTextures[cnt] = (Texture2D)(Resources.Load("Inventory/" + tempChain));
                        }
                        else
                        {
                            if (itemNos[cnt] == 0)
                            {
                                itemNos[cnt] = ps.inventory.Count - 1;

                            }
                            else
                            {
                                itemNos[cnt]--;

                            }
                            string tempChain = ps.inventory[itemNos[cnt]];
                            itemTextures[cnt] = (Texture2D)(Resources.Load("Inventory/" + tempChain));
                        }


                        //            Debug.Log(itemTextures[cnt]);
                        string descriptionLua = DialogueLua.GetActorField(ps.inventory[itemNos[cnt]], "short").asString;
                        //           Debug.Log(descriptionLua);
                        ChangeDescription(cnt, descriptionLua);
                    }


                }
            }      
        }
        else
        {
            bool nameExist = false;
            int tempCNT = -1;
            int objectToChangeCNT = 10;
            List<Transform> tempTa = new List<Transform>();
            for (int cnt = 0; cnt < quickSlots.childCount; cnt++)
            {
                Debug.Log(quickSlots.GetChild(cnt).name);

                if (quickSlots.GetChild(cnt).childCount > 0)
                {
                    if (quickSlots.GetChild(cnt).GetChild(0).name != "None")
                    {

                        tempTa.Add(quickSlots.GetChild(cnt).GetChild(0));
                        tempCNT++;
                        if (quickSlots.GetChild(cnt).GetChild(0).name == itemTextures[0].name)
                        {
                            objectToChangeCNT = tempCNT;
                        }

                    }
                }
                else
                {
                    GameObject tempGO = new GameObject();
                    tempGO.name = "None";
                    tempGO.transform.position = quickSlots.GetChild(cnt).position;
                    tempGO.transform.parent = quickSlots.GetChild(cnt);


                }
            
            }
            if (tempTa.Count > 0)
            {
                if (right == true)
                {
                    if (objectToChangeCNT >= tempTa.Count - 1)
                    {
                        objectToChangeCNT = 0;
                    }
                    else
                    {
                        objectToChangeCNT++;
                    }

                }
                else
                {
                    if (objectToChangeCNT <=  0)
                    {
                        objectToChangeCNT = tempTa.Count - 1;
                    }
                    else
                    {
                        objectToChangeCNT--;
                    }
                }



                for (int cntPlayer = 0; cntPlayer < gc.players.Count; cntPlayer++)
                {
                    itemTextures[cntPlayer] = (Texture2D)(Resources.Load("Inventory/" + tempTa[objectToChangeCNT].name));
                }
                
            }
        }
      
    }

    public void DropItem (GameObject go)
    {
        if (gc.isRPG == false)
        {
            for (int cnt = 0; cnt < gc.players.Count; cnt++)
            {
                if (go == gc.players[cnt])
                {
                    PlayerStats ps = go.GetComponent<PlayerStats>();

                    if (ps.inventory.Count >= 1)
                    {
                        Vector3 tempPos = go.transform.position + go.transform.forward;
                        string nameTexture = itemTextures[cnt].name;
                        GameObject itemTemp = Instantiate(Resources.Load("Items/" + nameTexture), tempPos, go.transform.rotation) as GameObject;
                        itemTemp.name = nameTexture;
                        Debug.Log(ps.inventory.Count);
                        ps.inventory.RemoveAt(itemNos[cnt]);
                        Debug.Log(ps.inventory.Count);

                        if (ps.inventory.Count == 0)
                        {
                            itemTextures[cnt] = null;
                        }
                        else if (ps.inventory.Count == 1)
                        {
                            itemNos[cnt] = 0;
                            string tempString = ps.inventory[itemNos[cnt]];
                            itemTextures[cnt] = (Texture2D)(Resources.Load("Inventory/" + tempString));
                        }
                        else
                        {
                            itemNos[cnt]--;
                            string tempString = ps.inventory[itemNos[cnt]];
                            itemTextures[cnt] = (Texture2D)(Resources.Load("Inventory/" + tempString));
                        }
                    }
                }

            }
        }

    }


    public void AddItem (GameObject go, string itemTemp)
    {
        for (int cnt = 0; cnt < gc.players.Count; cnt++)
        {
            if (go == gc.players[cnt])
            {
                PlayerStats ps = go.GetComponent<PlayerStats>();
                if (ps.inventory.Count ==0)
                {
                    itemNos[cnt] = 0;
                    itemTextures[cnt] = (Texture2D)(Resources.Load("Inventory/" + itemTemp));
                }
                Debug.Log(ps.inventory.Count);
                ps.inventory.Add(itemTemp);
                Debug.Log(ps.inventory.Count);

            }
        }

    }   


    public void UseItem (GameObject go)
    {
        PlayerStats ps = go.GetComponent<PlayerStats>();
        if (ps.inventory.Count > 0)
        {
            for (int cnt = 0; cnt < gc.players.Count; cnt++)
            {
                if (go == gc.players[cnt])
                {

                    Debug.Log(go.name + "/" + cnt + "/");
                    if (go.transform.Find(itemTextures[cnt].name) != null)
                    {

                        go.transform.Find(itemTextures[cnt].name).gameObject.SetActive(true);
                    }
                    else
                    {
                        GameObject itemTemp = Instantiate(Resources.Load("UseItem/" + itemTextures[cnt].name), go.transform.position, go.transform.rotation) as GameObject;
                        itemTemp.name = itemTextures[cnt].name;
                        itemTemp.transform.parent = go.transform;
                        itemTemp.SetActive(true);
                    }

                    ps.inventory.RemoveAt(itemNos[cnt]);
                    //          Debug.Log(ps.inventory.Count);
                    if (ps.inventory.Count == 0)
                    {
                        itemTextures[cnt] = null;
                    }
                    else if (ps.inventory.Count == 1)
                    {
                        itemNos[cnt] = 0;
                        string tempItem = ps.inventory[itemNos[cnt]];
                        itemTextures[cnt] = (Texture2D)(Resources.Load("Inventory/" + tempItem));
                    }
                    else
                    {
                        itemNos[cnt]--;
                        string tempItem = ps.inventory[itemNos[cnt]];
                        itemTextures[cnt] = (Texture2D)(Resources.Load("Inventory/" + tempItem));
                    }
                }
            }

        }


        /*
        if (go == gc.player1)
        {
            if (go.transform.Find (item1.name) != null)
            {
                go.transform.Find(item1.name).gameObject.SetActive(true);
            }
            else
            {
                GameObject itemTemp = Instantiate(Resources.Load("UseItem/" + item1.name), go.transform.position, go.transform.rotation) as GameObject;
                itemTemp.name = item1.name;
                itemTemp.transform.parent = go.transform;
                itemTemp.SetActive(true);

            }

            inventory1.RemoveAt(item1No);
            Debug.Log(inventory1.Count);
            if (inventory1.Count == 0)
            {
                item1 = null;
            }
            else if (inventory1.Count == 1)
            {
                item1No = 0;
                item1 = (Texture2D)(Resources.Load("Inventory/" + inventory1[item1No]));
            }
            else
            {
                item1No--;
                item1 = (Texture2D)(Resources.Load("Inventory/" + inventory1[item1No]));
            }

        }*/
    }

    private void ChangeDescription (int indexDescription, string newValue)
    {
        descriptions[indexDescription] = newValue;
        timers[indexDescription] = Time.timeSinceLevelLoad;

    }

}
