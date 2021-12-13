using System.Collections;
using PixelCrushers.DialogueSystem;
using UnityEngine;

//Spell. Heal 1D6 + 1/level

public class Healing : MonoBehaviour {

    public int manaConsumption = 30;
    GameController gc = null;
    GameObject morrigu = null;
    private GameObject gCon = null;
    Animator anim;
    PlayerStats ps;
    GameObject caster;
    private int level;
    private float timer;
    private string noEnoughMana = "doesnt have enough Mana to cast spell";
    private string invokesMorrigu = "invokes Goddness Morrigu`s healing powers";
    private string language = "en";


    void OnEnable ()
    {
   //     Debug.Log("Enable");
        if (gc == null)
        {
            gCon = GameObject.FindGameObjectWithTag("GameController");
            gc = gCon.GetComponent<GameController>();
            caster = transform.root.gameObject;
            ps = caster.GetComponent<PlayerStats>();
            anim = caster.GetComponent<Animator>();
        //    Quaternion morriguRot = Quaternion.Euler(0, 120, 0);
            morrigu = Instantiate(Resources.Load("Help/Morrigu"), caster.transform.position, caster.transform.rotation) as GameObject;
            timer = Time.timeSinceLevelLoad;

     //       Debug.Log(DialogueLua.GetVariable("language").asString);
            language = DialogueLua.GetVariable("language").asString;
            if (Localization.language == "")
            {
                Localization.language = language;
            }

            ChangeLanguage();
        }

        if ((ps.mana - manaConsumption) >=0 )
        {
            ps.AddjustMana(-manaConsumption, caster);
            level = caster.GetComponent<PlayerStats>().level;
            if (level < 1)
            {
                level = 1;
            }

            foreach (GameObject go in gc.players)
            {
         //       Debug.Log(go.name);
                go.transform.Find("Effects/Healing").gameObject.SetActive(true);
            }
            anim.SetTrigger("Spell1");
            DialogueManager.ShowAlert(caster.name + invokesMorrigu);
            ps.specialActive = true;
            Invoke("InvokeHealing", 1);
            morrigu.transform.position = new Vector3 ( caster.transform.position.x, caster.transform.position.y, caster.transform.position.z + 3);
            morrigu.transform.rotation = Quaternion.Euler(0, 180, 0);
            morrigu.SetActive(true);
            GetComponent<AudioSource>().Play();

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

    private void OnDisable()
    {
        
    }

    private void InvokeHealing ()
    {
        foreach (GameObject go in gc.players)
        {
            int heal = Random.Range(3, 12) + level;
            go.GetComponent<PlayerStats>().AddjustHealth(heal, caster, false);
            ps.specialActive = false;

        }

        Invoke("SetInactive", 3f);
    }

    private void SetInactive ()
    {
        gameObject.SetActive(false);
        morrigu.SetActive(false);
    }

    private void ChangeLanguage ()
    {
        if (language == "es")
        {
            invokesMorrigu = " invoca los poderes curativos de la diosa Morrigu";
            noEnoughMana = " no tiene suficiente mana para lanzar el conjuro";
        }
        else if (language == "en")
        {
            noEnoughMana = " doesnt have enough Mana to cast the spell";
            invokesMorrigu = " invokes the goddess Morrigu´s healing powers";

        }
        else if (language == "fr")
        {
            invokesMorrigu = " invoque les pouvoirs de guérison de la déesse Morrigan";
            noEnoughMana = " no tiene suficiente mana para lanzar el conjuro";
        }
    }
}
