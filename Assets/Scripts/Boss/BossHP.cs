using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHP : MonoBehaviour
{
    #region 변수

    Coroutine Co_damaged;
    Coroutine Co_RefreshHPBar;

    // 배경 HP바 이미지, 중간 흰 막대 이미지(HP바 끝 이미지), HP바 이미지. Damage Hp바 이미지
    [SerializeField] Image hpBar, damagedHPBar;

    [SerializeField, Range(0f, 1f), Tooltip("행 = HP 비율, 열 = 0(애니메이션 실행 불가능) 1(애니메이션 실행 가능 -> 세팅은 전부 1로)")] float[] damageRates;
    [SerializeField, Range(0, 1), Tooltip("0(애니메이션 실행 불가능), 1(애니메이션 실행 가능)")] int[] damageAnimPosibleCounts;

    [SerializeField, Tooltip("HP Bar UI의 fillAmount 변화 속도")] float refreshSpeed = 1;
    [SerializeField, Tooltip("붉은 HP Bar 변화 전 대기시간")] float refreshWaitTime = 0.1f;
    WaitForSeconds waitRefreshSeconds;

    #endregion

    #region 속성

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
        get => maxHP;
        private set => maxHP = value;
    }
    [SerializeField] float maxHP = 100;
    public float TakeDamage { set => CurrHP -= CurrHP - value <= 0 ? currHP : value; }

    #endregion

    #region 유니티 생명 주기

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

    #region 구현부

    void RefreshHPBar(float value)
    {
        if (Co_RefreshHPBar != null) StopCoroutine(Co_RefreshHPBar); 
        Co_RefreshHPBar = StartCoroutine(Co_RefreshHPBarCycle(value));
    }

    #region 코루틴

    IEnumerator Co_RefreshHPBarCycle(float _value)
    {
        damagedHPBar.fillAmount = hpBar.fillAmount;
        hpBar.fillAmount = _value / MaxHP;
        yield return waitRefreshSeconds;

        float rate = 0;
        while (rate < 1)
        {
            rate += Time.deltaTime;
            damagedHPBar.fillAmount = Mathf.Lerp(damagedHPBar.fillAmount, hpBar.fillAmount, rate * refreshSpeed);
            yield return null;
        }
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

    #region 테스트용

    public void Damage(float dmg)
    {
        TakeDamage = dmg;
    }

    #endregion
}
