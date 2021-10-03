using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    BossState state;

    private void OnCollisionEnter(Collision col)
    {
        state = BossSystem.Instance.BossStateMachine.CurrState;
        if (col.gameObject.CompareTag("Player"))
        {
            
            switch(state.ToString())
            {
                case "BasicAttackState":
                    //col.gameObject.GetComponent<PlayerStatus>().TakeDamage = ;
                    break;
                case "StrikeAttackState":
                    //col.gameObject.GetComponent<PlayerStatus>().TakeDamage = ;
                    break;
                case "RushState":
                    //col.gameObject.GetComponent<PlayerStatus>().TakeDamage = ;
                    break;
                case "JumpAttackState":
                    //col.gameObject.GetComponent<PlayerStatus>().TakeDamage = ;
                    break;
            }
            print("Ãæµµ¿Ã!");
        }
        print("Ha");
    }
}
