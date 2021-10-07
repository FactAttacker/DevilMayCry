using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metheo : MonoBehaviour
{
    SphereCollider sphereCollider;

    [SerializeField] float delay = 2.6f;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.enabled = false;
        Invoke(nameof(MetheoDamage), delay);
    }

    private void OnCollisionEnter(Collision collision)
    {
        BossSystem.Instance.Boss_DetectPlayerAndCalcDistance.playerScript.flyingBack = true;

        BossSystem.Instance.Boss_DetectPlayerAndCalcDistance.playerTr.GetComponent<PlayerState>().TakeDamage =
            GlobalState.bossAttackList[4].Damage;
    }

    void MetheoDamage()
    {
        sphereCollider.enabled = true;
    }
}
