using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreGame : MonoBehaviour {

    private GameObject playerCam;
    public bool ready = false;
    
    private string chosen = "Balder";
    

    private void Start()
    {
        if (Camera.main.transform.Find("Portraits/" + gameObject.name)!= null)
        {
            playerCam = Camera.main.transform.Find("Portraits/" + gameObject.name).gameObject;
            playerCam.GetComponent<MeshRenderer>().material = (Material)(Resources.Load("Materials/Balder"));

        }

    }

    public void ChangeModel ( bool right)
    {
        if (chosen == "Balder")
        {
            transform.Find("PCs/Balder").gameObject.SetActive(false);
            chosen = "Fred";
            transform.Find("PCs/Fred").gameObject.SetActive(true);
            playerCam.GetComponent<MeshRenderer>().material =  (Material)(Resources.Load("Materials/Fred"));
        }
        else if (chosen == "Fred")
        {
            transform.Find("PCs/Fred").gameObject.SetActive(false);
            chosen = "Oleg";
            transform.Find("PCs/Oleg").gameObject.SetActive(true);
            playerCam.GetComponent<MeshRenderer>().material = (Material)(Resources.Load("Materials/Oleg"));

        }
        else if (chosen == "Oleg")
        {

            transform.Find("PCs/Oleg").gameObject.SetActive(false);
            chosen = "Kira";
            transform.Find("PCs/Kira").gameObject.SetActive(true);
            playerCam.GetComponent<MeshRenderer>().material = (Material)(Resources.Load("Materials/Kira"));

        }
        else if (chosen == "Kira")
        {
            transform.Find("PCs/Kira").gameObject.SetActive(false);
            chosen = "Nanna";
            transform.Find("PCs/Nanna").gameObject.SetActive(true);
            playerCam.GetComponent<MeshRenderer>().material = (Material)(Resources.Load("Materials/Nanna"));

        }
        else if (chosen == "Nanna")

        {
            transform.Find("PCs/Nanna").gameObject.SetActive(false);
            chosen = "Balder";
            transform.Find("PCs/Balder").gameObject.SetActive(true);
            playerCam.GetComponent<MeshRenderer>().material = (Material)(Resources.Load("Materials/Balder"));

        }
    }
}
