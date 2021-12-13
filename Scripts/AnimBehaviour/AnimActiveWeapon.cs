using UnityEngine;

public class AnimActiveWeapon : StateMachineBehaviour
{
//    private GameObject player = null;
    private GameObject weapon = null;
    // Start is called before the first frame update

    private void OnEnable()
    {
        
    }

    override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
  //      Debug.Log("Enter");
        if (weapon == null)
        {
           

            if (animator.gameObject.GetComponent<PlayerAttack>().weapon != null)
            {
                weapon = animator.gameObject.GetComponent<PlayerAttack>().weapon;
                if (weapon.GetComponent<SphereCollider>() != null)
                {
                    weapon.GetComponent<SphereCollider>().enabled = true;
                    //            Debug.Log("Enter");
                }
                else
                {
                    weapon.GetComponent<CapsuleCollider>().enabled = true;
                }
            }
 
           
        }
        else
        {
            if (weapon.GetComponent<SphereCollider>() != null)
            {
                weapon.GetComponent<SphereCollider>().enabled = true;
    //            Debug.Log("Enter");
            }
            else
            {
                weapon.GetComponent<CapsuleCollider>().enabled = true;
            }
        }

        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (weapon != null)
        {
            if (weapon.GetComponent<SphereCollider>() != null)
            {
                weapon.GetComponent<SphereCollider>().enabled = false;
            }
            else
            {
                weapon.GetComponent<CapsuleCollider>().enabled = false;
            }
        }

    }




}
