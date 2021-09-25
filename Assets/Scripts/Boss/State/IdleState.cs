using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IdleState : BossState
{
    Coroutine Co_IdleCycle;

    [SerializeField] float farDist = 10;
    [SerializeField] int roarWeight = 100, attackWeight = 100, rushWeight = 100, strikeWeight = 100, jumpWeight = 100;
    [SerializeField] float normal;

    public override void OnAwake()
    {

    }

    public override void OnStart()
    {
        Co_IdleCycle = StartCoroutine(Co_DecideNextState());
        //StartCoroutine(임시());
    }

    public override void OnUpdate()
    {
        normal = bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
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

    // 테스트용 함수 - 나중에 지울 것
    IEnumerator 임시()
    {
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).IsName("Roar State"));
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        bossStateMachine.SetState(GetComponent<BasicAttackState>());

    }


    float damagedHP = 70;
    /// <summary> 플레이어와 보스의 거리를 체크한 다음 거리에 따라 무작위 상태로 진입하는 함수 </summary>
    /// <returns></returns>
    IEnumerator Co_DecideNextState()
    {
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle State"));
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f);

        if(GetComponent<BossHP>().CurrHP <= damagedHP)
        {
            yield return StartCoroutine(Co_Damaged());
        }

        if (boss_DetectPlayerAndCalcDistance.distance >= farDist)
        {
            int weightAmount = rushWeight + jumpWeight;
            int randomWeight = Random.Range(0, weightAmount + 1);
            BossState _nextState;
            
            if (randomWeight <= rushWeight)
                _nextState = GetComponent<RushAttackState>();
            else
                _nextState = GetComponent<JumpAttackState>();
            bossStateMachine.SetState(_nextState);
        }
        else
        {
            int weightAmount = roarWeight + attackWeight + strikeWeight;
            int randomWeight = Random.Range(0, weightAmount + 1);
            BossState _nextState;

            if (randomWeight <= roarWeight)
                 _nextState = GetComponent<RoarState>();
            else if (randomWeight > roarWeight && randomWeight <= roarWeight + attackWeight)
                _nextState = GetComponent<BasicAttackState>();
            else
                _nextState = GetComponent<StrikeAttackState>();
            bossStateMachine.SetState(_nextState);
        }
        StopCoroutine(Co_IdleCycle);
    }

    IEnumerator Co_Damaged()
    {
        // 데미지 처리
        yield return null;
    }
}
