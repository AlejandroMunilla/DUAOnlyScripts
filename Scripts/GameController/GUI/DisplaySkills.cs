using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PixelCrushers.DialogueSystem;

public class DisplaySkills : MonoBehaviour
{
    private GUIStyle styleButton;
    private GUISkin mySkin;

    private bool ready1;
    private bool ready2;
    private bool ready3;
    private bool ready4;


    private int iconWidth;
    private int distanceIcons;
    private int picWidth;
    private int points1;
    private int points2;
    private int points3;
    private int points4;


    private string name1;
    private string name2;
    private string name3;
    private string name4;
    private string player1Path1Name;
    private string player2Path1Name;
    private string player3Path1Name;
    private string player4Path1Name;

    private string skillIDNo;
    private string skillIDno2;
    private string skillIDNoP2;
    private string skillIDno2P2;
    private string skillIDNoP3;
    private string skillIDno2P3;
    private string skillIDNoP4;
    private string skillIDno2P4;


    private string player1Path2Name;
    private string player2Path2Name;
    private string player3Path2Name;
    private string player4Path2Name;


    private Texture2D background;
    private Texture2D pic1;
    private Texture2D pic2;
    private Texture2D pic3;
    private Texture2D pic4;
    private Texture2D buttonChecked;
    private Texture2D buttonNoChecked;

    private Rect backgroundRect;
    private Rect name1Rect;
    private Rect name2Rect;
    private Rect name3Rect;
    private Rect name4Rect;
    private Rect pic1Rect;
    private Rect pic2Rect;
    private Rect pic3Rect;
    private Rect pic4Rect;
    private Rect player1Path1Rect;
    private Rect player2Path1Rect;
    private Rect player3Path1Rect;
    private Rect player4Path1Rect;

    private Rect player1Path2Rect;
    private Rect player2Path2Rect;
    private Rect player3Path2Rect;
    private Rect player4Path2Rect;

    private Rect readyButtonRect1;
    private Rect readyButtonRect2;
    private Rect readyButtonRect3;
    private Rect readyButtonRect4;
    private Rect box1;
    private Rect box2;
    private Rect box3;
    private Rect box4;
    private Rect level1Rect;
    private Rect level2Rect;
    private Rect level3Rect;
    private Rect level4Rect;


    private Rect toolTip1;
    private Rect toolTip2;
    private Rect toolTip3;
    private Rect toolTip4;

    private Vector2 p1Skill1Pos;
    private Vector2 p2Skill1Pos;
    private Vector2 p3Skill1Pos;
    private Vector2 p4Skill1Pos;

    private Vector2 p1Skill2Pos;
    private Vector2 p2Skill2Pos;
    private Vector2 p3Skill2Pos;
    private Vector2 p4Skill2Pos;

    private List<Texture2D> p1Skill1 = new List<Texture2D>();
    private List<Texture2D> p2Skill1 = new List<Texture2D>();
    private List<Texture2D> p3Skill1 = new List<Texture2D>();
    private List<Texture2D> p4Skill1 = new List<Texture2D>();

    private List<Texture2D> p1Skill2 = new List<Texture2D>();
    private List<Texture2D> p2Skill2 = new List<Texture2D>();
    private List<Texture2D> p3Skill2 = new List<Texture2D>();
    private List<Texture2D> p4Skill2 = new List<Texture2D>();
    private List<string> p1Skill1String = new List<string>();
    private List<string> p2Skill1String = new List<string>();
    private List<string> p3Skill1String = new List<string>();
    private List<string> p4Skill1String = new List<string>();

    private List<string> p1Skill2String = new List<string>(); 
    private List<string> p2Skill2String = new List<string>();
    private List<string> p3Skill2String = new List<string>();
    private List<string> p4Skill2String = new List<string>();

    private List<int> expList = new List<int>();
    private List<int> pointsList = new List<int>();
    private List<int> levelList = new List<int>();
    private List<int> nextExpList = new List<int>();

    private List<bool> readyList = new List<bool>();


    private PreGame pregame;
    private GameController gc;

    private int expPoints = 2;
    // Start is called before the first frame update
    void  OnEnable ()
    {
        pregame = GetComponent<PreGame>();
        gc = GetComponent<GameController>();
        background = (Texture2D)(Resources.Load("GUI/background"));
        buttonChecked = (Texture2D)(Resources.Load("GUI/Checked"));
        buttonNoChecked = (Texture2D)(Resources.Load("GUI/Empty"));

        mySkin = pregame.mySkin;
        styleButton = mySkin.GetStyle("button");
        styleButton.fontSize = (int)(Screen.height * 0.017f);
        styleButton.normal.textColor = Color.white;
        styleButton.alignment = TextAnchor.MiddleCenter;

        backgroundRect = new Rect(0, 0, Screen.width, Screen.height);
        pregame = GetComponent<PreGame>();

        iconWidth = (int)(Screen.height * 0.07f);
        picWidth = (int)(Screen.height * 0.10f);
        distanceIcons = iconWidth;

        int posY1 = (int)(Screen.height * 0.04f);
        int posY2 = (int)(Screen.height * 0.24f);
        int posY3 = (int)(Screen.height * 0.44f);
        int posY4 = (int)(Screen.height * 0.64f);

        pic1Rect = new Rect(0, posY1, picWidth, picWidth);
        pic2Rect = new Rect(0, posY2, picWidth, picWidth);
        pic3Rect = new Rect(0, posY3, picWidth, picWidth);
        pic4Rect = new Rect(0, posY4, picWidth, picWidth);

        p1Skill1Pos = new Vector2((int)(Screen.width * 0.3f), posY1);
        p2Skill1Pos = new Vector2((int)(Screen.width * 0.3f), posY2);
        p3Skill1Pos = new Vector2((int)(Screen.width * 0.3f), posY3);
        p4Skill1Pos = new Vector2((int)(Screen.width * 0.3f), posY4);

        p1Skill2Pos = new Vector2((int)(Screen.width * 0.6f), posY1);
        p2Skill2Pos = new Vector2((int)(Screen.width * 0.6f), posY2);
        p3Skill2Pos = new Vector2((int)(Screen.width * 0.6f), posY3);
        p4Skill2Pos = new Vector2((int)(Screen.width * 0.6f), posY4);

        toolTip1 = new Rect(p1Skill1Pos.x, iconWidth + Screen.height * 0.05f, Screen.width * 0.7f, Screen.height * 0.09f);
        toolTip2 = new Rect(p2Skill1Pos.x, iconWidth + Screen.height * 0.25f, Screen.width * 0.7f, Screen.height * 0.09f);
        toolTip3 = new Rect(p3Skill1Pos.x, iconWidth + Screen.height * 0.45f, Screen.width * 0.7f, Screen.height * 0.09f);
        toolTip4 = new Rect(p4Skill1Pos.x, iconWidth + Screen.height * 0.65f, Screen.width * 0.7f, Screen.height * 0.09f);

        player1Path1Rect = new Rect(p1Skill1Pos.x, Screen.height * 0.01f, Screen.width * 0.25f, Screen.width * 0.08f);
        player2Path1Rect = new Rect(p2Skill1Pos.x, Screen.height * 0.2f, Screen.width * 0.25f, Screen.width * 0.08f);
        player3Path1Rect = new Rect(p3Skill1Pos.x, Screen.height * 0.4f, Screen.width * 0.25f, Screen.width * 0.08f);
        player4Path1Rect = new Rect(p4Skill1Pos.x, Screen.height * 0.6f, Screen.width * 0.25f, Screen.width * 0.08f);

        player1Path2Rect = new Rect(Screen.width * 0.6f, Screen.height * 0.01f, Screen.width * 0.25f, Screen.width * 0.08f);
        player2Path2Rect = new Rect(Screen.width * 0.6f, Screen.height * 0.20f, Screen.width * 0.25f, Screen.width * 0.08f);
        player3Path2Rect = new Rect(Screen.width * 0.6f, Screen.height * 0.40f, Screen.width * 0.25f, Screen.width * 0.08f);
        player4Path2Rect = new Rect(Screen.width * 0.6f, Screen.height * 0.60f, Screen.width * 0.25f, Screen.width * 0.08f);

        box1 = new Rect(Screen.width * 0.22f, Screen.height * 0.06f, Screen.width * 0.05f, Screen.height * 0.08f);
        box2 = new Rect(Screen.width * 0.22f, Screen.height * 0.26f, Screen.width * 0.05f, Screen.height * 0.08f);
        box3 = new Rect(Screen.width * 0.22f, Screen.height * 0.46f, Screen.width * 0.05f, Screen.height * 0.08f);
        box4 = new Rect(Screen.width * 0.22f, Screen.height * 0.66f, Screen.width * 0.05f, Screen.height * 0.08f);


        readyButtonRect1 = new Rect(Screen.width * 0.1f, Screen.height * 0.06f, Screen.width * 0.12f, Screen.height * 0.08f);
        readyButtonRect2 = new Rect(Screen.width * 0.1f, Screen.height * 0.26f, Screen.width * 0.12f, Screen.height * 0.08f);
        readyButtonRect3 = new Rect(Screen.width * 0.1f, Screen.height * 0.46f, Screen.width * 0.12f, Screen.height * 0.08f);
        readyButtonRect4 = new Rect(Screen.width * 0.1f, Screen.height * 0.66f, Screen.width * 0.12f, Screen.height * 0.08f);

        level1Rect = new Rect(Screen.width * 0.1f, Screen.height * 0.01f, Screen.width * 0.15f, Screen.height * 0.08f);
        level2Rect = new Rect(Screen.width * 0.1f, Screen.height * 0.2f, Screen.width * 0.15f, Screen.height * 0.08f);
        level3Rect = new Rect(Screen.width * 0.1f, Screen.height * 0.4f, Screen.width * 0.15f, Screen.height * 0.08f);
        level4Rect = new Rect(Screen.width * 0.1f, Screen.height * 0.6f, Screen.width * 0.15f, Screen.height * 0.08f);

        name1Rect = new Rect(Screen.width * 0.01f, Screen.height * 0.01f, Screen.width * 0.15f, Screen.height * 0.08f);
        name2Rect = new Rect(Screen.width * 0.01f, Screen.height * 0.20f, Screen.width * 0.15f, Screen.height * 0.08f);
        name3Rect = new Rect(Screen.width * 0.01f, Screen.height * 0.4f, Screen.width * 0.15f, Screen.height * 0.08f);
        name4Rect = new Rect(Screen.width * 0.01f, Screen.height * 0.6f, Screen.width * 0.15f, Screen.height * 0.08f);

        name1 = pregame.chosen1;
        name2 = pregame.chosen2;
        name3 = pregame.chosen3;
        name4 = pregame.chosen4;
 
        pic1 = (Texture2D)(Resources.Load("Portraits/" + name1));
        skillIDNo = DialogueLua.GetActorField(name1, "skill1").asString;        
        skillIDno2 = DialogueLua.GetActorField(name1, "skill2").asString;
    //    Debug.Log(skillIDNo + "/" + skillIDno2);
        player1Path1Name = DialogueLua.GetActorField(skillIDNo, "Description").asString;
        player1Path2Name = DialogueLua.GetActorField(skillIDno2, "Description").asString;
        for (int cnt = 0; cnt < 4; cnt++)
        {
            int tempInt = cnt + 1;
            string tempString = tempInt.ToString();
            /// Player 1 ///

            string skillActive = DialogueLua.GetActorField(name1, "skill1/" + tempString).asString;
            if (skillActive == "Yes")
            {
                p1Skill1.Add((Texture2D)(Resources.Load("SKills/" + skillIDNo + "/" + tempInt.ToString())));
            }
            else
            {
                p1Skill1.Add((Texture2D)(Resources.Load("SKills/" + skillIDNo + "/" + (tempInt.ToString()) + "no")));
                p1Skill1[cnt].name = tempInt.ToString();
            }

            string skillActive2 = DialogueLua.GetActorField(name1, "skill2/" + tempString).asString;
            if (skillActive2 == "Yes")
            {

                p1Skill2.Add((Texture2D)(Resources.Load("SKills/" + skillIDno2 + "/" + tempInt.ToString())));
            }
            else
            {
                Debug.Log(skillIDno2);
                Debug.Log("SKills/" + skillIDno2 + "/" + (tempInt.ToString()) + "no");
                p1Skill2.Add((Texture2D)(Resources.Load("SKills/" + skillIDno2 + "/" + (tempInt.ToString()) + "no")));
                p1Skill2[cnt].name = tempInt.ToString();
            }
        }

        if (DialogueLua.GetActorField(pregame.chosen1, "skill1/5a").asString == "Yes")
        {
            p1Skill1.Add((Texture2D)(Resources.Load("SKills/" + skillIDNo + "/5a")));
        }
        else
        {
            p1Skill1.Add((Texture2D)(Resources.Load("SKills/" + skillIDNo + "/5ano")));
            p1Skill1[4].name = "5a";
        }

        if (DialogueLua.GetActorField(pregame.chosen1, "skill1/5b").asString == "Yes")
        {
            p1Skill1.Add((Texture2D)(Resources.Load("SKills/" + skillIDNo + "/5b")));
        }
        else
        {
            p1Skill1.Add((Texture2D)(Resources.Load("SKills/" + skillIDNo + "/5bno")));
            p1Skill1[5].name = "5b";
        }

        if (DialogueLua.GetActorField(pregame.chosen1, "skill2/5a").asString == "Yes")
        {
            p1Skill2.Add((Texture2D)(Resources.Load("SKills/" + skillIDno2 + "/5a")));
        }
        else
        {
            p1Skill2.Add((Texture2D)(Resources.Load("SKills/" + skillIDno2 + "/5ano")));
            p1Skill2[4].name = "5a";
        }

        if (DialogueLua.GetActorField(pregame.chosen1, "skill2/5b").asString == "Yes")
        {
            p1Skill2.Add((Texture2D)(Resources.Load("SKills/" + skillIDno2 + "/5b")));
        }
        else
        {
            p1Skill2.Add((Texture2D)(Resources.Load("SKills/" + skillIDno2 + "/5bno")));
            p1Skill2[5].name = "5b";
        }

        for (int cnt = 0; cnt < p1Skill1.Count; cnt++)
        {
            
            TextAsset myText = (TextAsset)(Resources.Load("Text/" + skillIDNo + "/en/" + p1Skill1[cnt].name));
            string myString = myText.text;
            p1Skill1String.Add(myString);
        }

        for (int cnt = 0; cnt < p1Skill2.Count; cnt++)
        {
   //         Debug.Log("Text/" + skillIDno2 + "/en/" + p1Skill2[cnt].name);
            TextAsset myText = (TextAsset)(Resources.Load("Text/" + skillIDno2 + "/en/" + p1Skill2[cnt].name));
            string myString = myText.text;
            p1Skill2String.Add(myString);
        }
        readyList.Add(false);


        if (name2 != "None")
        {
            readyList.Add(false);
            pic2 = (Texture2D)(Resources.Load("Portraits/" + name2));
            skillIDNoP2 = DialogueLua.GetActorField(name2, "skill1").asString;
            skillIDno2P2 = DialogueLua.GetActorField(name2, "skill2").asString;
            player2Path1Name = DialogueLua.GetActorField(skillIDNoP2, "Description").asString;
            player2Path2Name = DialogueLua.GetActorField(skillIDno2P2, "Description").asString;
     //       Debug.Log(player2Path1Name + "/" + player2Path2Name);

            for (int cnt = 0; cnt < 4; cnt++)
            {
                int tempInt = cnt + 1;
                string tempString = tempInt.ToString();
                string skillActiveP2 = DialogueLua.GetActorField(name2, "skill1/" + tempString).asString;
                if (skillActiveP2 == "Yes")
                {
                    p2Skill1.Add((Texture2D)(Resources.Load("SKills/" + skillIDNoP2 + "/" + tempInt.ToString())));
                }
                else
                {
                    p2Skill1.Add((Texture2D)(Resources.Load("SKills/" + skillIDNoP2 + "/" + (tempInt.ToString()) + "no")));
                    p2Skill1[cnt].name = tempInt.ToString();
                }

                string skillActive2P2 = DialogueLua.GetActorField(name2, "skill2/" + tempString).asString;
                if (skillActive2P2 == "Yes")
                {

                    p2Skill2.Add((Texture2D)(Resources.Load("SKills/" + skillIDno2P2 + "/" + tempInt.ToString())));
                }
                else
                {

                    p2Skill2.Add((Texture2D)(Resources.Load("SKills/" + skillIDno2P2 + "/" + (tempInt.ToString()) + "no")));
                    p2Skill2[cnt].name = tempInt.ToString();
                }
            }

            if (DialogueLua.GetActorField(name2, "skill1/5a").asString == "Yes")
            {
                p2Skill1.Add((Texture2D)(Resources.Load("SKills/" + skillIDNoP2 + "/5a")));
            }
            else
            {
                p2Skill1.Add((Texture2D)(Resources.Load("SKills/" + skillIDNoP2 + "/5ano")));
                p2Skill1[4].name = "5a";
            }

            if (DialogueLua.GetActorField(name2, "skill1/5b").asString == "Yes")
            {
                p2Skill1.Add((Texture2D)(Resources.Load("5b")));
            }
            else
            {
                p2Skill1.Add((Texture2D)(Resources.Load("SKills/" + skillIDNoP2 + "/5bno")));
                p2Skill1[5].name = "5b";
            }

            if (DialogueLua.GetActorField(name2, "skill2/5a").asString == "Yes")
            {
                p2Skill2.Add((Texture2D)(Resources.Load("SKills/" + skillIDno2P2 + "/5a")));
            }
            else
            {
                p2Skill2.Add((Texture2D)(Resources.Load("SKills/" + skillIDno2P2 + "/5ano")));
                p2Skill2[4].name = "5a";
            }

            if (DialogueLua.GetActorField(name2, "skill2/5b").asString == "Yes")
            {
                p2Skill2.Add((Texture2D)(Resources.Load("5b")));
            }
            else
            {
                p2Skill2.Add((Texture2D)(Resources.Load("SKills/" + skillIDno2P2 + "/5bno")));
                p2Skill2[5].name = "5b";
            }

            for (int cnt = 0; cnt < p2Skill1.Count; cnt++)
            {
                TextAsset myText = (TextAsset)(Resources.Load("Text/" + skillIDNoP2 + "/en/" + p2Skill1[cnt].name));
                string myString = myText.text;
                p2Skill1String.Add(myString);
            }

            for (int cnt = 0; cnt < p2Skill2.Count; cnt++)
            {
                TextAsset myText = (TextAsset)(Resources.Load("Text/" + skillIDno2P2 + "/en/" + p2Skill1[cnt].name));
                string myString = myText.text;
                p2Skill2String.Add(myString);
            }
        }

        if (name3 != "None")
        {
            readyList.Add(false);
            pic3 = (Texture2D)(Resources.Load("Portraits/" + name3));
            skillIDNoP3 = DialogueLua.GetActorField(name3, "skill1").asString;
            skillIDno2P3 = DialogueLua.GetActorField(name3, "skill2").asString;
            player3Path1Name = DialogueLua.GetActorField(skillIDNoP3, "Description").asString;
            player3Path2Name = DialogueLua.GetActorField(skillIDno2P3, "Description").asString;

            for (int cnt = 0; cnt < 4; cnt++)
            {
                int tempInt = cnt + 1;
                string tempString = tempInt.ToString();
        //        Debug.Log(DialogueLua.GetActorField(name3, "skill1/" + tempString).asString);
                string skillActiveP3 = DialogueLua.GetActorField(name3, "skill1/" + tempString).asString;
                if (skillActiveP3 == "Yes")
                {
                    p3Skill1.Add((Texture2D)(Resources.Load("SKills/" + skillIDNoP3 + "/" + tempInt.ToString())));
                }
                else
                {
                    p3Skill1.Add((Texture2D)(Resources.Load("SKills/" + skillIDNoP3 + "/" + (tempInt.ToString()) + "no")));
                    p3Skill1[cnt].name = tempInt.ToString();
                }

                string skillActive2P3 = DialogueLua.GetActorField(name3, "skill2/" + tempString).asString;
                if (skillActive2P3 == "Yes")
                {

                    p3Skill2.Add((Texture2D)(Resources.Load("SKills/" + skillIDno2P3 + "/" + tempInt.ToString())));
                }
                else
                {

                    p3Skill2.Add((Texture2D)(Resources.Load("SKills/" + skillIDno2P3 + "/" + (tempInt.ToString()) + "no")));
                    p3Skill2[cnt].name = tempInt.ToString();
                }

            }

            if (DialogueLua.GetActorField(name3, "skill1/5a").asString == "Yes")
            {
                p3Skill1.Add((Texture2D)(Resources.Load("SKills/" + skillIDNoP3 + "/5a")));
            }
            else
            {
                p3Skill1.Add((Texture2D)(Resources.Load("SKills/" + skillIDNoP3 + "/5ano")));
                p3Skill1[4].name = "5a";
            }

            if (DialogueLua.GetActorField(name3, "skill1/5b").asString == "Yes")
            {
                p3Skill1.Add((Texture2D)(Resources.Load("5b")));
            }
            else
            {
                p3Skill1.Add((Texture2D)(Resources.Load("SKills/" + skillIDNoP3 + "/5bno")));
                p3Skill1[5].name = "5b";
            }

            if (DialogueLua.GetActorField(name3, "skill2/5a").asString == "Yes")
            {
                p3Skill2.Add((Texture2D)(Resources.Load("SKills/" + skillIDno2P3 + "/5a")));
            }
            else
            {
                p3Skill2.Add((Texture2D)(Resources.Load("SKills/" + skillIDno2P3 + "/5ano")));
                p3Skill2[4].name = "5a";
            }

            if (DialogueLua.GetActorField(name3, "skill2/5b").asString == "Yes")
            {
                p3Skill2.Add((Texture2D)(Resources.Load("5b")));
            }
            else
            {
                p3Skill2.Add((Texture2D)(Resources.Load("SKills/" + skillIDno2P3 + "/5bno")));
                p3Skill2[5].name = "5b";
            }

            for (int cnt = 0; cnt < p3Skill1.Count; cnt++)
            {
                TextAsset myText = (TextAsset)(Resources.Load("Text/" + skillIDNoP3 + "/en/" + p3Skill1[cnt].name));
                string myString = myText.text;
                p3Skill1String.Add(myString);
            }

            for (int cnt = 0; cnt < p1Skill2.Count; cnt++)
            {
                TextAsset myText = (TextAsset)(Resources.Load("Text/" + skillIDno2P3 + "/en/" + p3Skill1[cnt].name));
                string myString = myText.text;
                p3Skill2String.Add(myString);
            }
        }

        if (name4 != "None")
        {
            readyList.Add(false);
            pic4 = (Texture2D)(Resources.Load("Portraits/" + name4));
            skillIDNoP4 = DialogueLua.GetActorField(name4, "skill1").asString;
            skillIDno2P4 = DialogueLua.GetActorField(name4, "skill2").asString;
            player4Path1Name = DialogueLua.GetActorField(skillIDNoP4, "Description").asString;
            player4Path2Name = DialogueLua.GetActorField(skillIDno2P4, "Description").asString;

            for (int cnt = 0; cnt < 4; cnt++)
            {
                int tempInt = cnt + 1;
                string tempString = tempInt.ToString();
                string skillActiveP4 = DialogueLua.GetActorField(name3, "skill1/" + tempString).asString;
                if (skillActiveP4 == "Yes")
                {
                    p4Skill1.Add((Texture2D)(Resources.Load("SKills/" + skillIDNoP4 + "/" + tempInt.ToString())));
                }
                else
                {
                    p4Skill1.Add((Texture2D)(Resources.Load("SKills/" + skillIDNoP4 + "/" + (tempInt.ToString()) + "no")));
                    p4Skill1[cnt].name = tempInt.ToString();
                }

                string skillActive2P4 = DialogueLua.GetActorField(name4, "skill2/" + tempString).asString;
                if (skillActive2P4 == "Yes")
                {

                    p4Skill2.Add((Texture2D)(Resources.Load("SKills/" + skillIDno2P4 + "/" + tempInt.ToString())));
                }
                else
                {

                    p4Skill2.Add((Texture2D)(Resources.Load("SKills/" + skillIDno2P4 + "/" + (tempInt.ToString()) + "no")));
                    p4Skill2[cnt].name = tempInt.ToString();
                }
            }
            if (DialogueLua.GetActorField(name4, "skill1/5a").asString == "Yes")
            {
                p4Skill1.Add((Texture2D)(Resources.Load("SKills/" + skillIDNoP4 + "/5a")));
            }
            else
            {
                p4Skill1.Add((Texture2D)(Resources.Load("SKills/" + skillIDNoP4 + "/5ano")));
                p4Skill1[4].name = "5a";
            }

            if (DialogueLua.GetActorField(name4, "skill1/5b").asString == "Yes")
            {
                p4Skill1.Add((Texture2D)(Resources.Load("5b")));
            }
            else
            {
                p4Skill1.Add((Texture2D)(Resources.Load("SKills/" + skillIDNoP4 + "/5bno")));
                p4Skill1[5].name = "5b";
            }

            if (DialogueLua.GetActorField(name4, "skill2/5a").asString == "Yes")
            {
                p4Skill2.Add((Texture2D)(Resources.Load("SKills/" + skillIDno2P4 + "/5a")));
            }
            else
            {
                p4Skill2.Add((Texture2D)(Resources.Load("SKills/" + skillIDno2P4 + "/5ano")));
                p4Skill2[4].name = "5a";
            }

            if (DialogueLua.GetActorField(name4, "skill2/5b").asString == "Yes")
            {
                p4Skill2.Add((Texture2D)(Resources.Load("5b")));
            }
            else
            {
                p4Skill2.Add((Texture2D)(Resources.Load("SKills/" + skillIDno2P4 + "/5bno")));
                p4Skill2[5].name = "5b";
            }

            for (int cnt = 0; cnt < p4Skill1.Count; cnt++)
            {
                TextAsset myText = (TextAsset)(Resources.Load("Text/" + skillIDNoP4 + "/en/" + p4Skill1[cnt].name));
                string myString = myText.text;
                p4Skill1String.Add(myString);
            }

            for (int cnt = 0; cnt < p4Skill2.Count; cnt++)
            {
                TextAsset myText = (TextAsset)(Resources.Load("Text/" + skillIDno2P4 + "/en/" + p4Skill1[cnt].name));
                string myString = myText.text;
                p4Skill2String.Add(myString);
            }
        } 



        GetExperience(name1, 0);
        if (name2 != "None")
        {
            GetExperience(name2, 1);            
        }
        if (name3 != "None")
        {
            GetExperience(name3, 2);
        }
        if (name4 != "None")
        {
            GetExperience(name4, 3);
        }


        /*
        GetExperience(600);        
        GetExperience(1200);
        GetExperience(3499);
        GetExperience(4900);
        GetExperience(7000);
        GetExperience(8600);
        GetExperience(12300);
        GetExperience(15000);
        GetExperience(21000);
        GetExperience(32000);*/


        //    p1Skill1String.Add((Texture2D)(Resources.Load("SKills/2000/2")));

    }

    private void GetExperience (string playerName, int playerCNT)
    {
        int luaExperience = DialogueLua.GetActorField(playerName, "expArena").asInt;
  //      int luaExperience = 35000;
         expList.Add(luaExperience);

        //1000   2000   3500   5500  8000  11000   14500   18500   23000    26000
        int nextLevelExp = 500;
        int arimeticIncrease = 0;
        bool foundLevel = false;
        int levelTemp = 1;
        for (int cnt = 1; cnt<11; cnt++)
        {
        //    Debug.Log(cnt);
            if (foundLevel == false)
            {
                arimeticIncrease =  (cnt * 500);
                nextLevelExp = nextLevelExp + arimeticIncrease ;
                if (luaExperience >= 26000)
                {
                    luaExperience = 26000;
                    levelTemp = 10;
                    foundLevel = true;
                    nextLevelExp = 26000;
                }

                else if (nextLevelExp > luaExperience)
                {
                    levelTemp = cnt;
                    foundLevel = true;                    
                }
            }          
            
            
        }

    //    Debug.Log(luaExperience + "/" + nextLevelExp + "/"  + levelTemp);

        int levelSpent = DialogueLua.GetActorField(playerName, "levelSpent").asInt;
        int tempPoints = levelTemp - levelSpent;
        pointsList.Add(tempPoints);
        levelList.Add(levelTemp);
        expList.Add(luaExperience);
        nextExpList.Add(nextLevelExp);
        DialogueLua.SetActorField(playerName, "levelArena", levelTemp);


    }


    public void ShowSkills ()
    {
        GUI.DrawTexture(backgroundRect, background);
        ShowPlayer1();

        if (name2 != "None")
        {
            ShowPlayer2();
        }

        if (name3 != "None")
        {
            ShowPlayer3();
        }

        if (name4 != "None")
        {
            ShowPlayer4();
        }

        bool wholeReady = true;
        foreach (bool r in readyList)
        {
            if (r == false)
            {
                wholeReady = false;
            }
        }
  //      Debug.Log(wholeReady);

        if (wholeReady == true)
        {
            pregame.SaveData();
        }
    }

    private void ShowPlayer1 ()
    {

        GUI.Label(name1Rect, name1, pregame.myStyle);
        GUI.Label(player1Path1Rect, player1Path1Name, pregame.myStyle);
        GUI.Label(level1Rect, "LEVEL: " +  levelList[0].ToString() + "  (" + expList[0] + "/" + nextExpList[0] + ")", pregame.myStyle);
        GUI.DrawTexture(pic1Rect, pic1);

        if (GUI.Button(readyButtonRect1, "READY", styleButton))
        {
       //     ready1 = !ready1;
            readyList[0] = !readyList[0];
        }
        if (readyList[0] == false)
        {
            GUI.DrawTexture(box1, buttonNoChecked);
        }
        else
        {
            GUI.DrawTexture(box1, buttonChecked);
        }
        

        for (int cnt = 0; cnt < p1Skill1.Count - 2; cnt++)
        {
            //         GUI.Button(new Rect(p1Skill1Pos.x + (cnt * iconWidth), p1Skill1Pos.y, iconWidth, iconWidth), p1Skill1[cnt]);
            if (GUI.Button(new Rect(p1Skill1Pos.x + (cnt * iconWidth), p1Skill1Pos.y, iconWidth, iconWidth), new GUIContent(p1Skill1[cnt], p1Skill1String[cnt])))
            {
                if (pointsList[0] > 0)
                {
                    //         if (DialogueLua.GetActorField("Rose", "skill1/5a").asString == "Yes")


                    string skillID = "skill1/" + p1Skill1[cnt].name;

                    bool previousUnlocked = false;
                    if (cnt == 0)
                    {
                        previousUnlocked = true;
                    }
                    else if (cnt > 0)
                    {
                        int tempCNT = cnt - 1;
                        string skillIDprevious = "skill1/" + p1Skill1[tempCNT].name;

                        if (DialogueLua.GetActorField(pregame.chosen1, skillIDprevious).asString == "Yes")
                        {
                            previousUnlocked = true;
                        }
                    }

                    if (previousUnlocked == true)
                    {
                        if (DialogueLua.GetActorField(pregame.chosen1, skillID).asString != "Yes")
                        {
                            int tempPoints = pointsList[0] - 1;
                            pointsList[0] = tempPoints;
                            DialogueLua.SetActorField(pregame.chosen1, skillID, "Yes");
                            int levelSpent = DialogueLua.GetActorField(name1, "levelSpent").asInt + 1;
                            DialogueLua.SetActorField(name1, "levelSpent", levelSpent);

                            p1Skill1[cnt] = (Texture2D)(Resources.Load("SKills/" + skillIDNo + "/" + p1Skill1[cnt].name));
                        }
                    }

                    //              string skillIDbefore = "skill1/ + "
                    //             Debug.Log(skillID);

                }
                else
                {
                    DialogueManager.ShowAlert("Not skill points left");
                    Debug.Log("No points");
                }
            }

        }

        if (GUI.Button(new Rect(p1Skill1Pos.x + (4 * iconWidth), 0, iconWidth, iconWidth), new GUIContent(p1Skill1[4], p1Skill1String[4])))
        {
            if (pointsList[0] > 0)
            {
                string skillID = "skill1/" + p1Skill1[4].name;
                string skillID2 = "skill1/" + p1Skill1[5].name;
                string skillIDprevious = "skill1/" + p1Skill1[3].name;

                Debug.Log(DialogueLua.GetActorField(name1, skillIDprevious).asString);
                if (DialogueLua.GetActorField(name1, skillIDprevious).asString == "Yes")
                {
                    //       Debug.Log(skillID);
                    if (DialogueLua.GetActorField(name1, skillID).asString != "Yes" && DialogueLua.GetActorField(name1, skillID2).asString != "Yes")
                    {
                        int tempPoints = pointsList[0] - 1;
                        pointsList[0] = tempPoints;
                        DialogueLua.SetActorField(pregame.chosen1, skillID, "Yes");
                        int levelSpent = DialogueLua.GetActorField(name1, "levelSpent").asInt + 1;
                        DialogueLua.SetActorField(name1, "levelSpent", levelSpent);
                        p1Skill1[4] = (Texture2D)(Resources.Load("SKills/"+ skillIDNo + "/" + p1Skill1[4].name));
                    }
                }
            }
            else
            {
                DialogueManager.ShowAlert("Not skill points left");
                Debug.Log("No points");
            }
        }

        if (GUI.Button(new Rect(p1Skill1Pos.x + (4 * iconWidth), p1Skill1Pos.y * 1.5f, iconWidth, iconWidth), new GUIContent(p1Skill1[5], p1Skill1String[5])))
        {
            if (pointsList[0] > 0)
            {
                string skillID = "skill1/" + p1Skill1[4].name;
                string skillID2 = "skill1/" + p1Skill1[5].name;
    //            Debug.Log(skillID2);
                string skillIDprevious = "skill1/" + p1Skill1[3].name;

                if (DialogueLua.GetActorField(pregame.chosen1, skillIDprevious).asString == "Yes")
                {
                    if (DialogueLua.GetActorField(pregame.chosen1, skillID2).asString != "Yes" && DialogueLua.GetActorField(pregame.chosen1, skillID).asString != "Yes")
                    {
                        int tempPoints = pointsList[0] - 1;
                        pointsList[0] = tempPoints;
                        DialogueLua.SetActorField(pregame.chosen1, skillID2, "Yes");
                        int levelSpent = DialogueLua.GetActorField(name1, "levelSpent").asInt + 1;
                        DialogueLua.SetActorField(name1, "levelSpent", levelSpent);
                        p1Skill1[5] = (Texture2D)(Resources.Load("SKills/" + skillIDNo + "/" + p1Skill1[5].name));
                    }
                }
            }
            else
            {
                DialogueManager.ShowAlert("Not skill points left");
                Debug.Log("No points");
            }
        }



        GUI.Label(player1Path2Rect, player1Path2Name, pregame.myStyle);
        for (int cnt = 0; cnt < p1Skill2.Count - 2; cnt++)
        {
            //         GUI.Button(new Rect(p1Skill1Pos.x + (cnt * iconWidth), p1Skill1Pos.y, iconWidth, iconWidth), p1Skill1[cnt]);
            if (GUI.Button(new Rect(p1Skill2Pos.x + (cnt * iconWidth), p1Skill1Pos.y, iconWidth, iconWidth), new GUIContent(p1Skill2[cnt], p1Skill2String[cnt])))
            {
                if (pointsList[0] > 0)
                {

                    string skillID2 = "skill2/" + p1Skill2[cnt].name;
                    bool previousUnlocked = false;
                    if (cnt == 0)
                    {
                        previousUnlocked = true;
                    }
                    else if (cnt > 0)
                    {
                        int tempCNT = cnt - 1;
                        string skillIDprevious = "skill2/" + p1Skill2[tempCNT].name;

                        if (DialogueLua.GetActorField(pregame.chosen1, skillIDprevious).asString == "Yes")
                        {
                            previousUnlocked = true;
                        }
                    }
                    //           Debug.Log(skillID);
                    if (DialogueLua.GetActorField(pregame.chosen1, skillID2).asString != "Yes" && previousUnlocked == true)
                    {
                        int tempPoints = pointsList[0] - 1;
                        pointsList[0] = tempPoints;
                        DialogueLua.SetActorField(pregame.chosen1, skillID2, "Yes");
                        int levelSpent = DialogueLua.GetActorField(name1, "levelSpent").asInt + 1;
                        DialogueLua.SetActorField(name1, "levelSpent", levelSpent);
                        p1Skill2[cnt] = (Texture2D)(Resources.Load("SKills/"  + skillIDno2 + "/" + p1Skill2[cnt].name));
                    }
                }
                else
                {
                    DialogueManager.ShowAlert("Not skill points left");
                    Debug.Log("No points");
                }
            }

        }

        if (GUI.Button(new Rect(p1Skill2Pos.x + (4 * iconWidth), 0, iconWidth, iconWidth), new GUIContent(p1Skill2[4], p1Skill2String[4])))
        {
            if (pointsList[0] > 0)
            {
                string skillID = "skill2/" + p1Skill2[4].name;
                string skillID2 = "skill2/" + p1Skill2[5].name;
      //          Debug.Log(skillID);
                string skillIDprevious = "skill2/" + p1Skill2[3].name;

                if (DialogueLua.GetActorField(pregame.chosen1, skillIDprevious).asString == "Yes")
                {
                    if (DialogueLua.GetActorField(name1, skillID).asString != "Yes" && DialogueLua.GetActorField(pregame.chosen1, skillID2).asString != "Yes")
                    {
                        int tempPoints = pointsList[0] - 1;
                        pointsList[0] = tempPoints;
                        DialogueLua.SetActorField(pregame.chosen1, skillID, "Yes");
                        int levelSpent = DialogueLua.GetActorField(name1, "levelSpent").asInt + 1;
                        DialogueLua.SetActorField(name1, "levelSpent", levelSpent);
                        p1Skill2[4] = (Texture2D)(Resources.Load("SKills/" + skillIDno2 + "/" + p1Skill2[4].name));
                    }
                }

            }
            else
            {
                DialogueManager.ShowAlert("Not skill points left");
                Debug.Log("No points");
            }
        }

        if (GUI.Button(new Rect(p1Skill2Pos.x + (4 * iconWidth), p1Skill1Pos.y * 1.5f, iconWidth, iconWidth), new GUIContent(p1Skill2[5], p1Skill2String[5])))
        {
            if (pointsList [0] > 0)
            {
                string skillID = "skill2/" + p1Skill2[4].name;
                string skillID2 = "skill2/" + p1Skill2[5].name;
               
                string skillIDprevious = "skill2/" + p1Skill2[3].name;
   //             Debug.Log(skillID + DialogueLua.GetActorField(name1, skillIDprevious).asString);
                if (DialogueLua.GetActorField(name1, skillIDprevious).asString == "Yes")
                {
                    if (DialogueLua.GetActorField(pregame.chosen1, skillID).asString != "Yes" && DialogueLua.GetActorField(pregame.chosen1, skillID2).asString != "Yes")
                    {
                        int tempPoints = pointsList[0] - 1;
                        pointsList[0] = tempPoints;
                        DialogueLua.SetActorField(name1, skillID2, "Yes");
                        int levelSpent = DialogueLua.GetActorField(name1, "levelSpent").asInt + 1;
                        DialogueLua.SetActorField(name1, "levelSpent", levelSpent);
                        p1Skill2[5] = (Texture2D)(Resources.Load("SKills/" + skillIDno2 + "/" + p1Skill2[5].name));
                    }
                }
            }
            else
            {
                DialogueManager.ShowAlert("Not skill points left");
                Debug.Log("No points");
            }
        }

        GUI.Label(new Rect(Screen.width * 0.9f, p1Skill1Pos.y, Screen.width * 0.14f, Screen.height * 0.08f), "Points: " + pointsList[0].ToString(), pregame.myStyle);
   //     Debug.Log(DialogueLua.GetActorField(name1, "levelSpent").asInt);

        GUI.Label(toolTip1, GUI.tooltip);
        GUI.tooltip = null;
    }

    private void ShowPlayer2()
    {

        GUI.Label(name2Rect, name2, pregame.myStyle);
        GUI.Label(player2Path1Rect, player2Path1Name, pregame.myStyle);
        GUI.Label(level2Rect, "LEVEL: " + levelList[1].ToString() + "  (" + expList[1] + "/" + nextExpList[1] + ")", pregame.myStyle);
        GUI.DrawTexture(pic2Rect, pic2);

        if (GUI.Button(readyButtonRect2, "READY", styleButton))
        {
     //       ready2 = !ready2;
            readyList[1] = !readyList[1];
        }
        if (readyList[1] == false)
        {
            GUI.DrawTexture(box2, buttonNoChecked);
        }
        else
        {
            GUI.DrawTexture(box2, buttonChecked);
        }

        for (int cnt = 0; cnt < p2Skill1.Count - 2; cnt++)
        {
            //         GUI.Button(new Rect(p1Skill1Pos.x + (cnt * iconWidth), p1Skill1Pos.y, iconWidth, iconWidth), p1Skill1[cnt]);
            if (GUI.Button(new Rect(p2Skill1Pos.x + (cnt * iconWidth), p2Skill1Pos.y, iconWidth, iconWidth), new GUIContent(p2Skill1[cnt], p2Skill1String[cnt])))
            {
                if (pointsList[1] > 0)
                {
                    //         if (DialogueLua.GetActorField("Rose", "skill1/5a").asString == "Yes")


                    string skillID = "skill1/" + p2Skill1[cnt].name;

                    bool previousUnlocked = false;
                    if (cnt == 0)
                    {
                        previousUnlocked = true;
                    }
                    else if (cnt > 0)
                    {
                        int tempCNT = cnt - 1;
                        string skillIDprevious = "skill1/" + p2Skill1[tempCNT].name;

                        if (DialogueLua.GetActorField(name2, skillIDprevious).asString == "Yes")
                        {
                            previousUnlocked = true;
                        }
                    }

                    if (previousUnlocked == true)
                    {
                        if (DialogueLua.GetActorField(name2, skillID).asString != "Yes")
                        {
                            int tempPoints = pointsList[1] - 1;
                            pointsList[1] = tempPoints;
                            DialogueLua.SetActorField(name2, skillID, "Yes");
                            int levelSpent = DialogueLua.GetActorField(name2, "levelSpent").asInt + 1;
                            DialogueLua.SetActorField(name2, "levelSpent", levelSpent);

                            p2Skill1[cnt] = (Texture2D)(Resources.Load("SKills/" + skillIDNoP2 + "/" + p2Skill1[cnt].name));
                        }
                    }

                    //              string skillIDbefore = "skill1/ + "
                    //             Debug.Log(skillID);

                }
                else
                {
                    DialogueManager.ShowAlert("Not skill points left");
                    Debug.Log("No points");
                }
            }

        }

        if (GUI.Button(new Rect(p2Skill1Pos.x + (4 * iconWidth), p2Skill1Pos.y - (0.5f * iconWidth), iconWidth, iconWidth), new GUIContent(p2Skill1[4], p2Skill1String[4])))
        {
            if (pointsList[1] > 0)
            {
                string skillID = "skill1/" + p2Skill1[4].name;
                string skillID2 = "skill1/" + p2Skill1[5].name;
                string skillIDprevious = "skill1/" + p2Skill1[3].name;



                if (DialogueLua.GetActorField(name2, skillIDprevious).asString == "Yes")
                {
                    //       Debug.Log(skillID);
                    if (DialogueLua.GetActorField(name2, skillID).asString != "Yes" && DialogueLua.GetActorField(name2, skillID2).asString != "Yes")
                    {
                        int tempPoints = pointsList[1] - 1;
                        pointsList[1] = tempPoints;
                        DialogueLua.SetActorField(name2, skillID, "Yes");
                        int levelSpent = DialogueLua.GetActorField(name2, "levelSpent").asInt + 1;
                        DialogueLua.SetActorField(name2, "levelSpent", levelSpent);
                        p2Skill1[4] = (Texture2D)(Resources.Load("SKills/" + skillIDNoP2 + "/" + p2Skill1[4].name));
                    }
                }
            }
            else
            {
                DialogueManager.ShowAlert("Not skill points left");
                Debug.Log("No points");
            }
        }

        if (GUI.Button(new Rect(p2Skill1Pos.x + (4 * iconWidth), p2Skill1Pos.y + (0.5f * iconWidth), iconWidth, iconWidth), new GUIContent(p2Skill1[5], p2Skill1String[5])))
        {
            if (pointsList[1] > 0)
            {
                string skillID = "skill1/" + p2Skill1[4].name;
                string skillID2 = "skill1/" + p2Skill1[5].name;
      //          Debug.Log(skillID2);
                string skillIDprevious = "skill1/" + p2Skill1[3].name;

                if (DialogueLua.GetActorField(name2, skillIDprevious).asString == "Yes")
                {
                    if (DialogueLua.GetActorField(name2, skillID2).asString != "Yes" && DialogueLua.GetActorField(name2, skillID).asString != "Yes")
                    {
                        int tempPoints = pointsList[1] - 1;
                        pointsList[1] = tempPoints;
                        DialogueLua.SetActorField(name2, skillID2, "Yes");
                        int levelSpent = DialogueLua.GetActorField(name2, "levelSpent").asInt + 1;
                        DialogueLua.SetActorField(name2, "levelSpent", levelSpent);
                        p2Skill1[5] = (Texture2D)(Resources.Load("SKills/" + skillIDNoP2 + "/" + p2Skill1[5].name));
                    }
                }
            }
            else
            {
                DialogueManager.ShowAlert("Not skill points left");
                Debug.Log("No points");
            }
        }

        GUI.Label(player2Path2Rect, player2Path2Name, pregame.myStyle);
        for (int cnt = 0; cnt < p2Skill2.Count - 2; cnt++)
        {
            //         GUI.Button(new Rect(p1Skill1Pos.x + (cnt * iconWidth), p1Skill1Pos.y, iconWidth, iconWidth), p1Skill1[cnt]);
            if (GUI.Button(new Rect(p2Skill2Pos.x + (cnt * iconWidth), p2Skill1Pos.y, iconWidth, iconWidth), new GUIContent(p2Skill2[cnt], p2Skill2String[cnt])))
            {
                if (pointsList[1] > 0)
                {

                    string skillID2 = "skill2/" + p2Skill2[cnt].name;
                    bool previousUnlocked = false;
                    if (cnt == 0)
                    {
                        previousUnlocked = true;
                    }
                    else if (cnt > 0)
                    {
                        int tempCNT = cnt - 1;
                        string skillIDprevious = "skill2/" + p2Skill2[tempCNT].name;

                        if (DialogueLua.GetActorField(name2, skillIDprevious).asString == "Yes")
                        {
                            previousUnlocked = true;
                        }
                    }
                    //           Debug.Log(skillID);
                    if (DialogueLua.GetActorField(name2, skillID2).asString != "Yes" && previousUnlocked == true)
                    {
                        int tempPoints = pointsList[1] - 1;
                        pointsList[1] = tempPoints;
                        DialogueLua.SetActorField(name2, skillID2, "Yes");
                        int levelSpent = DialogueLua.GetActorField(name2, "levelSpent").asInt + 1;
                        DialogueLua.SetActorField(name2, "levelSpent", levelSpent);
                        p2Skill2[cnt] = (Texture2D)(Resources.Load("SKills/" + skillIDno2P2 + "/" + p2Skill2[cnt].name));
                    }
                }
                else
                {
                    DialogueManager.ShowAlert("Not skill points left");
                    Debug.Log("No points");
                }
            }

        }

        if (GUI.Button(new Rect(p2Skill2Pos.x + (4 * iconWidth), p2Skill1Pos.y - (0.5f * iconWidth), iconWidth, iconWidth), new GUIContent(p2Skill2[4], p2Skill2String[4])))
        {
            if (pointsList[1] > 0)
            {
                string skillID = "skill2/" + p2Skill2[4].name;
                string skillID2 = "skill2/" + p2Skill2[5].name;
                //          Debug.Log(skillID);
                string skillIDprevious = "skill2/" + p2Skill1[3].name;

                if (DialogueLua.GetActorField(name2, skillIDprevious).asString == "Yes")
                {
                    if (DialogueLua.GetActorField(name2, skillID).asString != "Yes" && DialogueLua.GetActorField(name2, skillID2).asString != "Yes")
                    {
                        int tempPoints = pointsList[1] - 1;
                        pointsList[1] = tempPoints;
                        DialogueLua.SetActorField(name2, skillID, "Yes");
                        int levelSpent = DialogueLua.GetActorField(name2, "levelSpent").asInt + 1;
                        DialogueLua.SetActorField(name2, "levelSpent", levelSpent);
                        p2Skill2[4] = (Texture2D)(Resources.Load("SKills/" + skillIDno2P2 + "/" + p2Skill2[4].name));
                    }
                }
            }
            else
            {
                DialogueManager.ShowAlert("Not skill points left");
                Debug.Log("No points");
            }
        }

        if (GUI.Button(new Rect(p2Skill2Pos.x + (4 * iconWidth), p2Skill1Pos.y + (0.5f * iconWidth), iconWidth, iconWidth), new GUIContent(p2Skill2[5], p2Skill2String[5])))
        {
            if (pointsList[1] > 0)
            {
                string skillID = "skill2/" + p2Skill2[4].name;
                string skillID2 = "skill2/" + p2Skill2[5].name;
                //          Debug.Log(skillID);
                string skillIDprevious = "skill2/" + p2Skill1[3].name;


                if (DialogueLua.GetActorField(name2, skillIDprevious).asString == "Yes")
                {
                    if (DialogueLua.GetActorField(name2, skillID).asString != "Yes" && DialogueLua.GetActorField(name2, skillID2).asString != "Yes")
                    {
                        int tempPoints = pointsList[1] - 1;
                        pointsList[1] = tempPoints;
                        DialogueLua.SetActorField(name2, skillID2, "Yes");
                        int levelSpent = DialogueLua.GetActorField(name2, "levelSpent").asInt + 1;
                        DialogueLua.SetActorField(name2, "levelSpent", levelSpent);
                        p2Skill2[5] = (Texture2D)(Resources.Load("SKills/" + skillIDno2P2 + "/" + p2Skill2[5].name));
                    }
                }
            }
            else
            {
                DialogueManager.ShowAlert("Not skill points left");
                Debug.Log("No points");
            }
        }

        GUI.Label(new Rect(Screen.width * 0.9f, p2Skill1Pos.y, Screen.width * 0.14f, Screen.height * 0.08f), "Points: " + pointsList[1].ToString(), pregame.myStyle);
    //    Debug.Log(DialogueLua.GetActorField(name2, "levelSpent").asInt);

        GUI.Label(toolTip2, GUI.tooltip);
        GUI.tooltip = null;
    }

    private void ShowPlayer3()
    {

        GUI.Label(name3Rect, name3, pregame.myStyle);
        GUI.Label(player3Path1Rect, player3Path1Name, pregame.myStyle);
        GUI.Label(level3Rect, "LEVEL: " + levelList[2].ToString() + "  (" + expList[2] + "/" + nextExpList[2] + ")", pregame.myStyle);
        GUI.DrawTexture(pic3Rect, pic3);

        if (GUI.Button(readyButtonRect3, "READY", styleButton))
        {
     //      ready3 = !ready3;
            readyList[2] = !readyList[2];
        }
        if (readyList[2] == false)
        {
            GUI.DrawTexture(box3, buttonNoChecked);
        }
        else
        {
            GUI.DrawTexture(box3, buttonChecked);
        }


        for (int cnt = 0; cnt < p3Skill1.Count - 2; cnt++)
        {
            //         GUI.Button(new Rect(p1Skill1Pos.x + (cnt * iconWidth), p1Skill1Pos.y, iconWidth, iconWidth), p1Skill1[cnt]);
            if (GUI.Button(new Rect(p3Skill1Pos.x + (cnt * iconWidth), p3Skill1Pos.y, iconWidth, iconWidth), new GUIContent(p3Skill1[cnt], p3Skill1String[cnt])))
            {
                if (pointsList[2] > 0)
                {
                    //         if (DialogueLua.GetActorField("Rose", "skill1/5a").asString == "Yes")


                    string skillID = "skill1/" + p3Skill1[cnt].name;

                    bool previousUnlocked = false;
                    if (cnt == 0)
                    {
                        previousUnlocked = true;
                    }
                    else if (cnt > 0)
                    {
                        int tempCNT = cnt - 1;
                        string skillIDprevious = "skill1/" + p3Skill1[tempCNT].name;

                        if (DialogueLua.GetActorField(name3, skillIDprevious).asString == "Yes")
                        {
                            previousUnlocked = true;
                        }
                    }

                    if (previousUnlocked == true)
                    {
                        if (DialogueLua.GetActorField(name3, skillID).asString != "Yes")
                        {
                            int tempPoints = pointsList[2] - 1;
                            pointsList[2] = tempPoints;
                            DialogueLua.SetActorField(name3, skillID, "Yes");
                            int levelSpent = DialogueLua.GetActorField(name3, "levelSpent").asInt + 1;
                            DialogueLua.SetActorField(name3, "levelSpent", levelSpent);

                            p3Skill1[cnt] = (Texture2D)(Resources.Load("SKills/" + skillIDNoP3 + "/" + p3Skill1[cnt].name));
                        }
                    }

                    //              string skillIDbefore = "skill1/ + "
                    //             Debug.Log(skillID);

                }
                else
                {
                    DialogueManager.ShowAlert("Not skill points left");
                    Debug.Log("No points");
                }
            }

        }

        if (GUI.Button(new Rect(p3Skill1Pos.x + (4 * iconWidth), p3Skill1Pos.y - (0.5f * iconWidth), iconWidth, iconWidth), new GUIContent(p3Skill1[4], p3Skill1String[4])))
        {
            if (pointsList[2] > 0)
            {
                string skillID = "skill1/" + p3Skill1[4].name;
                string skillID2 = "skill1/" + p3Skill1[5].name;
                string skillIDprevious = "skill1/" + p3Skill1[3].name;



                if (DialogueLua.GetActorField(name3, skillIDprevious).asString == "Yes")
                {
                    //       Debug.Log(skillID);
                    if (DialogueLua.GetActorField(name3, skillID).asString != "Yes" && DialogueLua.GetActorField(name3, skillID2).asString != "Yes")
                    {
                        int tempPoints = pointsList[2] - 1;
                        pointsList[2] = tempPoints;
                        DialogueLua.SetActorField(name3, skillID, "Yes");
                        int levelSpent = DialogueLua.GetActorField(name3, "levelSpent").asInt + 1;
                        DialogueLua.SetActorField(name3, "levelSpent", levelSpent);
                        p3Skill1[4] = (Texture2D)(Resources.Load("SKills/" + skillIDNoP3 + "/" + p3Skill1[4].name));
                    }
                }
            }
            else
            {
                DialogueManager.ShowAlert("Not skill points left");
                Debug.Log("No points");
            }
        }

        if (GUI.Button(new Rect(p3Skill1Pos.x + (4 * iconWidth), p3Skill1Pos.y + (0.5f * iconWidth), iconWidth, iconWidth), new GUIContent(p3Skill1[5], p3Skill1String[5])))
        {
            if (pointsList[2] > 0)
            {
                string skillID = "skill1/" + p3Skill1[4].name;
                string skillID2 = "skill1/" + p3Skill1[5].name;
      //          Debug.Log(skillID2);
                string skillIDprevious = "skill1/" + p3Skill1[3].name;

                if (DialogueLua.GetActorField(name3, skillIDprevious).asString == "Yes")
                {
                    if (DialogueLua.GetActorField(name3, skillID2).asString != "Yes" && DialogueLua.GetActorField(name3, skillID).asString != "Yes")
                    {
                        int tempPoints = pointsList[2] - 1;
                        pointsList[2] = tempPoints;
                        DialogueLua.SetActorField(name3, skillID2, "Yes");
                        int levelSpent = DialogueLua.GetActorField(name3, "levelSpent").asInt + 1;
                        DialogueLua.SetActorField(name3, "levelSpent", levelSpent);
                        p3Skill1[5] = (Texture2D)(Resources.Load("SKills/" + skillIDNoP3 + "/" + p3Skill1[5].name));
                    }
                }
            }
            else
            {
                DialogueManager.ShowAlert("Not skill points left");
                Debug.Log("No points");
            }
        }

        GUI.Label(player3Path2Rect, player3Path2Name, pregame.myStyle);
        for (int cnt = 0; cnt < p3Skill2.Count - 2; cnt++)
        {
            //         GUI.Button(new Rect(p1Skill1Pos.x + (cnt * iconWidth), p1Skill1Pos.y, iconWidth, iconWidth), p1Skill1[cnt]);
            if (GUI.Button(new Rect(p3Skill2Pos.x + (cnt * iconWidth), p3Skill1Pos.y, iconWidth, iconWidth), new GUIContent(p3Skill2[cnt], p3Skill2String[cnt])))
            {
                if (pointsList[2] > 0)
                {

                    string skillID2 = "skill2/" + p3Skill2[cnt].name;
                    bool previousUnlocked = false;
                    if (cnt == 0)
                    {
                        previousUnlocked = true;
                    }
                    else if (cnt > 0)
                    {
                        int tempCNT = cnt - 1;
                        string skillIDprevious = "skill2/" + p3Skill2[tempCNT].name;

                        if (DialogueLua.GetActorField(name3, skillIDprevious).asString == "Yes")
                        {
                            previousUnlocked = true;
                        }
                    }
                    //           Debug.Log(skillID);
                    if (DialogueLua.GetActorField(name3, skillID2).asString != "Yes" && previousUnlocked == true)
                    {
                        int tempPoints = pointsList[2] - 1;
                        pointsList[2] = tempPoints;
                        DialogueLua.SetActorField(name3, skillID2, "Yes");
                        int levelSpent = DialogueLua.GetActorField(name3, "levelSpent").asInt + 1;
                        DialogueLua.SetActorField(name3, "levelSpent", levelSpent);
                        p3Skill2[cnt] = (Texture2D)(Resources.Load("SKills/" + skillIDno2P3 + "/" + p3Skill2[cnt].name));
                    }
                }
                else
                {
                    DialogueManager.ShowAlert("Not skill points left");
                    Debug.Log("No points");
                }
            }

        }

        if (GUI.Button(new Rect(p3Skill2Pos.x + (4 * iconWidth), p3Skill1Pos.y - (0.5f * iconWidth), iconWidth, iconWidth), new GUIContent(p3Skill2[4], p3Skill2String[4])))
        {
            if (pointsList[2] > 0)
            {
                string skillID = "skill2/" + p3Skill2[4].name;
                string skillID2 = "skill2/" + p3Skill2[5].name;
                //          Debug.Log(skillID);
                string skillIDprevious = "skill2/" + p3Skill1[3].name;

                if (DialogueLua.GetActorField(name3, skillIDprevious).asString == "Yes")
                {
                    if (DialogueLua.GetActorField(name3, skillID).asString != "Yes" && DialogueLua.GetActorField(name3, skillID2).asString != "Yes")
                    {
                        int tempPoints = pointsList[2] - 1;
                        pointsList[2] = tempPoints;
                        DialogueLua.SetActorField(name3, skillID, "Yes");
                        int levelSpent = DialogueLua.GetActorField(name3, "levelSpent").asInt + 1;
                        DialogueLua.SetActorField(name3, "levelSpent", levelSpent);
                        p3Skill2[4] = (Texture2D)(Resources.Load("SKills/" + skillIDno2P3 + "/" + p3Skill2[4].name));
                    }
                }
            }
            else
            {
                DialogueManager.ShowAlert("Not skill points left");
                Debug.Log("No points");
            }
        }

        if (GUI.Button(new Rect(p3Skill2Pos.x + (4 * iconWidth), p3Skill1Pos.y + (0.5f * iconWidth), iconWidth, iconWidth), new GUIContent(p3Skill2[5], p3Skill2String[5])))
        {
            if (pointsList[2] > 0)
            {
                string skillID = "skill2/" + p3Skill2[4].name;
                string skillID2 = "skill2/" + p3Skill2[5].name;
                //          Debug.Log(skillID);
                string skillIDprevious = "skill2/" + p3Skill1[3].name;
                if (DialogueLua.GetActorField(name3, skillIDprevious).asString == "Yes")
                {
                    if (DialogueLua.GetActorField(name3, skillID).asString != "Yes" && DialogueLua.GetActorField(name3, skillID2).asString != "Yes")
                    {
                        int tempPoints = pointsList[2] - 1;
                        pointsList[2] = tempPoints;
                        DialogueLua.SetActorField(name3, skillID2, "Yes");
                        int levelSpent = DialogueLua.GetActorField(name3, "levelSpent").asInt + 1;
                        DialogueLua.SetActorField(name3, "levelSpent", levelSpent);
                        p2Skill2[5] = (Texture2D)(Resources.Load("SKills/" + skillIDno2P3 + "/" + p2Skill2[5].name));
                    }
                }

            }
            else
            {
                DialogueManager.ShowAlert("Not skill points left");
                Debug.Log("No points");
            }
        }

        GUI.Label(new Rect(Screen.width * 0.9f, p3Skill1Pos.y, Screen.width * 0.14f, Screen.height * 0.08f), "Points: " + pointsList[2].ToString(), pregame.myStyle);
        //    Debug.Log(DialogueLua.GetActorField(name2, "levelSpent").asInt);

        GUI.Label(toolTip3, GUI.tooltip);
        GUI.tooltip = null;
    }

    private void ShowPlayer4()
    {

        GUI.Label(name4Rect, name4, pregame.myStyle);
        GUI.Label(player4Path1Rect, player4Path1Name, pregame.myStyle);
        GUI.Label(level4Rect, "LEVEL: " + levelList[3].ToString() + "  (" + expList[3] + "/" + nextExpList[3] + ")", pregame.myStyle);
        GUI.DrawTexture(pic1Rect, pic4);

        if (GUI.Button(readyButtonRect4, "READY", styleButton))
        {
      //      ready4 = !ready4;
            readyList[3] = !readyList[3];
        }
        if (readyList[3] == false)
        {
            GUI.DrawTexture(box4, buttonNoChecked);
        }
        else
        {
            GUI.DrawTexture(box4, buttonChecked);
        }


        for (int cnt = 0; cnt < p4Skill1.Count - 2; cnt++)
        {
            if (GUI.Button(new Rect(p3Skill1Pos.x + (cnt * iconWidth), p4Skill1Pos.y, iconWidth, iconWidth), new GUIContent(p4Skill1[cnt], p4Skill1String[cnt])))
            {
                if (pointsList[3] > 0)
                {
                    //         if (DialogueLua.GetActorField("Rose", "skill1/5a").asString == "Yes")


                    string skillID = "skill1/" + p4Skill1[cnt].name;

                    bool previousUnlocked = false;
                    if (cnt == 0)
                    {
                        previousUnlocked = true;
                    }
                    else if (cnt > 0)
                    {
                        int tempCNT = cnt - 1;
                        string skillIDprevious = "skill1/" + p4Skill1[tempCNT].name;

                        if (DialogueLua.GetActorField(name4, skillIDprevious).asString == "Yes")
                        {
                            previousUnlocked = true;
                        }
                    }

                    if (previousUnlocked == true)
                    {
                        if (DialogueLua.GetActorField(name4, skillID).asString != "Yes")
                        {
                            int tempPoints = pointsList[3] - 1;
                            pointsList[3] = tempPoints;
                            DialogueLua.SetActorField(name4, skillID, "Yes");
                            int levelSpent = DialogueLua.GetActorField(name4, "levelSpent").asInt + 1;
                            DialogueLua.SetActorField(name4, "levelSpent", levelSpent);

                            p4Skill1[cnt] = (Texture2D)(Resources.Load("SKills/" + skillIDNoP4 + "/" + p4Skill1[cnt].name));
                        }
                    }

                    //              string skillIDbefore = "skill1/ + "
                    //             Debug.Log(skillID);

                }
                else
                {
                    DialogueManager.ShowAlert("Not skill points left");
                    Debug.Log("No points");
                }
            }

        }

        if (GUI.Button(new Rect(p4Skill1Pos.x + (4 * iconWidth), p4Skill1Pos.y - (0.5f * iconWidth), iconWidth, iconWidth), new GUIContent(p4Skill1[4], p4Skill1String[4])))
        {
            if (pointsList[3] > 0)
            {
                string skillID = "skill1/" + p4Skill1[4].name;
                string skillID2 = "skill1/" + p4Skill1[5].name;
                string skillIDprevious = "skill1/" + p4Skill1[3].name;

                if (DialogueLua.GetActorField(name4, skillIDprevious).asString == "Yes")
                {
                    //       Debug.Log(skillID);
                    if (DialogueLua.GetActorField(name4, skillID).asString != "Yes" && DialogueLua.GetActorField(name4, skillID2).asString != "Yes")
                    {
                        int tempPoints = pointsList[3] - 1;
                        pointsList[3] = tempPoints;
                        DialogueLua.SetActorField(name4, skillID, "Yes");
                        int levelSpent = DialogueLua.GetActorField(name4, "levelSpent").asInt + 1;
                        DialogueLua.SetActorField(name4, "levelSpent", levelSpent);
                        p4Skill1[4] = (Texture2D)(Resources.Load("SKills/" + skillIDNoP4 + "/" + p4Skill1[4].name));
                    }
                }
            }
            else
            {
                DialogueManager.ShowAlert("Not skill points left");
                Debug.Log("No points");
            }
        }

        if (GUI.Button(new Rect(p4Skill1Pos.x + (4 * iconWidth), p4Skill1Pos.y + (0.5f * iconWidth), iconWidth, iconWidth), new GUIContent(p4Skill1[5], p4Skill1String[5])))
        {
            if (pointsList[3] > 0)
            {
                string skillID = "skill1/" + p4Skill1[4].name;
                string skillID2 = "skill1/" + p4Skill1[5].name;
                Debug.Log(skillID2);
                string skillIDprevious = "skill1/" + p4Skill1[3].name;

                if (DialogueLua.GetActorField(name4, skillIDprevious).asString == "Yes")
                {
                    if (DialogueLua.GetActorField(name4, skillID2).asString != "Yes" && DialogueLua.GetActorField(name4, skillID).asString != "Yes")
                    {
                        int tempPoints = pointsList[3] - 1;
                        pointsList[3] = tempPoints;
                        DialogueLua.SetActorField(name4, skillID2, "Yes");
                        int levelSpent = DialogueLua.GetActorField(name4, "levelSpent").asInt + 1;
                        DialogueLua.SetActorField(name4, "levelSpent", levelSpent);
                        p4Skill1[5] = (Texture2D)(Resources.Load("SKills/" + skillIDNoP4 + "/" + p4Skill1[5].name));
                    }
                }
            }
            else
            {
                DialogueManager.ShowAlert("Not skill points left");
                Debug.Log("No points");
            }
        }

        GUI.Label(player4Path2Rect, player4Path2Name, pregame.myStyle);
        for (int cnt = 0; cnt < p4Skill2.Count - 2; cnt++)
        {
            //         GUI.Button(new Rect(p1Skill1Pos.x + (cnt * iconWidth), p1Skill1Pos.y, iconWidth, iconWidth), p1Skill1[cnt]);
            if (GUI.Button(new Rect(p4Skill2Pos.x + (cnt * iconWidth), p4Skill1Pos.y, iconWidth, iconWidth), new GUIContent(p4Skill2[cnt], p4Skill2String[cnt])))
            {
                if (pointsList[3] > 0)
                {

                    string skillID2 = "skill2/" + p4Skill2[cnt].name;
                    bool previousUnlocked = false;
                    if (cnt == 0)
                    {
                        previousUnlocked = true;
                    }
                    else if (cnt > 0)
                    {
                        int tempCNT = cnt - 1;
                        string skillIDprevious = "skill2/" + p4Skill2[tempCNT].name;

                        if (DialogueLua.GetActorField(name4, skillIDprevious).asString == "Yes")
                        {
                            previousUnlocked = true;
                        }
                    }
                    //           Debug.Log(skillID);
                    if (DialogueLua.GetActorField(name4, skillID2).asString != "Yes" && previousUnlocked == true)
                    {
                        int tempPoints = pointsList[3] - 1;
                        pointsList[3] = tempPoints;
                        DialogueLua.SetActorField(name4, skillID2, "Yes");
                        int levelSpent = DialogueLua.GetActorField(name4, "levelSpent").asInt + 1;
                        DialogueLua.SetActorField(name4, "levelSpent", levelSpent);
                        p3Skill2[cnt] = (Texture2D)(Resources.Load("SKills/" + skillIDno2P4 + "/" + p3Skill2[cnt].name));
                    }
                }
                else
                {
                    DialogueManager.ShowAlert("Not skill points left");
                    Debug.Log("No points");
                }
            }

        }

        if (GUI.Button(new Rect(p4Skill2Pos.x + (4 * iconWidth), p4Skill1Pos.y - (0.5f * iconWidth), iconWidth, iconWidth), new GUIContent(p4Skill2[4], p4Skill2String[4])))
        {
            if (pointsList[3] > 0)
            {
                string skillID = "skill2/" + p4Skill2[4].name;
                string skillID2 = "skill2/" + p4Skill2[5].name;
                //          Debug.Log(skillID);
                string skillIDprevious = "skill2/" + p4Skill1[3].name;

                if (DialogueLua.GetActorField(name4, skillIDprevious).asString == "Yes")
                {
                    if (DialogueLua.GetActorField(name4, skillID).asString != "Yes" && DialogueLua.GetActorField(name4, skillID2).asString != "Yes")
                    {
                        int tempPoints = pointsList[3] - 1;
                        pointsList[3] = tempPoints;
                        DialogueLua.SetActorField(name4, skillID, "Yes");
                        int levelSpent = DialogueLua.GetActorField(name4, "levelSpent").asInt + 1;
                        DialogueLua.SetActorField(name4, "levelSpent", levelSpent);
                        p4Skill2[4] = (Texture2D)(Resources.Load("SKills/" + skillIDno2P4 + "/" + p3Skill2[4].name));
                    }
                }
            }
            else
            {
                DialogueManager.ShowAlert("Not skill points left");
                Debug.Log("No points");
            }
        }

        if (GUI.Button(new Rect(p4Skill2Pos.x + (4 * iconWidth), p4Skill1Pos.y + (0.5f * iconWidth), iconWidth, iconWidth), new GUIContent(p4Skill2[5], p4Skill2String[5])))
        {
            if (pointsList[3] > 0)
            {
                string skillID = "skill2/" + p4Skill2[4].name;
                string skillID2 = "skill2/" + p4Skill2[5].name;
                //          Debug.Log(skillID);
                string skillIDprevious = "skill2/" + p4Skill1[3].name;
                if (DialogueLua.GetActorField(name4, skillIDprevious).asString == "Yes")
                {
                    if (DialogueLua.GetActorField(name4, skillID).asString != "Yes" && DialogueLua.GetActorField(name4, skillID2).asString != "Yes")
                    {
                        int tempPoints = pointsList[3] - 1;
                        pointsList[3] = tempPoints;
                        DialogueLua.SetActorField(name4, skillID2, "Yes");
                        int levelSpent = DialogueLua.GetActorField(name4, "levelSpent").asInt + 1;
                        DialogueLua.SetActorField(name4, "levelSpent", levelSpent);
                        p3Skill2[5] = (Texture2D)(Resources.Load("SKills/" + skillIDno2P4 + "/" + p3Skill2[5].name));
                    }
                }

            }
            else
            {
                DialogueManager.ShowAlert("Not skill points left");
                Debug.Log("No points");
            }
        }

        GUI.Label(new Rect(Screen.width * 0.9f, p4Skill1Pos.y, Screen.width * 0.14f, Screen.height * 0.08f), "Points: " + pointsList[3].ToString(), pregame.myStyle);
        //    Debug.Log(DialogueLua.GetActorField(name2, "levelSpent").asInt);

        GUI.Label(toolTip4, GUI.tooltip);
        GUI.tooltip = null;
    }

}
