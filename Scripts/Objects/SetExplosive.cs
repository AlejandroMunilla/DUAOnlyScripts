using UnityEngine;
using UnityEngine.AI;


public class SetExplosive : MonoBehaviour
{

    public bool reInitialize = false;
    public bool setOnFire = false;
    [SerializeField] int minDam= 1;
    [SerializeField] int maxDam = 10;
    [SerializeField] int addDam = 3;
    [SerializeField] float range = 4;

    private GameController gc;
    private GameObject explosion;
    private bool loaded = false;
    // Start is called before the first frame update
    void Start()
    {
        if (loaded == false)
        {
            loaded = true;
            gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            explosion = transform.Find("Explosion").gameObject;
            explosion.transform.parent = null;

            if (reInitialize == true)
            {
                gc.reinitialize.Add(gameObject);
            }
            gameObject.name = "ExplosiveBarrel";
        }

        GetComponent<NavMeshObstacle>().enabled = true;
    }

    private void OnEnable()
    {
        Start();
    }


    public void TriggerExplosion ()
    {
        explosion.SetActive(true);
        gameObject.SetActive(false);
        GetComponent<NavMeshObstacle>().enabled = false;
        foreach (GameObject go in gc.enemies)
        {
            CheckDamage(go);
        }

        foreach (GameObject go in gc.players)
        {
            CheckDamage(go);
        }
 
    }

    private void CheckDamage (GameObject go)
    {
        float distToEnemie = Vector3.Distance(go.transform.position, transform.position);
     //   Debug.Log(go.name + "/" + distToEnemie);
        if (distToEnemie <= 6)
        {
    //        Debug.Log(distToEnemie);
            int randomDam = Random.Range(minDam, maxDam) + addDam;
            go.GetComponent<PlayerStats>().AddjustHealth(-randomDam, gameObject, true);
            go.GetComponent<PlayerStats>().AddjustRegen(-randomDam, gameObject, false);

            if (setOnFire == true)
            {
                if (go.tag == "Enemy")
                {
                    if (go.transform.Find("Fire") == null)
                    {
                        GameObject fireTemp = Instantiate(Resources.Load("Effects/Fire"), go.transform.position, go.transform.rotation) as GameObject;
                        fireTemp.name = "Fire";
                        fireTemp.transform.parent = go.transform;
                        fireTemp.SetActive(true);
                    }
                    else
                    {
                        go.transform.Find("Fire").gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
