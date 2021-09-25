using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StrikeAttackState : BossState
{
    Coroutine Co_strikeAttackCycle;
    public override void OnAwake()
    {
        
    }

    public override void OnStart()
    {
        Co_strikeAttackCycle = StartCoroutine(Co_StrikeAttackCycle());
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

    IEnumerator Co_StrikeAttackCycle()
    {
        bossStateMachine.anim.SetTrigger("StrikeAttack");
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).IsName("Strike Attack State"));
        //Strike Attack 상태에서의 효과
        yield return StartCoroutine(Co_StrikeAttack());
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        bossStateMachine.SetState(GetComponent<AttackDelayState>());
        StopCoroutine(Co_strikeAttackCycle);
    }

    IEnumerator Co_StrikeAttack()
    {
        yield return null;
    }
    public void StrikeAttack() => StartCoroutine(Co_StrikeAttack());

    public void SetStrikeAttackSpeed(float _attackSpeed) => bossStateMachine.anim.SetFloat("StrikeAttackSpeed", _attackSpeed);
}
