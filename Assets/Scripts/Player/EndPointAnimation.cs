using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointAnimation : MonoBehaviour
{
    public void EndPoint() 
    {
        Animator ani = GetComponent<Animator>();
        ani.SetTrigger("AttackEnd");
        //ani.ResetTrigger("secoundAttack");
        //ani.ResetTrigger("thirdAttack");
        //ani.ResetTrigger("lastAttack");
        //ani.ResetTrigger("hiddenAttack");
        
    }
}
