using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChandelierFall : MonoBehaviour
{

    public float fallRange;
    private float lerpRate = 0;
    private GameObject rootGO;
    private Vector3 lastRange;
    private Vector3 initialPos;
    private GameObject explosion;
    // Start is called before the first frame update
    void OnEnable ()
    {
       
        rootGO = transform.root.gameObject;
        initialPos = new Vector3(rootGO.transform.position.x, rootGO.transform.position.y, rootGO.transform.position.z);
        lastRange = new Vector3(rootGO.transform.position.x, fallRange, rootGO.transform.position.z);
        Invoke ("FallOver", 0);
    //    Debug.Log(rootGO.name);
        if (rootGO.transform.Find ("Explosion") != null)
        {
            explosion = rootGO.transform.Find("Explosion").gameObject;
            explosion.transform.parent = null;
        }
        else
        {
    //        Debug.Log("null");
        }
    }


    private void FallOver ()
    {
        
        rootGO.transform.position = Vector3.Lerp(rootGO.transform.position, lastRange, Time.deltaTime);
   //     Debug.Log(rootGO.transform.position.y + "/" + lastRange.y);
        if (rootGO.transform.position.y <= lastRange.y + 0.15f)
        {
            Invoke("RiseUp", 0);
            if (explosion != null)
            {
                explosion.SetActive(true);
            }
            
        }
        else
        {
            Invoke("FallOver", 0);
        }


    }

    private void RiseUp()
    {
        lerpRate = lerpRate + Time.deltaTime * 0.2f;
        rootGO.transform.position = Vector3.Lerp(rootGO.transform.position, initialPos, Time.deltaTime);
    //    Debug.Log(rootGO.transform.position.y + "/" + initialPos.y);
        if (rootGO.transform.position.y > initialPos.y -0.15f)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Invoke("RiseUp", 0);
        }
    }



}
