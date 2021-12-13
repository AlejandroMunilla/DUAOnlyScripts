using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.AI;

public class Active2003 : MonoBehaviour
{
    public GameObject caster;
    private bool loaded = false;
    private bool fire = false;
    private bool skillAvailable = false;
    private bool invisible = false;
    private int addDamage = 0;
    private float coolDownTime = 10;
    private float timer = 90;
    private string idSkill = "2003";
    private string skillstring = "skill1";
    private Vector3 actualPos;
    private PlayerStats ps;
    private PlayerAttack pa;
    private ThirdPersonUserControl tpu;
    private GameController gc;
    private myGUI mygui;
    private Camera cam;
    private GameObject fireBall;

    
   
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
            cam = tpu.cam;
            fireBall = Instantiate(Resources.Load("RangeEffect/FireBall"), caster.transform.position, caster.transform.rotation) as GameObject;
            BulletController bc = fireBall.GetComponent<BulletController>();
            bc.criticalHit = true;

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
                invisible = true;

            }
            else if (DialogueLua.GetActorField(caster.name, skillstring + "/5b").asString == "Yes")
            {
                skillAvailable = true;


            }
            else if (DialogueLua.GetActorField(caster.name, skillstring + "/4").asString == "Yes")
            {
                skillAvailable = true;
            
            }
            else if (DialogueLua.GetActorField(caster.name, skillstring + "/3").asString == "Yes")
            {
                skillAvailable = true;

            }

        }
        Debug.Log("Enable");


        if (skillAvailable == true)
        {

                Shoot(tpu.target);



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

        gameObject.SetActive(false);

        
    }

    private void Shoot (GameObject target)
    {
        fireBall.GetComponent<BulletController>().target = target;
        fireBall.GetComponent<BulletController>().caster = caster;
        fireBall.transform.position = pa.bulletSpawnPoint.transform.position;
        fireBall.SetActive(true);

        if (invisible == true)
        {
            ps.TurnInvisible(5.0f);
        }
    }

}

