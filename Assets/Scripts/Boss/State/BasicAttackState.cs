﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasicAttackState : BossState
{
    Coroutine Co_basicAttackCycle;

    public override void OnAwake()
    {

    }

    public override void OnStart()
    {
        Co_basicAttackCycle = StartCoroutine(Co_BasicAttackCycle());
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

    IEnumerator Co_BasicAttackCycle()
    {
        bossStateMachine.anim.SetTrigger("BasicAttack");
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).IsName("Basic Attack State"));
        //Basic Attack 상태에서의 효과
        yield return StartCoroutine(Co_BasicAttack());
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        bossStateMachine.SetState(GetComponent<AttackDelayState>());
        StopCoroutine(Co_basicAttackCycle);
    }

    IEnumerator Co_BasicAttack()
    {
        //충돌처리 체크
        //이펙트
        yield return null;
    }
    public void BasicAttack() => StartCoroutine(Co_BasicAttack());
    public void SetBasicAttackSpeed(float _attackSpeed) => bossStateMachine.anim.SetFloat("BasicAttackSpeed", _attackSpeed);
}