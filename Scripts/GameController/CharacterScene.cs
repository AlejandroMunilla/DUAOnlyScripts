using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScene : MonoBehaviour
{
    private bool alive = true;
    private GameObject pc;
    private Texture2D background;
    private Texture2D pic;
    private Texture2D frame;
    private Texture2D leftArrow;
    private Texture2D rightArrow;
    private Animator anim;
    private Vector3  initialPos;

    private enum StateBalder
    {
        Attack,
        Jump,
        Block,
        SpecialPower
    }
    private StateBalder stateBalder;

    // Start is called before the first frame update
    void Start()
    {
        pc = transform.Find("Players/PCs/Nanna").gameObject;
        anim = pc.GetComponent<Animator>();
        pic = (Texture2D)(Resources.Load("Portraits/Balder"));
        background = (Texture2D)(Resources.Load("GUI/Bigboard"));
        initialPos = pc.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(Screen.width * 0.4f, 0, Screen.width * 0.6f, Screen.height), background);
        Balder();

    }

    private void Balder ()
    {
        if (GUI.Button (new Rect(Screen.width * 0.43f, 0, Screen.width * 0.3f, Screen.height * 0.1f), "Attack"))
        {
            BalderAttack();
 
        }
        if (GUI.Button(new Rect(Screen.width * 0.43f, Screen.height * 0.1f, Screen.width * 0.3f, Screen.height * 0.1f), "Block"))
        {
            BalderBlock();

        }
        GUI.Button(new Rect(Screen.width * 0.43f, Screen.height * 0.2f, Screen.width * 0.3f, Screen.height * 0.1f), "Jump");
        GUI.Button(new Rect(Screen.width * 0.43f, Screen.height * 0.3f, Screen.width * 0.3f, Screen.height * 0.1f), "Special Power");

    }

    private IEnumerator FSMBalder ()
    {
        while (alive)
        {
            switch (stateBalder)
            {
                case StateBalder.Attack:
                    
                    int attackNo = Random.Range(1, 4);
                    anim.SetTrigger("Attack" + attackNo.ToString());
                    yield return new WaitForSeconds(5);
                    pc.transform.position = initialPos;
                    break;

                case StateBalder.Block:

                    pc.transform.position = initialPos;
                    Debug.Log("Block");
                    yield return new WaitForSeconds(2);
                    break;

                case StateBalder.Jump:
                    pc.transform.position = initialPos;
                    anim.SetBool("OnGround", false);
                    yield return new WaitForSeconds(5);

                    break;

                case StateBalder.SpecialPower:
                    pc.transform.position = initialPos;
                    anim.SetTrigger("Spell1");
                    yield return new WaitForSeconds(6);
                    break;
            }

        }
        yield return null;
    }

    private void BalderAttack ()
    {
        Debug.Log("Attack");
        StopAllCoroutines();
        StartCoroutine("FSMBalder");
        anim.SetBool("Blocking", false);
        anim.SetBool("OnGround", true);
        stateBalder = StateBalder.Attack;
    }

    private void BalderBlock()
    {
        Debug.Log("Block");
        StopAllCoroutines();
        stateBalder = StateBalder.Block;
        StartCoroutine("FSMBalder");
        anim.SetBool("Blocking", true);
        anim.SetBool("OnGround", true);
   
    }
    
}
