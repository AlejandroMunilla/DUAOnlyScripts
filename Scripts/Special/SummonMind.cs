using System.Collections;
using PixelCrushers.DialogueSystem;
using UnityEngine;

//Spell. Heal 1D6 + 1/level

public class SummonMind : MonoBehaviour
{

    public int manaConsumption = 20;
    GameController gc = null;
    GameObject mind = null;
    private GameObject gCon = null;
    Animator anim;
    PlayerStats ps;
    GameObject caster;
    private int level;
    private float timer;
    private string noEnoughMana = "doesnt have enough Mana to cast spell";
    private string invokes = "casts mind blast";
    private string language = "en";

    void OnEnable()
    {
    //    Debug.Log("Enable");
        if (gc == null)
        {
            gCon = GameObject.FindGameObjectWithTag("GameController");
            gc = gCon.GetComponent<GameController>();
            caster = transform.root.gameObject;
            ps = caster.GetComponent<PlayerStats>();
            anim = caster.GetComponent<Animator>();
            //    Quaternion morriguRot = Quaternion.Euler(0, 120, 0);
            mind = Instantiate(Resources.Load("Help/MindBlast"), new Vector3 (caster.transform.position.x, caster.transform.position.y, caster.transform.position.z), caster.transform.rotation) as GameObject;
            timer = Time.timeSinceLevelLoad;
            if (Localization.language == "")
            {
                Localization.language = language;
            }
            ChangeLanguage();
        }

        if ((ps.mana - manaConsumption) >= 0)
        {
            ps.AddjustMana(-manaConsumption, caster);
            level = caster.GetComponent<PlayerStats>().level;
            if (level < 1)
            {
                level = 1;
            }

            foreach (GameObject go in gc.enemies)
            {
                //       Debug.Log(go.name);
         //       go.transform.Find("Effects/Dalila").gameObject.SetActive(true);
            }
            anim.SetTrigger("Spell1");
            caster.GetComponent<ThirdPersonUserControl>().enabled = false;
            ps.specialActive = true;
            DialogueManager.ShowAlert(caster.name + invokes);
            Invoke("InvokeEffect", 2);

       //     mind.SetActive(true);


        }
        else
        {
            if ((timer + 5) < Time.timeSinceLevelLoad)
            {
                DialogueManager.ShowAlert(caster.name + noEnoughMana);
                timer = Time.timeSinceLevelLoad;
            }
            gameObject.SetActive(false);


        }


    }

    private void PreEffect ()
    {

    }

    private void InvokeEffect()
    {
        mind.transform.position = new Vector3(caster.transform.position.x, caster.transform.position.y + 2, caster.transform.position.z);
        mind.transform.rotation = Quaternion.Euler(0, 140, 0);
        mind.SetActive(true);
        caster.GetComponent<ThirdPersonUserControl>().enabled = true;
        
        foreach (GameObject go in gc.enemies)
        {
            float enemyDistance = Vector3.Distance(go.transform.position, caster.transform.position);

            if (enemyDistance < 20)
            {
                int damage = level;
                Debug.Log(level);
                go.GetComponent<PlayerStats>().AddjustHealth(-damage, caster, true);

                GameObject mindEffect = Instantiate(Resources.Load("RangeEffect/StunEffect"), new Vector3(go.transform.position.x, go.transform.position.y + 2, go.transform.position.z), go.transform.rotation) as GameObject;
                GameObject smallExplosion = Instantiate(Resources.Load("RangeEffect/SmallExplosion"), new Vector3(go.transform.position.x, go.transform.position.y + 1, go.transform.position.z), go.transform.rotation) as GameObject;
                EnemyAI ea = go.GetComponent<EnemyAI>();
                mindEffect.GetComponent<DisableOnTime>().timer = 10;
                ea.ChangeToStun(10);

            }
        }
        ps.specialActive = false;
        Invoke("SetInactive", 1);
    }

    private void SetInactive()
    {
        gameObject.SetActive(false);
        mind.SetActive(false);
    }

    private void ChangeLanguage()
    {
        if (language == "es")
        {
            invokes = " lanza explosión mental";
            noEnoughMana = " no tiene suficiente mana para lanzar el conjuro";
        }
        else if (language == "en")
        {
            noEnoughMana = " doesnt have enough Mana to cast the spell";
            invokes = " casts Mind Blast";
        }
        else if (language == "fr")
        {
            invokes = " invoque Attaque Mentale";
            noEnoughMana = " n'a pas assez de Mana pour lancer le sort";
        }
    }

    
}
