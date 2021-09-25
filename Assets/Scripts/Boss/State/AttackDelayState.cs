using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackDelayState : BossState
{
    Coroutine Co_attackDelay;
    public override void OnAwake()
    {

    }

    public override void OnStart()
    {
        Co_attackDelay = StartCoroutine(Co_AttackDelay());
    }

    public override void OnUpdate()
    {

    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnEnd()
    {

    }

    public override void OnReset()
    {

    }

    IEnumerator Co_AttackDelay()
    {
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        bossStateMachine.SetState(GetComponent<IdleState>());
        StopCoroutine(Co_attackDelay);
    }
}
