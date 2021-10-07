using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateAttackState : BossState
{
    [SerializeField] Transform UltimateEffectTr;
    public override void OnAwake()
    {

    }

    public override void OnStart()
    {
        print("궁극기이이!");
        StartCoroutine(UltimateAttackCycle());
    }

    public override void OnEnd()
    {
        
    }

    public override void OnReset()
    {

    }

    IEnumerator UltimateAttackCycle()
    {
        GetComponent<BossRotate>().Co_RotateToPlayer();

        BossSystem.Instance.Animator.SetTrigger("UltimateAttack");
        yield return new WaitUntil(() => BossSystem.Instance.Animator.GetCurrentAnimatorStateInfo(0).IsName("Ultimate Attack State"));
        OnUltimateAttackEffect("EnergyExplosion");
        yield return new WaitForSeconds(4.8f);
        float tempDist = Mathf.Clamp(boss_DetectPlayerAndCalcDistance.distance - 30, 1f, 30f);
        float damage = tempDist <= 50 ? (GlobalState.bossAttackList[4].Damage / tempDist) : 0; // 플레이어와의 거리에 비례해서 데미지를 주는 공식을 짠다.
        boss_DetectPlayerAndCalcDistance.playerTr.GetComponent<PlayerState>().TakeDamage = damage;

        BossSystem.Instance.Boss_DetectPlayerAndCalcDistance.playerScript.flyingBack = true;
        print(tempDist);
        yield return null;
    }

    public void SetUltimateAttackSpeed(float _attackSpeed) => BossSystem.Instance.Animator.SetFloat("UltimateAttackSpeed", _attackSpeed);

    public void OnUltimateAttackEffect(string _effectName)
    {
        Vector3 tempPos = UltimateEffectTr.position;
        BossSystem.Instance.BossAnimationEvents.OnEffect(_effectName, tempPos);
    }
}
