using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JumpAttackState : BossState
{
    public override void OnAwake()
    {
        
    }

    public override void OnStart()
    {
        bossStateMachine.anim.SetTrigger("Jump");
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
}
