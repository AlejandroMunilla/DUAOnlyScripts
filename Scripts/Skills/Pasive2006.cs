using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class Pasive2006 : MonoBehaviour
{
    
    public GameObject caster;
    private bool loaded = false;
    private string idSkill = "2006";
    private string focus = "5a";
    private int level;
    private float coolDownTime = 20;
    private PlayerStats ps;
    private PlayerAttack pa;
    private ThirdPersonUserControl tpu;
    private GameController gc;
    private myGUI mygui;
   
    void OnEnable()
    {
        if (loaded == false)
        {
            loaded = true;
            //        Debug.Log(transform.root.gameObject.name);
            caster = transform.root.gameObject;
            level = DialogueLua.GetActorField(caster.name, "levelArena").asInt;
            GameObject gcon = GameObject.FindGameObjectWithTag("GameController");
            gc = gcon.GetComponent<GameController>();
            mygui = gcon.GetComponent<myGUI>();
            ps = caster.GetComponent<PlayerStats>();
            pa = caster.GetComponent<PlayerAttack>();
            tpu = caster.GetComponent<ThirdPersonUserControl>();
            //          Debug.Log(caster);

            ApplyEffect();
        }
    }


    private void ApplyEffect()
    {
        int health = 0;
        string skillID = "skill1/";
        //    Debug.Log(DialogueLua.GetActorField(caster.name, skillID + "1").asString);
        if (DialogueLua.GetActorField(caster.name, skillID + "1").asString == "Yes")
        {
            health = (int)(ps.totalHealth / 0.95f);
        }

        if (DialogueLua.GetActorField(caster.name, skillID + "2").asString == "Yes")
        {
            health = (int)(ps.totalHealth / 0.95f);
        }

        if (DialogueLua.GetActorField(caster.name, skillID + "3").asString == "Yes")
        {
            

        }

        if (DialogueLua.GetActorField(caster.name, skillID + "4").asString == "Yes")
        {

            health = (int)(ps.totalHealth / 0.85f);
        }

        if (DialogueLua.GetActorField(caster.name, skillID + "5a").asString == "Yes")
        {
            health = (int)(ps.totalHealth / 0.8f);
        }
        if (DialogueLua.GetActorField(caster.name, skillID + "5b").asString == "Yes")
        {
            health = (int)(ps.totalHealth / 0.8f);
        }

        if (health > 0)
        {
            ps.totalHealth = health;
            ps.health = health;
        }
    }
}
