using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondWeapon : MonoBehaviour
{
    public bool back = true;


    private void Start()
    {
        if (back == true)
        {
            transform.root.GetComponent<ThirdPersonUserControl>().secondWeaponBack = gameObject;
        }
        else
        {
      //      Debug.Log("false");
            transform.root.GetComponent<ThirdPersonUserControl>().secondWeaponHand = gameObject;
            gameObject.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void OnEnable()
    {

        
    }


}
