using System.Collections;
using PixelCrushers.DialogueSystem;
using UnityEngine;

//Spell. Heal 1D6 + 1/level

public class SummonDalila : MonoBehaviour
{

    public int manaConsumption = 30;
    GameController gc = null;
    GameObject dalila = null;
    private GameObject gCon = null;
    Animator anim;
    Animation animDalila;
    PlayerStats ps;
    GameObject caster;
    private int level;
    private float timer;
    private string noEnoughMana = "doesnt have enough Mana to cast spell";
    private string invokes = "invokes the Entropic Demon Dalila´s necromantic powers";
    private string language = "en";

    void OnEnable()
    {

        if (gc == null)
        {
            gCon = GameObject.FindGameObjectWithTag("GameController");
            gc = gCon.GetComponent<GameController>();
            caster = transform.root.gameObject;
      //      Debug.Log(caster.name);
            ps = caster.GetComponent<PlayerStats>();
            anim = caster.GetComponent<Animator>();
            //    Quaternion morriguRot = Quaternion.Euler(0, 120, 0);
            dalila = Instantiate(Resources.Load("Help/Dalila"), caster.transform.position, caster.transform.rotation) as GameObject;
            dalila.name = "Dalila";
            animDalila = dalila.GetComponent<Animation>();
            timer = Time.timeSinceLevelLoad;
            if (Localization.language == "")
            {
                Localization.language = language;
            }
            ChangeLanguage();
        }

   //     Debug.Log(ps.mana + "/" + manaConsumption + "/" + caster.name);
        if ((ps.mana - manaConsumption) >= 0)
        {
            ps.AddjustMana(-manaConsumption, caster);
            level = caster.GetComponent<PlayerStats>().level;
            if (level < 1)
            {
                level = 1;
            }

            if (gc.enemies.Count > 0)
            {
                foreach (GameObject go in gc.enemies)
                {

                    float distance = Vector3.Distance(go.transform.position, caster.transform.position);

                    if (distance <= 10)
                    {
                        if (go.transform.Find("Effects/Dalila") == null)
                        {
                            Vector3 posEffect = new Vector3(go.transform.position.x, go.transform.position.y + 1, go.transform.position.z);
                            GameObject dalilaGO = Instantiate(Resources.Load("Effects/Dalila"), posEffect, go.transform.rotation) as GameObject;
                            dalilaGO.name = "Dalila";

                            if (go.transform.Find("Effects") == null)
                            {
                                GameObject effectGO = Instantiate(Resources.Load("Effects/Dalila"), go.transform.position, go.transform.rotation) as GameObject;
                                effectGO.name = "Effects";
                                effectGO.transform.parent = go.transform;
                                effectGO.SetActive(true);
                            }
                            else
                            {
                                go.transform.Find("Effects").gameObject.SetActive(true);
                            }

                            dalilaGO.transform.parent = go.transform.Find("Effects");

                        }
                        else
                        {
                            go.transform.Find("Effects/Dalila").gameObject.SetActive(true);
                        }
                    }        //            Debug.Log(go.name);

                    
                }
            }

            anim.SetTrigger("Spell1");
            
            DialogueManager.ShowAlert(caster.name + invokes);
      //      Debug.Log("Emabe");
            Invoke("InvokeEffect", 1);
              

            if (gc.sceneFP == true)
            {
        //        Debug.Log(gc.sceneFP);
                Vector3 localPos = caster.transform.Find ("HelpPlace").position;
                dalila.transform.position = localPos;
            }
            else
            {
           //     Debug.Log(gc.sceneFP);
                dalila.transform.position = caster.transform.position;
            }


     //       dalila.transform.position = caster.transform.forward * 2;

            dalila.transform.rotation = Quaternion.Euler(0, 120, 0);
            animDalila["demongirl_slap2"].speed = 0.3f;
            
            dalila.SetActive(true);
            dalila.GetComponent<AudioSource>().Play();


        }
        else
        {
            if ((timer + 5) < Time.timeSinceLevelLoad)
            {
     //           DialogueManager.ShowAlert(caster.name + noEnoughMana);
                timer = Time.timeSinceLevelLoad;
            }
            gameObject.SetActive(false);


        }


    }

    private void OnDisable()
    {

    }

    private void InvokeEffect()
    {
   //     Debug.Log("Set");

        if (gc.enemies != null)
        {
            if (gc.enemies.Count > 0)
            {
                foreach (GameObject go in gc.enemies)
                {
                    
                    float enemyDistance = Vector3.Distance(go.transform.position, caster.transform.position);
            //        Debug.Log(go.name + "/" + enemyDistance + "/" + caster.name);
                    if (enemyDistance < 30)
                    {
                        InstantiateEffects(go.transform);

                        int damage = Random.Range(1, 8) + 1 + level;
                        go.GetComponent<PlayerStats>().AddjustHealth(-damage, caster, true);
                        if (level < 1)
                        {
                            level = 1;
                        }
                        caster.GetComponent<PlayerStats>().AddjustHealth(3 + level, caster, false);
          //              Debug.Log(go.name + "/withinDistance" + damage);
                    }
                }
            }
        }

        ps.specialActive = false;
        Invoke("SetInactive", 3);
    }

    private void SetInactive()
    {
   //     Debug.Log("SetInactive");
        if (gameObject.activeSelf || dalila.activeSelf)
        {
      //      Debug.Log("SetInactive2");
            Invoke("SetInactive", 0);
            gameObject.SetActive(false);
            dalila.SetActive(false);
        }
   

    }

    private void ChangeLanguage()
    {
        if (language == "es")
        {
            invokes = " invoca los poderes necromanticos del demonio entropico Dalila";
            noEnoughMana = " no tiene suficiente mana para lanzar el conjuro";
        }
        else if (language == "en")
        {
            noEnoughMana = " doesnt have enough Mana to cast the spell";
            invokes = " invokes the Entropic Demon Dalila´s necromantic powers";
        }
        else if (language == "fr")
        {
            invokes = " invoque les pouvoirs nécromantiques du démon entropique Dalila";
            noEnoughMana = " n'a pas assez de Mana pour lancer le sort";
        }
    }

    private void InstantiateEffects (Transform targetFrom)
    {
        GameObject effectNew = Instantiate(Resources.Load("RangeEffect/FireBallGreen"), targetFrom.transform.position, targetFrom.transform.rotation) as GameObject;
        BulletToTarget bToTarget = effectNew.GetComponent<BulletToTarget>();
        bToTarget.target = caster.transform;
        effectNew.name = "GreenBall";
  
        if (bToTarget.enabled == false)
        {
            bToTarget.enabled = true;
        }
        if (effectNew.GetComponent<BulletController>().enabled == true)
        {
            effectNew.GetComponent<BulletController>().enabled = false;
        }

        effectNew.GetComponent<SphereCollider>().enabled = false;

        effectNew.SetActive(true);

    }
}
