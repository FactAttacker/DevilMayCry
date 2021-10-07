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
        print("�ñر�����!");
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

        CameraManager.instance.currentTarget = CameraManager.TargetType.PLAYER;
        BossSystem.Instance.Animator.SetTrigger("UltimateAttack");
        yield return new WaitUntil(() => BossSystem.Instance.Animator.GetCurrentAnimatorStateInfo(0).IsName("Ultimate Attack State"));
        OnUltimateAttackEffect("EnergyExplosion");
        yield return new WaitForSeconds(4.8f);
        float tempDist = Mathf.Clamp(boss_DetectPlayerAndCalcDistance.distance - 30, 1f, 30f);
        float damage = tempDist <= 50 ? (GlobalState.bossAttackList[4].Damage / tempDist) : 0; // �÷��̾���� �Ÿ��� ����ؼ� �������� �ִ� ������ §��.
        boss_DetectPlayerAndCalcDistance.playerTr.GetComponent<PlayerState>().TakeDamage = damage;
        CameraManager.instance.currentTarget = CameraManager.TargetType.BOSS;
        BossSystem.Instance.Boss_DetectPlayerAndCalcDistance.playerScript.flyingBack = true;
        BossSystem.Instance.BossStateMachine.SetState(BossSystem.Instance.IdleState);
        yield return null;
    }

    public void SetUltimateAttackSpeed(float _attackSpeed) => BossSystem.Instance.Animator.SetFloat("UltimateAttackSpeed", _attackSpeed);

    public void OnUltimateAttackEffect(string _effectName)
    {
        Vector3 tempPos = UltimateEffectTr.position;
        BossSystem.Instance.BossAnimationEvents.OnEffect(_effectName, tempPos);
    }
}
