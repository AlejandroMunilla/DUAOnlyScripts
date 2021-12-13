using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.AI;

public class Active2002 : MonoBehaviour
{
    public GameObject caster;
    private bool loaded = false;
    private bool fire = false;
    private int addDamage = 0;
    private int maxTraps = 2;
    private float coolDownTime = 10;
    private float timer = 90;
    private float trapArea = 3;
    private float effectArea = 6;
    private string idSkill = "2002";
    private string skillstring = "skill1";
    private Vector3 actualPos;
    private PlayerStats ps;
    private ThirdPersonUserControl tpu;
    private GameController gc;
    private GameObject activeTrap;
    private List<GameObject> traps = new List<GameObject>();
    private myGUI mygui;
    private Camera cam;


   
   
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
            cam = tpu.cam;

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
        Debug.Log("Enable");
        bool skillAvailable = false;

        if (DialogueLua.GetActorField(caster.name, skillstring + "/5a").asString == "Yes")
        {
            skillAvailable = true;
            maxTraps = maxTraps + 1;

        }
        else if (DialogueLua.GetActorField(caster.name, skillstring + "/5b").asString == "Yes")
        {
            skillAvailable = true;
            maxTraps = maxTraps + 3;
            fire = true;
            trapArea = 5;
            effectArea = 10;
            addDamage = 3;

        }
        else if (DialogueLua.GetActorField(caster.name, skillstring + "/4").asString == "Yes")
        {
            skillAvailable = true;
            maxTraps = maxTraps + 1;
        }
        else if (DialogueLua.GetActorField(caster.name, skillstring + "/3").asString == "Yes")
        {
            skillAvailable = true;
            
        }

  


        if (skillAvailable == true && CheckGround())
        {

            Debug.Log("Spawn Trap");


            if (traps == null || traps.Count < maxTraps)
            {
                GameObject go = Instantiate(Resources.Load("Help/ExplosiveTrap"), actualPos, transform.rotation) as GameObject;
                go.name = "ExplosiveTrap";
                go.SetActive(true);
                traps.Add(go);
                activeTrap = go;
                Debug.Log("Spawn Trap");


            }
            else
            {
                activeTrap = traps[0];
                activeTrap.SetActive(false);
                traps.RemoveAt(0);
                traps.Add(activeTrap);
                activeTrap.transform.position = actualPos;
                activeTrap.SetActive(true);
                Debug.Log("Spawn Trap2");

            }
            TrapExplosive trapExp = activeTrap.GetComponent<TrapExplosive>();
            trapExp.adDam = ps.level + addDamage;
            trapExp.fire = fire;
            trapExp.areaEffect = effectArea;
            trapExp.areaTrap = trapArea;


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


    private bool CheckGround ()
    {
        bool suitableArea = false;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.50f, 0.495f, 0.5f));
        RaycastHit hit;
 
        if (Physics.Raycast(ray, out hit, 3))
        {
            NavMeshHit hitNav;
            if (NavMesh.SamplePosition(hit.point, out hitNav, 1.0f, NavMesh.AllAreas))
            {
                
                suitableArea = true;
                actualPos = hit.point;
                Debug.Log(hit.transform.name);
            }
        }

        return suitableArea;

    }
}

