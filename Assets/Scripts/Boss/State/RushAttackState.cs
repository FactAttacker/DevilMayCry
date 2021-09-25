using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RushAttackState : BossState
{
    Coroutine Co_rushAttackCycle;

    [SerializeField] float attackDist = 1;

    public override void OnAwake()
    {
        
    }

    public override void OnStart()
    {
        Co_rushAttackCycle = StartCoroutine(Co_RushAttackCycle());
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

    IEnumerator Co_RushAttackCycle()
    {
        bossStateMachine.anim.SetTrigger("Rotate");
        yield return GetComponent<BossRotate>().Co_RushRotate();
        bossStateMachine.anim.SetTrigger("Rush");
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).IsName("Rush State"));
        yield return StartCoroutine(Co_Rush());
        yield return StartCoroutine(Co_RushAttack());
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).IsName("Rush Attack State"));
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        bossStateMachine.SetState(GetComponent<AttackDelayState>());
        StopCoroutine(Co_rushAttackCycle);
    }

    [SerializeField] float rushSpeed = 10;
    IEnumerator Co_Rush()
    {
        Vector3 targetTr = boss_DetectPlayerAndCalcDistance.playerTr.position;
        while (Vector3.Distance(transform.position, targetTr) > attackDist)
        {
            transform.position += transform.forward * Time.deltaTime * rushSpeed;
            yield return null;
        }
    }

    IEnumerator Co_RushAttack()
    {
        float rate = 1;
        bossStateMachine.anim.SetTrigger("RushAttack");
        while(rate > 0)
        {
            transform.position += transform.forward * Time.deltaTime * rushSpeed * rate;
            rate -= Time.deltaTime * 2;
        }
        yield return null;
    }
    public void RushAttack() => StartCoroutine(Co_RushAttack());
}
