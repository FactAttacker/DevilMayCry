using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHP : MonoBehaviour
{
    #region ����

    Coroutine Co_damaged;
    Coroutine Co_RefreshHPBar;

    // ��� HP�� �̹���, �߰� �� ���� �̹���(HP�� �� �̹���), HP�� �̹���. Damage Hp�� �̹���
    [SerializeField] Image hpBar, damagedHPBar;

    [SerializeField, Range(0f, 1f), Tooltip("�� = HP ����, �� = 0(�ִϸ��̼� ���� �Ұ���) 1(�ִϸ��̼� ���� ���� -> ������ ���� 1��)")] float[] damageRates;
    [SerializeField, Range(0, 1), Tooltip("0(�ִϸ��̼� ���� �Ұ���), 1(�ִϸ��̼� ���� ����)")] int[] damageAnimPosibleCounts;

    [SerializeField, Tooltip("HP Bar UI�� fillAmount ��ȭ �ӵ�")] float refreshSpeed = 1;
    [SerializeField, Tooltip("���� HP Bar ��ȭ �� ���ð�")] float refreshWaitTime = 0.1f;
    WaitForSeconds waitRefreshSeconds;

    #endregion

    #region �Ӽ�

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

    #region ����Ƽ ���� �ֱ�

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

    #region ������

    void RefreshHPBar(float value)
    {
        if (Co_RefreshHPBar != null) StopCoroutine(Co_RefreshHPBar); 
        Co_RefreshHPBar = StartCoroutine(Co_RefreshHPBarCycle(value));
    }

    #region �ڷ�ƾ

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

    #region �׽�Ʈ��

    public void Damage(float dmg)
    {
        TakeDamage = dmg;
    }

    #endregion
}
