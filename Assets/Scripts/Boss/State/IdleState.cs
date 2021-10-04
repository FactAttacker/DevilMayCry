using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IdleState : BossState
{
    Coroutine Co_IdleCycle;

    [SerializeField] float farDist = 10;
    [SerializeField] int roarWeight = 100, attackWeight = 100, rushWeight = 100, strikeWeight = 100, jumpWeight = 100;

    [SerializeField] bool isBasicAttack = false, isRush = false, isStrike = false, isJump = false;
    public bool isRoar = false;

    public bool IsCinematic
    {
        get => isCinematic;
        set => isCinematic = value;
    }
    bool isCinematic = false;

    public override void OnAwake()
    {
        IsCinematic = true;
    }

    public override void OnStart()
    {
        Co_IdleCycle = StartCoroutine(Co_DecideNextState());

        //StartCoroutine(Test());
    }

    public override void OnUpdate()
    {
        if (isBasicAttack)
        {
            bossStateMachine.SetState(GetComponent<BasicAttackState>());
            isBasicAttack = false;
        }
        if (isRoar)
        {
            bossStateMachine.SetState(GetComponent<RoarState>());
            isRoar = false;
        }
        if (isRush)
        {
            bossStateMachine.SetState(GetComponent<RushAttackState>());
            isRush = false;
        }
        if (isStrike)
        {
            bossStateMachine.SetState(GetComponent<StrikeAttackState>());
            isStrike = false;
        }
        if (isJump)
        {
            bossStateMachine.SetState(GetComponent<JumpAttackState>());
            isJump = false;
        }
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

    // ???? ?? - ??? ?? ?
    IEnumerator Test()
    {
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle State"));
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        bossStateMachine.SetState(GetComponent<RoarState>());

    }

    /// <summary> ????? ??? ??? ??? ?? ??? ?? ??? ??? ???? ?? </summary>
    /// <returns></returns>
    IEnumerator Co_DecideNextState()
    {
        yield return new WaitUntil(() => GameManager.instance.isBattle);
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle State"));
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f);

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
}