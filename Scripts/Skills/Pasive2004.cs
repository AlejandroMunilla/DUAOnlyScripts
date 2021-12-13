using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;


/// <summary>
/// Warrior Skill Script
/// </summary>
public class Pasive2004 : MonoBehaviour
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
        ThirdPersonUserControl tpu = caster.GetComponent<ThirdPersonUserControl>();
        int mana = 0;
        int health = 0;
        string skillID = "skill1/";
    //    Debug.Log(DialogueLua.GetActorField(caster.name, skillID + "1").asString);

        
        if (DialogueLua.GetActorField(caster.name, skillID + "5a").asString == "Yes")
        {
            health = (int)(ps.health / 0.85f);

        }
        else if (DialogueLua.GetActorField(caster.name, skillID + "5b").asString == "Yes")
        {
            health = (int)(ps.health / 0.85f);

        }
        else if (DialogueLua.GetActorField(caster.name, skillID + "4").asString == "Yes")
        {
            health = (int)(ps.health / 0.85f);
            
        }
        else if (DialogueLua.GetActorField(caster.name, skillID + "3").asString == "Yes")
        {
            health = (int)(ps.health / 0.90f);
            ps.headShotMultiplayer = ps.headShotMultiplayer + 0.25f;
        }
        else if (DialogueLua.GetActorField(caster.name, skillID + "2").asString == "Yes")
        {
            health = (int)(ps.health / 0.90f);

        }
        else if (DialogueLua.GetActorField(caster.name, skillID + "1").asString == "Yes")
        {
            health = (int)(ps.totalMana / 0.95f);
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
     //   Debug.Log(health);
        
    }

}
