using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHP : MonoBehaviour
{
    Coroutine Co_damaged;

    [SerializeField] Text text;

    [SerializeField, Range(0f, 1f), Tooltip("행 = HP 비율, 열 = 0(애니메이션 실행 불가능) 1(애니메이션 실행 가능 -> 세팅은 전부 1로)")] float[] damageRates;
    [Range(0, 1), Tooltip("0(애니메이션 실행 불가능), 1(애니메이션 실행 가능)")] int[] damageAnimPosibleCounts;

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

    private void OnEnable()
    {
        damageAnimPosibleCounts = new int[damageRates.Length];
        for (int i = 0; i < damageRates.Length; i++)
        {
            damageAnimPosibleCounts[i] = 1;
        }
        CurrHP = MaxHP;
    }


    public float TakeDamage { set => CurrHP -= CurrHP - value <= 0 ? currHP : value; }
    void RefreshHPBar(float value)
    {
        text.text = value.ToString();
    }

    public void Damage(float dmg)
    {
        TakeDamage = dmg;
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
}
