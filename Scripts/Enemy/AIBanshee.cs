using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBanshee : MonoBehaviour
{
    private bool alive = true;    
    private float counter;
    private float originalOffSet;
    private EnemyAI ea;
    private PlayerAttack pa;
    private GameObject target;
    private Animator anim;
    private Transform grab;
    private NavMeshAgent navTarget;
    private float navOffSet;

    private enum State
    {
        Base,
        Embrace,
        Stun
    }

    private State state;


    // Start is called before the first frame update
    void OnEnable ()
    {
        ea = GetComponent<EnemyAI>();
        pa = GetComponent<PlayerAttack>();
        anim = GetComponent<Animator>();        
        grab = transform.Find("Grab");
        state = State.Base;
        
        StartCoroutine(FSM(1));
    }

    private IEnumerator FSM(float waitTime)
    {
        while (alive)
        {
            switch(state)
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

    private void Base ()
    {
        
   //     Debug.Log(counter);
        if (ea.state == EnemyAI.State.MoveToEngage)
        {
            counter++;
            if (ea.distToTarget > 4 && ea.distToTarget < 60)
            {
                if (counter >= 5)
                {
                    counter = 0;
                    transform.LookAt(ea.target.transform.position);
                    pa.rangedAttack = true;
                    pa.Attack();
                    pa.rangedAttack = false;
                }
            }
        }
        else if (ea.state == EnemyAI.State.Attack)
        {
            counter += 3;
            int diceRoll = Random.Range(0, 100);
  
            if (diceRoll <= counter && ea.target != null)
            {
                state = State.Embrace;
                target = ea.target;
                ea.state = EnemyAI.State.UsingAbility;
                anim.SetTrigger("AttackSpell1");
                counter = 0;
                Invoke("Ascension", 0.01f);
                transform.Find("ShockWave").gameObject.SetActive(true);
                GetComponent<AudioSource>().Play();                
                PreparePC();
            }
        }
    }

    private void Embrace ()
    {
     //   Debug.Log("Embrace");

        counter++;
        Debug.Log(counter);
        if (counter >= 10)
        {
            navTarget.baseOffset = navOffSet;
            target.transform.position = grab.transform.position;
            target.transform.rotation = grab.transform.rotation;
            target.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            target.GetComponent<PlayerStats>().AddjustHealth(-1000, gameObject, true);
            state = State.Base;
            ea.state = EnemyAI.State.Search;
            anim.SetTrigger("IdleTrigger");
            transform.Find("ShockWave").gameObject.SetActive(false);
            
        }
    }

    private void PreparePC ()
    {
     //   target = ea.target;
        target.transform.position = grab.position;
        target.transform.rotation = grab.rotation;
        target.GetComponent<ThirdPersonCharacter>().enabled = false;
        target.GetComponent<ThirdPersonUserControl>().enabled = false;
        target.GetComponent<ThirdPersonUserControl>().cam.GetComponent<MouseOrbitImproved>().enabled = false;
        navTarget = target.GetComponent<NavMeshAgent>();
        navOffSet = navTarget.baseOffset;
        navTarget.isStopped = true;
        target.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;


    }

    private void Ascension ()
    {
        if (navTarget.baseOffset <= navOffSet + 0.5f)
        {
            navTarget.baseOffset += 0.01f;
            Invoke("Ascension", 0.06f);
            target.GetComponent<ThirdPersonUserControl>().cam.transform.LookAt(transform.Find("LookAt"));
            target.GetComponent<ThirdPersonUserControl>().cam.transform.position = target.transform.Find("Camera").transform.position;

        }
        else
        {
            EndAttack();
        }
    }

    private void EndAttack ()
    {
        target.GetComponent<ThirdPersonUserControl>().cam.GetComponent<MouseOrbitImproved>().enabled = true;
        target.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        target.GetComponent<PlayerStats>().AddjustHealth(-1000, gameObject, false);
        navTarget.baseOffset = navOffSet;
    }




}
