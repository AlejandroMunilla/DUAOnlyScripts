using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class CatchMe : MonoBehaviour
{
    public int manaConsumption = 30;
    public GameObject gladiusHand;
    public GameObject gladiusClavicle;
    public GameObject riflleHand;
    public GameObject rifleClavicle;
    public float duration = 10;
    private bool alive = true;
    private int level;
    private float timer;
    private string noEnoughMana = "doesnt have enough Mana to cast spell";
    private string invokesMorrigu = "uses his special ability Catch Me If You Can";
    private string language = "en";
    private AudioClip fredVoice;
    private AudioSource audioS;
    private GameController gc = null;
    private GameObject gCon = null;
    private Animator anim;
    private PlayerStats ps;
    private PlayerAttack pa;
    private GameObject caster;
    private ThirdPersonUserControl tpc;

    private List<Material> parts = new List<Material>();

    private enum State
    {
        Seq01,
        Seq02,
        Seq03
    }

    private State state;


    void OnEnable()
    {
        //     Debug.Log("Enable");
        if (gc == null)
        {
            gCon = GameObject.FindGameObjectWithTag("GameController");
            gc = gCon.GetComponent<GameController>();
            caster = transform.root.gameObject;
            ps = caster.GetComponent<PlayerStats>();
            pa = caster.GetComponent<PlayerAttack>();
            anim = caster.GetComponent<Animator>();
            audioS = caster.GetComponent<AudioSource>();
            tpc = caster.GetComponent<ThirdPersonUserControl>();
            //    Quaternion morriguRot = Quaternion.Euler(0, 120, 0);
            timer = Time.timeSinceLevelLoad;
   

            //       Debug.Log(DialogueLua.GetVariable("language").asString);
            language = DialogueLua.GetVariable("language").asString;
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

            ps.TurnInvisible(duration);

        //    anim.SetTrigger("Spell1");
            DialogueManager.ShowAlert(caster.name + invokesMorrigu);
            ps.specialActive = true;
            Invoke("InvokePower", 0);
            GetComponent<AudioSource>().Play();

        }
        else
        {
            if ((timer + 1) < Time.timeSinceLevelLoad)
            {
                DialogueManager.ShowAlert(caster.name + noEnoughMana);
                timer = Time.timeSinceLevelLoad;
            }
            gameObject.SetActive(false);


        }


    }



    private void InvokePower()
    {
       float timeToExit = 10;

        rifleClavicle.SetActive(true);
        gladiusHand.SetActive(true);
        gladiusClavicle.SetActive(false);
        riflleHand.SetActive(false);
        ps.attackSpecial = true;
        pa.specialActive = "CatchMe";
        pa.rangedAttack = false;
        ps.invisible = true;
        //Activate knife, deact pistol. 
        //invisible = true;
        audioS.clip = fredVoice;
        audioS.Play();
        state = State.Seq01;
        StartCoroutine("FSM");
        Invoke("SetInactive", timeToExit);
    }



    private void ChangeLanguage()
    {
    //    Debug.Log("ChangeLanguate");
        if (language == "es")
        {
            invokesMorrigu = " usa la habilidad: Cogeme Si Puedes";
            noEnoughMana = " no tiene suficiente mana para usar su abilidad special";
            
        }
        else if (language == "en")
        {
            noEnoughMana = " doesnt have enough Mana to use his special ability";
            invokesMorrigu = " uses his ability Catch Me If You Can";

        }
        else if (language == "fr")
        {
            invokesMorrigu = " utilise sa capacité: Attrape-Moi Si Tu Peux";
            noEnoughMana = " nn´a pas asses de mana pour utiliser sa capacité spéciale";
        }
        fredVoice = (AudioClip)(Resources.Load("Audio/" + language + "/Fred/CatchMe"));
        caster.GetComponent<AudioSource>().clip = fredVoice;
    }


    private IEnumerator FSM()
    {
        while (alive)
        {
            switch (state)
            {
                case State.Seq01:
                    yield return new WaitForSeconds(0);
                    Seq01();
                    break;

                case State.Seq02:
                    yield return new WaitForSeconds(0);
                    Seq02();
                    break;
            }
        }
        yield return null;
    }

    private void Seq01()
    {
    //    Debug.Log(ps.attackSpecial);
        tpc.timer = Time.timeSinceLevelLoad + 1.3f;
        if (ps.attackSpecial == false)
        {
            CancelInvoke("SetInactive");            
            pa.Attack();
            SetInactive();
            anim.SetTrigger("BackStab");
            state = State.Seq02;
        }
    }

    private void Seq02()
    {

    }


    private void SetInactive()
    {
   //     Debug.Log("efewf");
        Invoke("ChangeWeaponBack", 1.2f);
        StopCoroutine("FSM");
        ps.invisible = false;
        ps.TurnVisible();
        gameObject.SetActive(false);
    }

    public void Execution(GameObject enemy, int maxDamage)
    {
        pa.specialActive = null;
        PlayerStats enemyStats = enemy.GetComponent<PlayerStats>();

        if (enemyStats.boss == false)
        {
            enemyStats.AddjustHealth(-1000, caster, false);
            enemyStats.Death();
            enemyStats.CriticalHit();
      //      enemy.SetActive(false);
        }
        else
        {
            enemyStats.AddjustHealth((-(maxDamage * 3)), caster, false);
        }

        //  audio Brutal gCon.transform.Find("Barks").
    }

    private void ChangeWeaponBack ()
    {
        pa.rangedAttack = true;
        ps.specialActive = false;
        rifleClavicle.SetActive(false);
        gladiusHand.SetActive(false);
        gladiusClavicle.SetActive(true);
        riflleHand.SetActive(true);
        pa.specialActive = "null";
    }
}
