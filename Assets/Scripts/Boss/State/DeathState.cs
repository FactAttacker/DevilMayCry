using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : BossState
{
    public override void OnStart()
    {
        Collider[] cols = GetComponentsInChildren<Collider>();
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].enabled = false;
        }
        BossSystem.Instance.Animator.SetTrigger("Death");
        Invoke("Death", 5);
    }

    void Death()
    {
        
    }
}
