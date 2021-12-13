using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.AI;

public class Active2004 : MonoBehaviour
{
    public GameObject caster;
    private bool loaded = false;
    private bool skillAvailable = false;
    private bool barrierAllies = false;
    private int barrierPoints = 20;
    private float coolDownTime = 60;
    private float timer = 90;
    private float range = 20;
    private string idSkill = "2004";
    private string skillstring = "skill1";
    private GameObject barrier;
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
            GameObject gcon = GameObject.FindGameObjectWithTag("GameController");
            gc = gcon.GetComponent<GameController>();
            mygui = gcon.GetComponent<myGUI>();
            ps = caster.GetComponent<PlayerStats>();
            pa = caster.GetComponent<PlayerAttack>();
            tpu = caster.GetComponent<ThirdPersonUserControl>();
            barrier = Instantiate(Resources.Load("Effects/Barrier"), caster.transform.position, caster.transform.rotation) as GameObject;
            barrier.name = "Barrier";
            barrier.transform.parent = caster.transform;
            barrierPoints = 20 + ps.level;
            
            
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

            if (DialogueLua.GetActorField(caster.name, skillstring + "/5a").asString == "Yes")
            {
                skillAvailable = true;
                barrierPoints = barrierPoints + 30;

            }
            else if (DialogueLua.GetActorField(caster.name, skillstring + "/5b").asString == "Yes")
            {
                skillAvailable = true;
                barrierPoints = barrierPoints + 10;
                barrierAllies = true;

            }
            else if (DialogueLua.GetActorField(caster.name, skillstring + "/4").asString == "Yes")
            {
                skillAvailable = true;
                barrierPoints = barrierPoints + 10;
            }
            else if (DialogueLua.GetActorField(caster.name, skillstring + "/3").asString == "Yes")
            {
                skillAvailable = true;

            }

        }
        Debug.Log("Enable");

        if (skillAvailable == true)
        {
            SetUpBarrier();
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
        barrier.SetActive(true);
        gameObject.SetActive(false);        
    }


    private void SetUpBarrier ()
    {
        if (ps.maxBarrier < barrierPoints)
        {
            ps.maxBarrier = barrierPoints;
        }

        ps.AddjustBarrier(barrierPoints);

        if (barrierAllies == true)
        {
            foreach (GameObject go in gc.players)
            {
                if (go != caster)
                {
                    float distanceToPlayer = Vector3.Distance(caster.transform.position, go.transform.position);
                    if (distanceToPlayer <= 20)
                    {
                        PlayerStats psAlly = go.GetComponent<PlayerStats>();
                        if (psAlly.maxBarrier < 20)
                        {
                            psAlly.maxBarrier = 20;
                            psAlly.AddjustBarrier(20);
                            if (go.transform.Find ("Barrier") != null)
                            {

                            }
                            else
                            {
                                GameObject barrierObj = Instantiate(Resources.Load("Effects/Barrier"), go.transform.position, go.transform.rotation) as GameObject;
                                barrierObj.name = "Barrier";
                                barrierObj.transform.parent = go.transform;
                                barrierObj.SetActive(true);
                            }
                        }
                    }
                }

            }
        }

    }
}

