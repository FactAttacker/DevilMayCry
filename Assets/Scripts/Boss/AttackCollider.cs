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
                    col.gameObject.GetComponent<PlayerState>().TakeDamage = 100;
                    break;
                case "StrikeAttackState":
                    col.gameObject.GetComponent<PlayerState>().TakeDamage = 200;
                    break;
                case "RushState":
                    col.gameObject.GetComponent<PlayerState>().TakeDamage = 300;
                    break;
                case "JumpAttackState":
                    col.gameObject.GetComponent<PlayerState>().TakeDamage = 400;
                    break;
            }
            print("??????!");
        }
        print("Ha");
    }
}
