using UnityEngine;

public class LampLight : MonoBehaviour
{
    // Start is called before the first frame update
    private Material darkMaterial;
    private int health = 15;
    private Quaternion finalRotation;
    private GameObject hitEffect;


    private void Start()
    {
        darkMaterial = (Material)(Resources.Load("Materials/Lanterns UV Dark"));
        finalRotation = Quaternion.Euler(0, 90, 0);
        hitEffect = Instantiate(Resources.Load("Effects/HitEffect"), new Vector3 (transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation) as GameObject;
        hitEffect.transform.parent = transform;


    }

    public void AddjustHealth(int dam)
    {
        health = health - dam;
        hitEffect.SetActive(true);

        if (health <= 0)
        {
            health = 0;
            StartDown();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            GameObject attacker =   other.transform.root.gameObject;
            PlayerStats ps = attacker.GetComponent<PlayerStats>();

            int dam = Random.Range(ps.minDam, ps.maxDam) + ps.addDam;
        //    Debug.Log(dam);
            AddjustHealth(dam);
        }
    }

    private void StartDown ()
    {
        GetComponent<MeshRenderer>().material = darkMaterial;
        transform.Find("Point Light").gameObject.SetActive(false);
        Invoke("TurnDownLamp", 0);
    }

    private void TurnDownLamp ()
    {
        //  transform.localRotation *= Quaternion.AngleAxis(15 * Time.deltaTime, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, finalRotation, 0.2f);
     //   Debug.Log(transform.eulerAngles.x + "/" + finalRotation );
        
        
        if (transform.eulerAngles.x == 0)
        {
            
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<AudioSource>().Play();
            this.enabled = false;
        }
        else
        {
            Invoke("TurnDownLamp", 0);
        }
    }

}
