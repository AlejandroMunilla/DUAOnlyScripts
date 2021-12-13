using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public int health;
    
    // Start is called before the first frame update
    void Start()
    {

    }


    /*
    public void OnTriggerEnter (Collider col)
    {
        Debug.Log(col.name);

        
        if (col.gameObject.name == "Weapon")
        {
            if (transform.Find("Explosion") != null)
            {
                GameObject go = transform.Find("Explosion").gameObject;
                go.transform.parent = null;
                go.SetActive(true);
            }
            gameObject.SetActive(false);
        }
    }*/

    public void DisableObjectEffect ()
    {
        if (transform.Find("X") != null)
        {
            Debug.Log("Explosion");
            GameObject go = transform.Find("X").gameObject;
            go.transform.parent = null;
            go.SetActive(true);
            Invoke("CheckExplosionDone", 0);
        }
        gameObject.SetActive(false);
    }

    private void CheckExplosionDone ()
    {
        if (transform.Find("X") != null)
        {
            Debug.Log("Explosion");
            GameObject go = transform.Find("X").gameObject;
            go.transform.parent = null;
            go.SetActive(true);
            Invoke("CheckExplosionDone", 0);
        }
    }


}
