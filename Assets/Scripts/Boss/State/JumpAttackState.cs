using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JumpAttackState : BossState
{
    Coroutine Co_jumpAttackCycle;
    public override void OnAwake()
    {
        
    }

    public override void OnStart()
    {
        Co_jumpAttackCycle = StartCoroutine(Co_JumpAttackCycle());
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

    IEnumerator Co_JumpAttackCycle()
    {
        bossStateMachine.anim.SetTrigger("Jump");
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).IsName("Jump State"));
        yield return StartCoroutine(Co_Jump());
        bossStateMachine.anim.SetTrigger("StrikeAttack");
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).IsName("Jump Strike Attack State"));
        yield return StartCoroutine(Co_StrikeAttack());
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        bossStateMachine.SetState(GetComponent<AttackDelayState>());
        StopCoroutine(Co_jumpAttackCycle);
    }

    IEnumerator Co_Jump()
    {
        //점프 구현
        yield return null;
    }

    IEnumerator Co_StrikeAttack()
    {
        //내려찍기 구현
        yield return null;
    }
}
