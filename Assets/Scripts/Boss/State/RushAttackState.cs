using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RushAttackState : BossState
{
    Coroutine Co_rushAttackCycle;
    Coroutine Co_rush;

    float playerDetectedDistance;
    [SerializeField] Transform rayStartTr;

    public override void OnAwake()
    {
        playerDetectedDistance = GlobalState.bossList[0].PlayerDetectDistance;
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
        yield return Co_rush = StartCoroutine(Co_Rush());
        yield return StartCoroutine(Co_RushAttack());
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).IsName("Rush Attack State"));
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        bossStateMachine.SetState(GetComponent<AttackDelayState>());
        StopAllCoroutines();
    }

    [SerializeField] float rushSpeed = 10, maxRushRange = 40;
    IEnumerator Co_Rush()
    {
        BossSystem.Instance.BossAnimationEvents.OnBossVoice("Boss-Rush1");

        float rushRange = 0;
        while ( GetComponent<Boss_DetectPlayerAndCalcDistance>().distance > playerDetectedDistance
               && rushRange <= maxRushRange
               && !IsThereWallToFront())
        {
            rushRange += Time.deltaTime * rushSpeed;
            transform.position += transform.forward * Time.deltaTime * rushSpeed;
            yield return null;
        }
        bossStateMachine.anim.SetTrigger("RushAttack");
    }

    [SerializeField] float wallDetectingRayDist = 10;
    bool IsThereWallToFront()
    {
        Ray ray = new Ray(rayStartTr.position, transform.forward * wallDetectingRayDist);
        bool result = Physics.Raycast(ray, 1 << LayerMask.NameToLayer("Default"));
        print(result);
        return result;
    }

    IEnumerator Co_RushAttack()
    {
        float rate = 1;
        while (rate > 0)
        {
            transform.position += transform.forward * Time.deltaTime * rushSpeed * rate;
            rate -= Time.deltaTime;
            yield return null;
        }
    }

    public void SetRushAttackSpeed(float _attackSpeed) => bossStateMachine.anim.SetFloat("RushAttackSpeed", _attackSpeed);

    public void OnRushEffect(string _effectName)
    {
        Vector3 tempPos = BossSystem.Instance.AttackColliderManager.ColliderArr[2].transform.position;
        tempPos.y = 0;
        BossSystem.Instance.BossAnimationEvents.OnEffect(_effectName, tempPos, Quaternion.identity);
    }
}