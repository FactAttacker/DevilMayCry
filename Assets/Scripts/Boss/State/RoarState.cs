using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoarState : BossState
{
    Coroutine Co_roarCycle;
    bool isNotKnockback = true;

    public override void OnAwake()
    {
        
    }

    public override void OnStart()
    {
        Co_roarCycle = StartCoroutine(Co_RoarCycle());
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

    IEnumerator Co_RoarCycle()
    {
        if(GameManager.instance.isBattle)
        {
            VoiceSoundManager.instatnce.OnBossVoice("Boss-2");
        }
        yield return new WaitForSeconds(1.8f);
        BossSystem.Instance.Animator.SetTrigger("Roar");
        yield return new WaitUntil(() => BossSystem.Instance.Animator.GetCurrentAnimatorStateInfo(0).IsName("Roar State"));
        yield return new WaitUntil(() => BossSystem.Instance.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        bossStateMachine.SetState(BossSystem.Instance.AttackDelayState);
        StopCoroutine(Co_roarCycle);
    }

    public void Roar_Knockback()
    {
        if(!isNotKnockback)
        {
            isNotKnockback = false;
            return;
        }
        else
        {
            boss_DetectPlayerAndCalcDistance.playerTr.GetComponent<Player>().flyingBack = true;
        }
            
    }
}
