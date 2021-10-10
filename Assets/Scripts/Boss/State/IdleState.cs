using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class IdleState : BossState
{
    Coroutine Co_IdleCycle;

    [SerializeField] float farDist = 10;
    [SerializeField] int roarWeight = 100, attackWeight = 100, rushWeight = 100, strikeWeight = 100, jumpWeight = 100;

    public bool isBasicAttack = false, isRush = false, isStrike = false, isRoar = false, isJump = false, isDeath = false;

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
            bossStateMachine.SetState(BossSystem.Instance.BasicAttackState);
            isBasicAttack = false;
        }
        if (isRoar)
        {
            bossStateMachine.SetState(BossSystem.Instance.RoarState);
            isRoar = false;
        }
        if (isRush)
        {
            bossStateMachine.SetState(BossSystem.Instance.RushAttackState);
            isRush = false;
        }
        if (isStrike)
        {
            bossStateMachine.SetState(BossSystem.Instance.StrikeAttackState);
            isStrike = false;
        }
        if (isJump)
        {
            bossStateMachine.SetState(BossSystem.Instance.JumpAttackState);
            isJump = false;
        }
        if (isDeath)
        {
            bossStateMachine.SetState(GetComponent<DeathState>());
            isDeath = false;
        }
        
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnEnd()
    {
        StopAllCoroutines();
    }

    public override void OnReset()
    {

    }

    // ???? ?? - ??? ?? ?
    IEnumerator Test()
    {
        yield return new WaitUntil(() => BossSystem.Instance.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle State"));
        yield return new WaitUntil(() => BossSystem.Instance.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        bossStateMachine.SetState(GetComponent<RoarState>());

    }

    /// <summary> ????? ??? ??? ??? ?? ??? ?? ??? ??? ???? ?? </summary>
    /// <returns></returns>
    IEnumerator Co_DecideNextState()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Battle-CameraProcessing");

        if (GameManager.instance != null)
        {
            yield return new WaitUntil(() => GameManager.instance.isBattle);
        }
        yield return new WaitUntil(() => BossSystem.Instance.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle State"));
        yield return new WaitUntil(() => BossSystem.Instance.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f);

        if (boss_DetectPlayerAndCalcDistance.distance >= farDist)
        {
            int weightAmount = rushWeight + jumpWeight;
            int randomWeight = Random.Range(0, weightAmount + 1);
            BossState _nextState;

            if (randomWeight <= rushWeight)
                _nextState = BossSystem.Instance.RushAttackState;
            else
                _nextState = BossSystem.Instance.JumpAttackState;
            bossStateMachine.SetState(_nextState);
        }
        else
        {
            int weightAmount = roarWeight + attackWeight + strikeWeight;
            int randomWeight = Random.Range(0, weightAmount + 1);
            BossState _nextState;

            if (randomWeight <= roarWeight)
                _nextState = BossSystem.Instance.RoarState;
            else if (randomWeight > roarWeight && randomWeight <= roarWeight + attackWeight)
                _nextState = BossSystem.Instance.BasicAttackState;
            else
                _nextState = BossSystem.Instance.StrikeAttackState;
            bossStateMachine.SetState(_nextState);
        }
        StopCoroutine(Co_IdleCycle);
    }
}