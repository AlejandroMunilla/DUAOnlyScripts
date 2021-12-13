using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletToTarget : MonoBehaviour
{
    public Transform target;
    private Rigidbody rb;
    private bool alive = true;
    private enum State
    {
        Seq01,
        Seq02,
        Seq03,
        Seq04,
        Seq05
    }
    private State state;

    private IEnumerator FSM()
    {
        while (alive)
        {
            switch (state)
            {
                case State.Seq01:
                    yield return new WaitForSeconds(0.1f);
                    Seq01();
                    break;
                case State.Seq02:
                    yield return new WaitForSeconds(0.1f);
                    break;
                case State.Seq03:
                    yield return new WaitForSeconds(0.1f);
                    break;
            }
        }
        yield return null;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        state = State.Seq01;
        StartCoroutine("FSM");
    }

    private void Seq01()
    {
    //    Debug.Log("Seq91");
        transform.LookAt(target.transform);
        rb.velocity = transform.forward * 10;

        float distanceTarget = Vector3.Distance(target.position, transform.position);
        if (distanceTarget < 1.0f)
        {
            StopCoroutine("FSM");
            InvokeDisable();
        }
    }

    private void InvokeDisable ()
    {

        gameObject.SetActive(false);
    }
}
