using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSystem : MonoBehaviour
{
    public static BossSystem Instance;

    void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }
        else
        {
            //Destroy(this.gameObject);
        }
    }

    [SerializeField] GameObject boss;
    Animator animator;
    BossHP bossHP;
    BossStateMachine bossStateMachine;
    AttackColliderManager attackColliderManager;
    Boss_DetectPlayerAndCalcDistance boss_DetectPlayerAndCalcDistance;
    BossEffectManager bossEffectManager;
    BossAnimationEvents bossAnimationEvents;
    BossCinematic bossCinematic;

    public GameObject Boss => boss;
    public Animator Animator => animator = animator ? animator : Boss.GetComponent<Animator>();
    public BossHP BossHP => bossHP = bossHP ? bossHP : Boss.GetComponent<BossHP>();
    public BossStateMachine BossStateMachine => bossStateMachine = bossStateMachine ? bossStateMachine : Boss.GetComponent<BossStateMachine>();
    public AttackColliderManager AttackColliderManager => attackColliderManager = attackColliderManager ? attackColliderManager : Boss.GetComponent<AttackColliderManager>();
    public Boss_DetectPlayerAndCalcDistance Boss_DetectPlayerAndCalcDistance => boss_DetectPlayerAndCalcDistance = boss_DetectPlayerAndCalcDistance ? boss_DetectPlayerAndCalcDistance : Boss.GetComponent<Boss_DetectPlayerAndCalcDistance>();
    public BossEffectManager BossEffectManager => bossEffectManager = bossEffectManager ? bossEffectManager : FindObjectOfType< BossEffectManager>();
    public BossAnimationEvents BossAnimationEvents => bossAnimationEvents = bossAnimationEvents ? bossAnimationEvents : Boss.GetComponent<BossAnimationEvents>();
    public BossCinematic BossCinematic => bossCinematic = bossCinematic ? bossCinematic : Boss.GetComponent<BossCinematic>();

}
