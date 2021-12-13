using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;


/// <summary>
/// Warrior Skill Script
/// </summary>
public class Pasive2002 : MonoBehaviour
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

        
        if (DialogueLua.GetActorField(caster.name, skillID + "5a").asString == "Yes")
        {
            ps.backStabMod = ps.backStabMod + 1.0f;
            ps.headShotMultiplayer = ps.headShotMultiplayer + 0.25f;
            health = (int)(ps.health / 0.85f);
            ps.addDam = ps.addDam + 2;
            ps.secondAddDam = ps.secondAddDam + 2;
        }
        else if (DialogueLua.GetActorField(caster.name, skillID + "5b").asString == "Yes")
        {
            ps.backStabMod = ps.backStabMod + 0.5f;
            health = (int)(ps.health / 0.85f);
        }
        else if (DialogueLua.GetActorField(caster.name, skillID + "4").asString == "Yes")
        {
            ps.backStabMod = ps.backStabMod + 0.5f;
            health = (int)(ps.health / 0.85f);
        }
        else if (DialogueLua.GetActorField(caster.name, skillID + "3").asString == "Yes")
        {
            health = (int)(ps.health / 0.85f);
        }
        else if (DialogueLua.GetActorField(caster.name, skillID + "2").asString == "Yes")
        {
            health = (int)(ps.health / 0.90f);
        }
        else if (DialogueLua.GetActorField(caster.name, skillID + "1").asString == "Yes")
        {
            health = (int)(ps.totalMana / 0.95f);
            ps.backStabMod = ps.backStabMod + 0.25f;
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

    }

}
