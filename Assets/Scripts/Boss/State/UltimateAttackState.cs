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
        print("±√±ÿ±‚¿Ã¿Ã!");
        StartCoroutine(UltimateAttackCycle());
    }

    public override void OnEnd()
    {
        StopAllCoroutines();
    }

    public override void OnReset()
    {

    }

    IEnumerator UltimateAttackCycle()
    {
        GetComponent<BossRotate>().Co_RotateToPlayer();

        BossSystem.Instance.Animator.SetTrigger("UltimateAttack");
        yield return new WaitUntil(() => BossSystem.Instance.Animator.GetCurrentAnimatorStateInfo(0).IsName("Ultimate Attack State"));

        yield return null;
    }

    public void SetUltimateAttackSpeed(float _attackSpeed) => BossSystem.Instance.Animator.SetFloat("UltimateAttackSpeed", _attackSpeed);

    public void OnUltimateAttackEffect(string _effectName)
    {
        Vector3 tempPos = UltimateEffectTr.position;
        BossSystem.Instance.BossAnimationEvents.OnEffect(_effectName, tempPos);
    }
}
