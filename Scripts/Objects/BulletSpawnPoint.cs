using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.root.gameObject.GetComponent<PlayerAttack>().bulletSpawnPoint = gameObject;
    }


}
