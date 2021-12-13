using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignWeapon : MonoBehaviour
{
    public Material normalMat;
    public bool secondWeapon = false;
    private GameObject caster;
    // Start is called before the first frame update
    void Start()
    {
        caster = transform.root.gameObject;
        caster.GetComponent<PlayerAttack>().weapon = gameObject;
    //    Debug.Log(transform.root.name);

        if (gameObject.GetComponent<SphereCollider>() != null)
        {
            gameObject.GetComponent<SphereCollider>().enabled = false;
        }
        else if (gameObject.GetComponent<CapsuleCollider>()!= null)
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
       
        if (secondWeapon == true)
        {
            transform.parent.gameObject.SetActive(false);
        }

    }

    public void OnTriggerEnter(Collider col)
    {
     //   Debug.Log(col.name);
        if (col.gameObject.tag == "Inter")
        {
    //        Debug.Log(col.gameObject.name);
            if (col.gameObject.tag == "Inter")
            {
                GameObject go = col.gameObject;
                if (go.GetComponent<DestroyObject>() != null)
                {
                    go.GetComponent<DestroyObject>().DisableObjectEffect();
                }
                else if (col.gameObject.name == "ExplosiveBarrel")
                {
                    col.gameObject.GetComponent<SetExplosive>().TriggerExplosion();
                }
                else if (col.gameObject.name == "Forge")
                {
                    col.gameObject.GetComponent<SetForge>().TriggerExplosion();
                }
                else if (col.gameObject.name == "ShortCut")
                {
                    col.gameObject.GetComponent<ShortCut>().QuickTravel(col.gameObject.transform.root.gameObject);
                }
                else if (col.gameObject.GetComponent<ObjectStats>() != null)
                {
                    PlayerStats ps = caster.GetComponent<PlayerStats>();
                    int totalDamage = ps.maxDam + ps.addDam;
                    col.gameObject.GetComponent<ObjectStats>().AddjustHealth(-totalDamage);
                }
            }

        }
    }


    /*
    public void OnCollisionEnter (Collision col)
    {
        Debug.Log(col.gameObject.name);
        if (col.gameObject.tag == "Inter")
        {
            GameObject go = col.gameObject;
            if (go.GetComponent<DestroyObject>() !=null)
            {
                go.GetComponent<DestroyObject>().DisableObjectEffect();
            }
        }
    }*/


}
