using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHP : MonoBehaviour
{
    #region Variable

    Coroutine Co_damaged;
    Coroutine Co_RefreshHPBar;

    // 배경 HP바 이미지, 중간 흰 막대 이미지(HP바 끝 이미지), HP바 이미지. Damage Hp바 이미지
    [SerializeField] Image hpBar, damagedHPBar;

    [SerializeField, Range(0f, 1f), Tooltip("설정된 비율에 따라 보스의 피격 애니메이션 및 이벤트 실행")] float[] damageRates;
    [Range(0, 1), Tooltip("0(애니메이션 실행 불가능), 1(애니메이션 실행 가능)")] int[] damageAnimPosibleCounts;

    [SerializeField, Tooltip("HP Bar UI의 fillAmount 변화 속도")] float refreshSpeed = 1;
    [SerializeField, Tooltip("붉은 HP Bar 변화 전 대기시간")] float refreshWaitTime = 0.1f;
    WaitForSeconds waitRefreshSeconds;

    [SerializeField, Tooltip("damageRates 인덱스 중 궁극기가 실행될 인덱스")] int ultimateIndex;

    #endregion

    #region Property

    public float CurrHP
    {
        get => currHP;
        private set
        {
            currHP = value;
            for (int i = 0; i < damageRates.Length; i++)
            {
                if (CurrHP / MaxHP <= damageRates[i] && damageAnimPosibleCounts[i] == 1)
                {
                    if(ultimateIndex == i)
                    {
                        BossSystem.Instance.BossStateMachine.SetState(BossSystem.Instance.UltimateAttackState, true);
                    }
                    Co_damaged = StartCoroutine(Co_Damaged());
                    damageAnimPosibleCounts[i] = 0;
                    break;
                }
            }
            RefreshHPBar(value);
        }
    }
    float currHP;

    public float MaxHP
    {
        get => GlobalState.bossList[0].MaxHP;
    }

    public float TakeDamage { set => CurrHP -= CurrHP - value <= 0 ? currHP : value; }

    #endregion

    #region Unity Life Cycle

    private void Start()
    {
        waitRefreshSeconds = new WaitForSeconds(refreshWaitTime);
    }

    private void OnEnable()
    {
        damageAnimPosibleCounts = new int[damageRates.Length];
        for (int i = 0; i < damageRates.Length; i++)
        {
            damageAnimPosibleCounts[i] = 1;
        }
        CurrHP = MaxHP;
    }

    #endregion

    #region Implementation Space

    void RefreshHPBar(float value)
    {
        if (Co_RefreshHPBar != null) StopCoroutine(Co_RefreshHPBar); 
        Co_RefreshHPBar = StartCoroutine(Co_RefreshHPBarCycle(value));
    }

    #region Coroutine

    IEnumerator Co_RefreshHPBarCycle(float _value)
    {
        damagedHPBar.fillAmount = hpBar.fillAmount;
        hpBar.fillAmount = _value / MaxHP;
        yield return waitRefreshSeconds;

        float rate = 0;
        while (rate <= 1)
        {
            rate += Time.deltaTime * refreshSpeed;
            damagedHPBar.fillAmount = Mathf.Lerp(damagedHPBar.fillAmount, hpBar.fillAmount, rate);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        damagedHPBar.fillAmount = hpBar.fillAmount;
    }

    public IEnumerator Co_Damaged()
    {
        Animator anim = GetComponent<BossStateMachine>().anim;

        anim.SetTrigger("Damaged");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Damaged State"));

        while (anim.GetCurrentAnimatorStateInfo(0).IsName("Damaged State"))
        {
            yield return null;
        }

        StopCoroutine(Co_damaged);
    }

    #endregion

    #endregion
}
