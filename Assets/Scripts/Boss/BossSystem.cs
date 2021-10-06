using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSystem : MonoBehaviour
{
    #region Singleton

    public static BossSystem Instance;

    void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    #endregion

    #region Variable

    #region Basic Variable

    [SerializeField] GameObject boss;
    Boss_DetectPlayerAndCalcDistance boss_DetectPlayerAndCalcDistance;
    AttackColliderManager attackColliderManager;
    BossAnimationEvents bossAnimationEvents;
    BossEffectManager bossEffectManager;
    BossStateMachine bossStateMachine;
    BossCinematic bossCinematic;
    Animator animator;
    BossHP bossHP;

    #endregion

    #region State Variable

    UltimateAttackState ultimateAttackState;
    StrikeAttackState strikeAttackState;
    BasicAttackState basicAttackState;
    AttackDelayState attackDelayState;
    RushAttackState rushAttackState;
    JumpAttackState jumpAttackState;
    IdleState idleState;
    RoarState roarState;

    #endregion

    #endregion

    #region Property

    #region Basic Property
    public GameObject Boss => boss;
    public Boss_DetectPlayerAndCalcDistance Boss_DetectPlayerAndCalcDistance => boss_DetectPlayerAndCalcDistance = boss_DetectPlayerAndCalcDistance ? boss_DetectPlayerAndCalcDistance : Boss.GetComponent<Boss_DetectPlayerAndCalcDistance>();
    public AttackColliderManager AttackColliderManager => attackColliderManager = attackColliderManager ? attackColliderManager : Boss.GetComponent<AttackColliderManager>();
    public BossAnimationEvents BossAnimationEvents => bossAnimationEvents = bossAnimationEvents ? bossAnimationEvents : Boss.GetComponent<BossAnimationEvents>();
    public BossEffectManager BossEffectManager => bossEffectManager = bossEffectManager ? bossEffectManager : FindObjectOfType< BossEffectManager>();
    public BossStateMachine BossStateMachine => bossStateMachine = bossStateMachine ? bossStateMachine : Boss.GetComponent<BossStateMachine>();
    public BossCinematic BossCinematic => bossCinematic = bossCinematic ? bossCinematic : Boss.GetComponent<BossCinematic>();
    public Animator Animator => animator = animator ? animator : Boss.GetComponent<Animator>();
    public BossHP BossHP => bossHP = bossHP ? bossHP : Boss.GetComponent<BossHP>();

    #endregion

    #region State Property

    public UltimateAttackState UltimateAttackState => ultimateAttackState = ultimateAttackState ? ultimateAttackState : Boss.GetComponent<UltimateAttackState>();
    public StrikeAttackState StrikeAttackState => strikeAttackState = strikeAttackState ? strikeAttackState : Boss.GetComponent<StrikeAttackState>();
    public BasicAttackState BasicAttackState => basicAttackState = basicAttackState ? basicAttackState : Boss.GetComponent<BasicAttackState>();
    public AttackDelayState AttackDelayState => attackDelayState = attackDelayState ? attackDelayState : Boss.GetComponent<AttackDelayState>();
    public RushAttackState RushAttackState => rushAttackState = rushAttackState ? rushAttackState : Boss.GetComponent<RushAttackState>();
    public JumpAttackState JumpAttackState => jumpAttackState = jumpAttackState ? jumpAttackState : Boss.GetComponent<JumpAttackState>();
    public IdleState IdleState => idleState = idleState ? idleState : Boss.GetComponent<IdleState>();
    public RoarState RoarState => roarState = roarState ? roarState : Boss.GetComponent<RoarState>();

    #endregion

    #endregion
}
