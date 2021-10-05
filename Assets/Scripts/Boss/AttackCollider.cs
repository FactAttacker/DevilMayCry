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
            state = BossSystem.Instance.BossStateMachine.CurrState;
            string type = "";

            if (state.GetComponent<BasicAttackState>() == state) type = "BasicAttackState";
            else if (state.GetComponent<StrikeAttackState>() == state) type = "StrikeAttackState";
            else if (state.GetComponent<RushAttackState>() == state) type = "RushAttackState";
            else if (state.GetComponent<JumpAttackState>() == state) type = "JumpAttackState";

            switch (type)
            {
                case "BasicAttackState":
                    if (BasicAttackState.basicAttackCount == 2) {
                        BossSystem.Instance.Boss_DetectPlayerAndCalcDistance.playerScript.knuckBack = true;
                    }
                    col.gameObject.GetComponent<PlayerState>().TakeDamage = GlobalState.bossAttackList[0].Damage;
                    break;
                case "StrikeAttackState":
                    BossSystem.Instance.Boss_DetectPlayerAndCalcDistance.playerScript.flyingBack = true;
                    col.gameObject.GetComponent<PlayerState>().TakeDamage = GlobalState.bossAttackList[1].Damage;
                    break;
                case "RushAttackState":
                    BossSystem.Instance.Boss_DetectPlayerAndCalcDistance.playerScript.flyingBack = true;
                    col.gameObject.GetComponent<PlayerState>().TakeDamage = GlobalState.bossAttackList[2].Damage;
                    break;
                case "JumpAttackState":
                    BossSystem.Instance.Boss_DetectPlayerAndCalcDistance.playerScript.flyingBack = true;
                    col.gameObject.GetComponent<PlayerState>().TakeDamage = GlobalState.bossAttackList[3].Damage;
                    break;
            }
        }
    }
}