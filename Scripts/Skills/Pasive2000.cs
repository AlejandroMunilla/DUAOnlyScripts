using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;


/// <summary>
/// Warrior Skill Script
/// </summary>
public class Pasive2000 : MonoBehaviour
{
    private bool loaded = false;
    private GameObject caster;
    private int level;

    void OnEnable ()
    {
        if (loaded == false)
        {
            loaded = true;
    //        Debug.Log(transform.root.gameObject.name);
            caster = transform.root.gameObject;
            level = DialogueLua.GetActorField(caster.name, "levelArena").asInt;
            ApplyEffect();
        }
    }

    private void ApplyEffect ()
    {
        PlayerStats ps = caster.GetComponent<PlayerStats>();
        PlayerAttack pa = caster.GetComponent<PlayerAttack>();
        int mana = 0;
        int health = 0;
        string skillID = "skill1/";
    //    Debug.Log(DialogueLua.GetActorField(caster.name, skillID + "1").asString);
        if (DialogueLua.GetActorField(caster.name, skillID + "1").asString == "Yes")
        {
            mana = (int)(ps.totalMana / 0.9f);   

        }
        
        if (DialogueLua.GetActorField(caster.name, skillID + "2").asString == "Yes")
        {
            mana = (int)(ps.mana / 0.9f);
            health = (int)(ps.health / 0.95f);
        }
        
        if (DialogueLua.GetActorField(caster.name, skillID + "3").asString == "Yes")
        {
            
            ps.addDam = ps.addDam + 1;
        }
        
        if (DialogueLua.GetActorField(caster.name, skillID + "4").asString == "Yes")
        {
            mana = (int)(ps.mana / 0.9f);
            health = (int)(ps.health / 0.95f);
        }
        
        if (DialogueLua.GetActorField(caster.name, skillID + "5a").asString == "Yes")
        {

        }
        if (DialogueLua.GetActorField(caster.name, skillID + "5b").asString == "Yes")
        {
            ps.addDam = ps.addDam + 2;
        }

        if (health > 0)
        {
            ps.totalHealth = health;
            ps.health = health;
        }

        if (mana > 0)
        {
            ps.mana = mana;
            ps.totalMana = mana;
        }

    //    Debug.Log("Done");
        
    }

}
