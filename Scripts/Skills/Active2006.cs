using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class Active2006 : MonoBehaviour
{
    public int armour = 0;
    public GameObject caster;

    private bool loaded = false;
    private bool healing = false;
    private bool stopRegeneration = false;
    private int level;
    private int damage = 1;
    private int iceRes = 0;
    private int fireRes = 0;
    private int entropyRes = 0;
    private int magicRes = 0;
    private int poisonRes = 0;
    private int mindRes = 0;
    private int necroRes = 0;
    private float coolDownTime = 20;
    private float range = 10;
    private float timer = 10;
    private string idSkill = "2006";
    private string skillstring = "skill1";
    private GameObject aura;
    private List<GameObject> players = new List<GameObject>();
    private PlayerStats ps;
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
            PlayerAttack pa = caster.GetComponent<PlayerAttack>();
            tpu = caster.GetComponent<ThirdPersonUserControl>();
            aura = transform.Find("Aura").gameObject;
            aura.transform.parent = null;
            aura.transform.rotation = Quaternion.Euler(0, 0, 0);
            aura.transform.parent = caster.transform;

            Invoke("CheckAura", 1);
            
            if (ps.skill1 == idSkill)
            {
                tpu.coolDownTime1 = coolDownTime;
                mygui.skill1Cool[ps.internalCNT] = coolDownTime;
                skillstring = "skill1";
            }
            else
            {
                tpu.coolDownTime2 = coolDownTime;
                Debug.Log(mygui.skill2Cool.Count);
                mygui.skill2Cool[ps.internalCNT] = coolDownTime;
                skillstring = "skill2";
                //          mygui.skill2Tex[ps.internalCNT] = mygui.skill2TexActive[ps.internalCNT];
            }

        }
        players.Clear();
        aura.SetActive(true);
        bool skillAvailable = false;

        if (DialogueLua.GetActorField(caster.name, skillstring + "/5a").asString == "Yes")
        {
            skillAvailable = true;
            damage = 4;
            range = 15;
            timer = 19;
            range = 15;

            necroRes = 1;
            fireRes = 1;
            iceRes = 1;
            poisonRes = 1;
            mindRes = 1;
            magicRes = 1;
            entropyRes = 1;
            stopRegeneration = true;
        }
        else if (DialogueLua.GetActorField(caster.name, skillstring + "/5b").asString == "Yes")
        {
            skillAvailable = true;
            armour = 1;
            healing = true;

            range = 15;
            timer = 19;
            necroRes = 3;
            fireRes = 3;
            iceRes = 3;
            poisonRes = 3;
            mindRes = 3;
            magicRes = 3;
            entropyRes = 3;

            range = 15;
            damage = 2;
            stopRegeneration = true;
        }
        else if (DialogueLua.GetActorField(caster.name, skillstring + "/4").asString == "Yes")
        {
            range = 15;
            damage = 2;

            necroRes = 1;
            fireRes = 1;
            iceRes = 1;
            poisonRes = 1;
            mindRes = 1;
            magicRes = 1;
            entropyRes = 1;
            skillAvailable = true;
            stopRegeneration = true;
        }
        else if (DialogueLua.GetActorField(caster.name, skillstring + "/3").asString == "Yes")
        {
            damage = 2;
            skillAvailable = true;
            necroRes = 1;
            fireRes = 1;
            iceRes = 1;
            poisonRes = 1;
            mindRes = 1;
            magicRes = 1;
            entropyRes = 1;
            stopRegeneration = true;
        }


        if (skillAvailable == true)
        {
            caster.GetComponent<Animator>().SetTrigger("HeavyAttack");
            //     Invoke("CallAudio", 1);
            Invoke("CheckMembersInside", 0.01f);
        }
        else
        {
            if (ps.skill1 == idSkill)
            {

                tpu.coolDownTime1 = coolDownTime;
                mygui.skill1Cool[ps.internalCNT] = 0;

            }
            else
            {
                tpu.coolDownTime2 = coolDownTime;
                mygui.skill2Cool[ps.internalCNT] = 0;
                //          mygui.skill2Tex[ps.internalCNT] = mygui.skill2TexActive[ps.internalCNT];
            }



        }

        if (stopRegeneration == true)
        {
            InvokeRepeating("StopRegeneration", 1, 3);
        }
    }

    private void CheckMembersInside ()
    {

        foreach (GameObject go in gc.players)
        {
            float distance = Vector3.Distance(caster.transform.position, go.transform.position);
            if (distance <= range)
            {
                players.Add(go);
                PlayerStats ps = go.GetComponent<PlayerStats>();
                ps.addDam = ps.addDam + damage;
                ps.armor = ps.armor + armour;

                ps.necroRes = ps.necroRes + necroRes;
                ps.fireRes = ps.fireRes + fireRes;
                ps.iceRes = ps.iceRes + iceRes;
                ps.poisonRes = ps.poisonRes + poisonRes;
                ps.mindRes = ps.mindRes + mindRes;
                ps.magidRes = ps.magidRes + magicRes;
                ps.entropyRes = ps.entropyRes + entropyRes;

                if (healing == true)
                {
                    int healDamage = Random.Range(2, 12) + 2;
                    ps.AddjustHealth(healDamage, caster, false);

                }
            }
        }

        Invoke("EndEffect", timer);
        gameObject.SetActive(false);
    }

    private void EndEffect ()
    {
        foreach (GameObject go in gc.players)
        {
            float distance = Vector3.Distance(caster.transform.position, go.transform.position);
            if (distance <= range)
            {
                players.Add(go);
                PlayerStats ps = go.GetComponent<PlayerStats>();
                ps.addDam = ps.addDam - damage;
                ps.armor = ps.armor - armour;
                ps.necroRes = ps.necroRes - necroRes;
                ps.fireRes = ps.fireRes - fireRes;
                ps.iceRes = ps.iceRes - iceRes;
                ps.poisonRes = ps.poisonRes - poisonRes;
                ps.mindRes = ps.mindRes - mindRes;
                ps.magidRes = ps.magidRes - magicRes;
                ps.entropyRes = ps.entropyRes - entropyRes;

            }
        }

        aura.SetActive(false);
    }

    private void CheckAura ()
    {

        aura.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void StopRegeneration ()
    {
        foreach (GameObject go in gc.enemies)
        {
            if (go.GetComponent<PlayerStats>().currentRegen >0)
            {
                ReduceRegeneration(go);
            }
        }
        foreach (GameObject go in gc.allies)
        {
            if (go.GetComponent<PlayerStats>().currentRegen > 0)
            {
                ReduceRegeneration(go);
            }
        }
    }

    private void ReduceRegeneration (GameObject go)
    {
        PlayerStats ps = go.GetComponent<PlayerStats>();
        ps.AddjustRegen(-1, caster, false);
    }
}

