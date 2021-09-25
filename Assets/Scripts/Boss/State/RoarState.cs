using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoarState : BossState
{
    Coroutine Co_roarCycle;

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
        bossStateMachine.anim.SetTrigger("Roar");
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).IsName("Roar State"));
        yield return new WaitUntil(() => bossStateMachine.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        bossStateMachine.SetState(GetComponent<AttackDelayState>());
        StopCoroutine(Co_roarCycle);
    }


    IEnumerator Co_Roar()
    {
        //³Ë¹é Ã³¸®
        print("Knockback!!");
        yield return null;
    }
    public void Roar_Knockback() => StartCoroutine(Co_Roar());
}
