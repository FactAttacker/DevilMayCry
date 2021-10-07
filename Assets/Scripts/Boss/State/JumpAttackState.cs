using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JumpAttackState : BossState
{
    Coroutine Co_jumpAttackCycle;
    public bool isJumpAtk = false;
    Vector3 vezierVector0, vezierVector1, vezierVector2, vezierVector3;
    [SerializeField] float jumpHeight;
    public Transform targetPos;
    [SerializeField] Transform effectTr;
    public override void OnAwake()
    {

    }

    public override void OnStart()
    {
        Co_jumpAttackCycle = StartCoroutine(Co_JumpAttackCycle());
        vezierVector0 = transform.position;
        vezierVector1 = new Vector3(transform.position.x, jumpHeight, transform.position.z);
        vezierVector2 = new Vector3(targetPos.position.x, jumpHeight, targetPos.position.z);
        vezierVector3 = targetPos.position;
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
        BossSystem.Instance.Animator.SetTrigger("Jump");
        yield return new WaitUntil(() => BossSystem.Instance.Animator.GetCurrentAnimatorStateInfo(0).IsName("Jump State"));
        yield return StartCoroutine(Co_Jump());
        yield return new WaitUntil(() => BossSystem.Instance.Animator.GetCurrentAnimatorStateInfo(0).IsName("Jump Strike Attack State"));
        yield return new WaitUntil(() => BossSystem.Instance.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        bossStateMachine.SetState(BossSystem.Instance.AttackDelayState);
        StopCoroutine(Co_jumpAttackCycle);
    }

    IEnumerator Co_Jump()
    {
        // ?????? ?????? ?????? ???? ????
        // ?????? ?????? ?????? ???? ??, isJumpAtk?? true?? ????
        float rate = 0;
        while(rate < 0.6f)
        {
            rate += Time.deltaTime/2.5f;
            transform.position = VezierCurve(rate);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.zero - new Vector3(transform.position.x, 0, transform.position.z)), rate * 2);
            yield return null;
        }
        BossSystem.Instance.Animator.SetTrigger("StrikeAttack");
        while (rate < 1f)
        {
            rate += Time.deltaTime/2.5f;
            transform.position = VezierCurve(rate);
            yield return null;
        }
        transform.position = targetPos.position;
        yield return null;
    }

    IEnumerator Co_JumpAttack1()
    {
        BossSystem.Instance.Animator.SetTrigger("Jump");
        yield return new WaitUntil(() => BossSystem.Instance.Animator.GetCurrentAnimatorStateInfo(0).IsName("Jump State"));
        yield return new WaitForSeconds(0.5f);
        isJumpAtk = true;
        yield return StartCoroutine(Co_Jump());
        yield return StartCoroutine(Co_JumpAttack2());
    }

    IEnumerator Co_JumpAttack2()
    {
        BossSystem.Instance.Animator.SetTrigger("StrikeAttack");
        yield return new WaitUntil(() => BossSystem.Instance.Animator.GetCurrentAnimatorStateInfo(0).IsName("Jump Strike Attack State"));
        yield return new WaitUntil(() => BossSystem.Instance.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        bossStateMachine.SetState(BossSystem.Instance.AttackDelayState);
        StopCoroutine(Co_jumpAttackCycle);
    }

    public void SetJumpAttackSpeed(float _attackSpeed) => BossSystem.Instance.Animator.SetFloat("JumpAttackSpeed", _attackSpeed);

    public void OnJumpAttackEffect(string _effectName)
    {
        Vector3 tempPos = effectTr.position;
        tempPos.y = 0;
        BossSystem.Instance.BossAnimationEvents.OnEffect(_effectName, tempPos, Quaternion.identity);
    }

    Vector3 VezierCurve(float rate)
    {
        //Vector3[] resultVec;                                 // Return Vector
        Vector3 vezierOrigin1, vezierOrigin2, vezierOrigin3;
        Vector3 vezierAlpha1, vezierAlpha2;
        Vector3 vezierBeta;

            vezierOrigin1 = Vector3.Lerp(vezierVector0, vezierVector1, rate);
            vezierOrigin2 = Vector3.Lerp(vezierVector1, vezierVector2, rate);
            vezierOrigin3 = Vector3.Lerp(vezierVector2, vezierVector3, rate);
            vezierAlpha1 = Vector3.Lerp(vezierOrigin1, vezierOrigin2, rate);
            vezierAlpha2 = Vector3.Lerp(vezierOrigin2, vezierOrigin3, rate);
            vezierBeta = Vector3.Lerp(vezierAlpha1, vezierAlpha2, rate);


        return vezierBeta;
    }

    public void OnV()
    {
        print("?? ???? ??????");
    }
}