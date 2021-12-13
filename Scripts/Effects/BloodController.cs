using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodController : MonoBehaviour {

    private int[] bloodList = new int[4];
	
	void OnEnable ()
    {
        int bloodNo = transform.childCount;

        for (int cnt = 0; cnt < bloodList.Length; cnt ++)
        {
            bloodList[cnt] = Random.Range(1, bloodNo);
            transform.Find(bloodList[cnt].ToString()).gameObject.SetActive(true);
        }

        Invoke("DisableAll", 1);


    }

    private void DisableAll ()
    {
        foreach (Transform ta in transform)
        {
            if (ta.gameObject.activeSelf)
            {
                ta.gameObject.SetActive(false);
            }
        }

        gameObject.SetActive(false);
    }



}
