using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCinematic : MonoBehaviour
{
    public static bool isNext = false;

    public IEnumerator Co_BossCinematic()
    {
        IdleState idleState = GetComponent<IdleState>();

        yield return new WaitUntil(() => isNext);
        isNext = false;
        BossSystem.Instance.BossStateMachine.SetState(GetComponent<JumpAttackState>());

        yield return new WaitUntil(() => isNext);
        isNext = false;
        BossSystem.Instance.BossStateMachine.SetState(GetComponent<RoarState>());
    }
}
