using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JumpAttackState : BossState
{
    Coroutine Co_jumpAttackCycle;
    public bool isJumpAtk = false;
    public override void OnAwake()
    {

    }

    public override void OnStart()
    {
        Co_jumpAttackCycle = StartCoroutine(Co_JumpAttackCycle());
        Co_jumpAttackCycle = StartCoroutine(Co_JumpAttack1()); 
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
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        bossStateMachine.SetState(GetComponent<AttackDelayState>());
        StopCoroutine(Co_jumpAttackCycle);
    }

    IEnumerator Co_Jump()
    {
        // 점프 상태일 때 진행될 것
        yield return new WaitUntil(() => isJumpAtk == true);
        isJumpAtk = false;

        yield return null;
    }

    IEnumerator Co_JumpAttack1()
    {
        bossStateMachine.anim.SetTrigger("Jump");
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).IsName("Jump State"));
        yield return new WaitForSeconds(0.5f);
        isJumpAtk = true;
        yield return StartCoroutine(Co_Jump());
        yield return StartCoroutine(Co_JumpAttack2());
    }

    IEnumerator Co_JumpAttack2()
    {
        bossStateMachine.anim.SetTrigger("StrikeAttack");
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).IsName("Jump Strike Attack State"));
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        bossStateMachine.SetState(GetComponent<AttackDelayState>());
        StopCoroutine(Co_jumpAttackCycle);
    }

    public void SetJumpAttackSpeed(float _attackSpeed) => bossStateMachine.anim.SetFloat("JumpAttackSpeed", _attackSpeed);

    public void OnRushAttackEffect(string _effectName)
    {
        Vector3 tempPos = BossSystem.Instance.AttackColliderManager.ColliderArr[2].transform.position;
        tempPos.y = 0;
        BossSystem.Instance.BossAnimationEvents.OnEffect(_effectName, tempPos, Quaternion.identity);
    }
}