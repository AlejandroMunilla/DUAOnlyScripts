using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPolidoriArena : MonoBehaviour
{
    public GameObject blood;

    private bool alive = true;
    private float counter = 0;
    private float attackCounter = 0;
    private EnemyAI ea;
    private PlayerAttack pa;
    private PlayerStats ps;
    private GameObject target;
    private Animator anim;
    private Transform grab;
    private NavMeshAgent navTarget;
    private float navOffSet;
    private float originOffSet;
    private GameController gc;

    private enum State
    {
        Base,
        Embrace,
        Stun
    }

    private State state;


    // Start is called before the first frame update
    void Start()
    {
        ea = GetComponent<EnemyAI>();
        pa = GetComponent<PlayerAttack>();
        ps = GetComponent<PlayerStats>();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        anim = GetComponent<Animator>();
        grab = transform.Find("Grab");
        Debug.Log(grab.name);
        state = State.Base;
        StartCoroutine(FSM(1));
        Invoke("CheckPlayerStats", 2);
    }

    private IEnumerator FSM(float waitTime)
    {
        while (alive)
        {
            switch (state)
            {
                case State.Base:
                    Base();
                    break;

                case State.Embrace:
                    Embrace();
                    break;
            }


            yield return new WaitForSeconds(waitTime);
        }

    }

    private void Base()
    {

        Debug.Log(counter);
        if (ea.state == EnemyAI.State.MoveToEngage)
        {
            counter++;
            attackCounter = 0;
            if (ea.distToTarget > 4 && ea.distToTarget < 60)
            {
                if (counter >= 8)
                {
                    counter = 0;
                    transform.LookAt(ea.target.transform.position);
                    pa.rangedAttack = true;
                    pa.Attack();
                    pa.rangedAttack = false;
                    
                     Debug.Log("Attack");
                }
            }
        }
        else if (ea.state == EnemyAI.State.Attack)
        {
            attackCounter += 3;
            int diceRoll = Random.Range(0, 100);
            counter = 0;
            
            if (diceRoll <= attackCounter)
            {
                state = State.Embrace;
                target = ea.target;
                ea.state = EnemyAI.State.UsingAbility;
                anim.SetTrigger("AttackSpell1");
                anim.SetBool("Embrace", true);
                counter = 0;
                Invoke("Ascension", 0.01f);
          //      transform.Find("ShockWave").gameObject.SetActive(true);
                GetComponent<AudioSource>().Play();
                PreparePC();
                attackCounter = 0;
            }
        }
    }

    private void Embrace()
    {
        if (ea.state == EnemyAI.State.UsingAbility)
        {

            counter++;
            if (counter >= 5 && counter <= 8)
            {
                blood.SetActive(true);
            }

            else if (counter >= 10)
            {
                navTarget.baseOffset = originOffSet;
                target.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z);
                target.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                target.GetComponent<PlayerStats>().AddjustHealth(-1000, gameObject, true);
                state = State.Base;
                ea.state = EnemyAI.State.Search;
                anim.SetTrigger("IdleTrigger");
                anim.SetBool("Embrace", false);
                attackCounter = 0;
                //    transform.Find("ShockWave").gameObject.SetActive(false);      



            }
        }
        else
        {
          
            BreakAbility();
        }

    }

    private void PreparePC()
    {
        Transform playerGrab = grab.transform.Find(target.name);
        navTarget = target.GetComponent<NavMeshAgent>();
        navTarget.isStopped = true;
        navTarget.enabled = false;
        target.transform.position = playerGrab.position;
        target.transform.rotation = playerGrab.rotation;
        target.GetComponent<ThirdPersonCharacter>().enabled = false;
        target.GetComponent<ThirdPersonUserControl>().enabled = false;
        navTarget.enabled = true;
        navTarget.isStopped = false;
        

        originOffSet = navTarget.baseOffset;
        if (target.name == "Rose")
        {
            navOffSet = 0.1f;
            navTarget.baseOffset = 0.1f;
        }
        else if (target.name == "Fred")
        {
            navOffSet = 1f;
            navTarget.baseOffset = 1f;
        }
        else if (target.name == "Oleg")
        {
            navOffSet = 0.5f;
            navTarget.baseOffset = 0.5f;
        }
        else if (target.name == "Nanna")
        {
            navOffSet = 0;
            navTarget.baseOffset = 0;
        }
        else

        {
            navOffSet = 0.2f;
            navTarget.baseOffset = 0.2f;
        }
        Debug.Log(navTarget.baseOffset);
        navTarget.baseOffset = navOffSet;
    //    navTarget.isStopped = true;
        
 //       navTarget.enabled = false;
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
  //      targetRb.useGravity = false;
        targetRb.constraints = RigidbodyConstraints.FreezeAll;

    }


    
    private void Ascension()
    {
        if (navTarget.baseOffset <= navOffSet)
        {
            navTarget.baseOffset += 0.03f;
            Invoke("Ascension", 0.01f);
        }
    }


    private void BreakAbility ()
    {
        CancelInvoke("Ascension");
        target.GetComponent<ThirdPersonCharacter>().enabled = true;
        target.GetComponent<ThirdPersonUserControl>().enabled = true;
        navTarget.baseOffset = originOffSet;
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        targetRb.constraints = RigidbodyConstraints.None;
        state = State.Base;
        anim.SetBool("Embrace", false);
    }

    private void CheckPlayerStats ()
    {
        bool level6 = false;
        foreach (GameObject go in gc.players)
        {
            if (go.GetComponent<PlayerStats>().level >= 6)
            {
                level6 = true;
            }
        }

        if (level6 == false)
        {
            ps.maxRegen = 0;
            ps.currentRegen = 0;
        }
    }
}
