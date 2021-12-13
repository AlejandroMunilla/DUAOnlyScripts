using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpear : MonoBehaviour
{

    private bool alive = true;
    private bool up = true;
    private float timeChange = 1.6f;
    private float rate = 5;
    private Vector3 originalPos;
    private Vector3 finalPos;

    private enum State
    {
        Seq01,
        Seq02,
        Seq03
    }
    private State state;

    private IEnumerator FSM()
    {
        while (alive)
        {
            switch (state)
            {
                case State.Seq01:
                    Seq01();
                    yield return new WaitForSeconds(0.1f);
                    break;

                case State.Seq02:
                    Seq02();
                    yield return new WaitForSeconds(0.1f);
                    break;

                case State.Seq03:                    
                    yield return new WaitForSeconds(1);
                    Seq03();
                    break;

            }
        }
        yield return null;
    }
    // Start is called before the first frame update
    void OnEnable ()
    {
        Debug.Log("Enable");
        originalPos = transform.position;
        finalPos = new Vector3(originalPos.x, originalPos.y + 1.48f, originalPos.z);
        rate = 0;
        state = State.Seq01;
        StartCoroutine("FSM");


    }

    private void Seq01 ()
    {
        rate = rate +  Time.deltaTime * 8;
     //   transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f, transform.position.z);
        transform.position = Vector3.Lerp(originalPos, finalPos, rate);

        Debug.Log("Seq01");
        if (rate >= 1)
        {
            rate = 1;
            up = false;
            state = State.Seq03;
            transform.position = finalPos;
        }
    }


    private void Seq02()
    {
        rate = rate  - Time.deltaTime * 10;
        transform.position = Vector3.Lerp(finalPos, originalPos, rate);

        if (rate <= 0)
        {
            rate = 0;
            up = true;
            state = State.Seq03;                
            transform.position = originalPos;  
        }
    }


    private void Seq03()
    {
  //      Debug.Log("Seq03");
        if (up == true)
        {
            state = State.Seq01;
        }
        else
        {
            state = State.Seq02;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.tag == "Player" || other.tag == "Enemy" || other.tag == "Ally")
        {
            Debug.Log(other.name);
            int damage = Random.Range(1, 10) + 1;
            other.gameObject.GetComponent<PlayerStats>().AddjustHealth(-damage, gameObject, true);
        }
    }

}
