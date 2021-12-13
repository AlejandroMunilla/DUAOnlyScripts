using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class Active2000 : MonoBehaviour
{
    public GameObject caster;
    private bool loaded = false;
    private bool livingIlusion = false;
    private bool destroyOnTime = false;
    private int addDamage = 0;
    private float coolDownTime = 20;
    private float timer = 90;
    private string idSkill = "2000";
    private string skillstring = "skill1";
    private GameObject fairy;
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
            GameObject gcon = GameObject.FindGameObjectWithTag("GameController");
            gc = gcon.GetComponent<GameController>();
            mygui = gcon.GetComponent<myGUI>();
            ps = caster.GetComponent<PlayerStats>();
            PlayerAttack pa = caster.GetComponent<PlayerAttack>();
            tpu = caster.GetComponent<ThirdPersonUserControl>();
            fairy =  Instantiate(Resources.Load("Ally/Fairy"), caster.transform.position, caster.transform.rotation) as GameObject;
    //        fairy.SetActive(false);

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

        bool skillAvailable = false;

        if (DialogueLua.GetActorField(caster.name, skillstring + "/5a").asString == "Yes")
        {
            skillAvailable = true;
            livingIlusion = true;
        }
        else if (DialogueLua.GetActorField(caster.name, skillstring + "/5b").asString == "Yes")
        {
            skillAvailable = true;
            addDamage = caster.GetComponent<PlayerStats>().level;
            destroyOnTime = true;
            if (destroyOnTime == true)
            {
                GameObject go2 = Instantiate(Resources.Load("Effects/DamageOnDestroyed"), fairy.transform.position, fairy.transform.rotation) as GameObject;
                go2.name = "DamageOnDestroyed";
                go2.transform.parent = fairy.transform;
                go2.SetActive(true);
                DamageOnDestroy dondes = go2.GetComponent<DamageOnDestroy>();
                dondes.addDam = ps.level;
            }
            //   

        }
        else if (DialogueLua.GetActorField(caster.name, skillstring + "/4").asString == "Yes")
        {
            skillAvailable = true;
        }
        else if (DialogueLua.GetActorField(caster.name, skillstring + "/3").asString == "Yes")
        {
            skillAvailable = true;
        }


        if (skillAvailable == true)
        {
            caster.GetComponent<Animator>().SetTrigger("Spell1");
            Invoke("EndEffect", timer);
            fairy.transform.position = caster.transform.position + (caster.transform.forward * 4);
            fairy.SetActive(true);
            SetUpFairy();
            gameObject.SetActive(false);
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

    private void EndEffect ()
    {

        fairy.SetActive(false);
    }

    private void SetUpFairy ()
    {
        PlayerStats psCaster = caster.GetComponent<PlayerStats>();
        PlayerStats ps = fairy.GetComponent<PlayerStats>();

        ps.totalHealth = psCaster.totalHealth;
        ps.health = (int)( psCaster.health * 0.5f);
        ps.minDam = (int) (psCaster.minDam * 0.5f);
        ps.maxDam = (int) (psCaster.maxDam * 0.5f);
        ps.addDam = (int) ( psCaster.addDam * 0.5f);
        fairy.GetComponent<AudioSource>().enabled = true ;
        fairy.GetComponent<AudioSource>().Play();
        Invoke("AudioOff", 2);
        

        if (livingIlusion == true)
        {
            fairy.GetComponent<Animation>().enabled = false;
            fairy.GetComponent<Animator>().enabled = true;
            fairy.GetComponent<EnemyAI>().guardTarget = caster;
            fairy.GetComponent<EnemyAI>().enabled = true;
            fairy.GetComponent<PlayerAttack>().enabled = true;
            ps.setInactiveTime = 0.01f;
           
        }

 


    }

    private void AudioOff ()
    {
        fairy.GetComponent<AudioSource>().enabled = false;
        
    }

}

