using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;


/// <summary>
/// Warrior Skill Script
/// </summary>
public class Active2005 : MonoBehaviour
{
    private bool loaded = false;
    private bool adrenaline = false;
    private int coolDownTime = 20;
    private GameObject caster;
    private Transform sphere;
    private int level;
    private float size;
    private float finalSize = 16.0f;
    private string idSkill = "2005";
    private string skillstring = "skill1";
    private float rate = 0;
    private GameController gc;
    private myGUI mygui;
    private SphereCollider col;
    private MeshRenderer rend;
    private PlayerStats ps;
    private ThirdPersonUserControl tpu;
    private int maxDamage;
    private bool attackDone = false;
   
    

    void OnEnable ()
    {
        


        if (loaded == false)
        {
            loaded = true;
    //        Debug.Log(transform.root.gameObject.name);
            caster = transform.root.gameObject;            
            level = DialogueLua.GetActorField(caster.name, "levelArena").asInt;
            sphere = transform.Find("Sphere");
            rend = sphere.GetComponent<MeshRenderer>();
            rend.enabled = false;
            GameObject gcon = GameObject.FindGameObjectWithTag("GameController");
            gc = gcon.GetComponent<GameController>();
            mygui = gcon.GetComponent<myGUI>();
            ps = caster.GetComponent<PlayerStats>();
            PlayerAttack pa = caster.GetComponent<PlayerAttack>();
            tpu = caster.GetComponent<ThirdPersonUserControl>();
            //          Debug.Log(caster);

            if (ps.skill1 == idSkill)
            {
                
                tpu.coolDownTime1 = coolDownTime;
                mygui.skill1Cool[ps.internalCNT] = coolDownTime;
                skillstring = "skill1";

            }
            else
            {
                tpu.coolDownTime2 = coolDownTime;
                mygui.skill2Cool[ps.internalCNT] = coolDownTime;
                skillstring = "skill2";
                //          mygui.skill2Tex[ps.internalCNT] = mygui.skill2TexActive[ps.internalCNT];
            }

        }

        bool skillAvailable = false;

        if (DialogueLua.GetActorField(caster.name, skillstring + "/5a").asString == "Yes")
        {
            skillAvailable = true;
        }
        else if (DialogueLua.GetActorField(caster.name, skillstring  + "/5b").asString == "Yes")
        {
            skillAvailable = true;
            finalSize = 20;
            adrenaline = true;
        }
        else if (DialogueLua.GetActorField(caster.name, skillstring + "/3").asString == "Yes")
        {
            skillAvailable = true;
            
        }


        if (skillAvailable == true)
        {
            size = 0.5f;
            sphere.transform.localScale = new Vector3(size, size, size);
            caster.GetComponent<Animator>().SetTrigger("HeavyAttack");
            InvokeRepeating("GrowSphere", 1f, 0.05f);
            Invoke("CallAudio", 1);

     //       Debug.Log(level);
            maxDamage = ps.maxDam + ps.addDam;
     //       Debug.Log(maxDamage);
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



    }

    private void CallAudio ()
    {
        GetComponent<AudioSource>().Play();
        rend.enabled = true;
    }


    private void GrowSphere ()
    {
        rate += Time.deltaTime * 12;
        sphere.transform.localScale = Vector3.Lerp(sphere.transform.localScale, new Vector3(finalSize, finalSize, finalSize), rate);
        if (rate >= 1)
        {
            CancelInvoke("GrowSphere");
            UnleashHell();
        }   

    }

    private void UnleashHell ()
    {
        int enemyNo = 0;
        foreach (GameObject go in gc.enemies)
        {
            float distance = Vector3.Distance(transform.position, go.transform.position);
            float maxDistance = finalSize / 2;
            
            if (distance <= maxDistance)
            {
                enemyNo++;
                PlayerStats enemyps = go.GetComponent<PlayerStats>();
                int armor = enemyps.armor;
                int finalDamage = maxDamage - armor;
                if (finalDamage > 0)
                {
                    enemyps.AddjustHealth(-finalDamage, caster, true);
                    if (enemyps.currentRegen > 0)
                    {
                        enemyps.currentRegen = enemyps.currentRegen - finalDamage;
                    }
                    
                  
                }
            }
        }

        if (adrenaline == true)
        {
            int healingPoints = 0;
            for (int cnt = 0; cnt < enemyNo; cnt++)
            {
                Debug.Log("dam");
                healingPoints = healingPoints + Random.Range(1, 3);

            }

            ps.AddjustHealth(healingPoints, caster, false);
        }

        gameObject.SetActive(false);
        rate = 0;
        rend.enabled = false;
        sphere.transform.localScale = new Vector3(size, size, size);

    }

}
