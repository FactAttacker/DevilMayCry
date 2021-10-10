using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHP : MonoBehaviour
{
    #region Variable

    Coroutine Co_damaged;
    Coroutine Co_RefreshHPBar;

    // ???? HP?? ??????, ???? ?? ???? ??????(HP?? ?? ??????), HP?? ??????. Damage Hp?? ??????
    [SerializeField] Image hpBar, damagedHPBar;

    [SerializeField, Range(0f, 1f), Tooltip("?????? ?????? ???? ?????? ???? ?????????? ?? ?????? ????")] float[] damageRates;
    [Range(0, 1), Tooltip("0(?????????? ???? ??????), 1(?????????? ???? ????)")] int[] damageAnimPosibleCounts;

    [SerializeField, Tooltip("HP Bar UI?? fillAmount ???? ????")] float refreshSpeed = 1;
    [SerializeField, Tooltip("???? HP Bar ???? ?? ????????")] float refreshWaitTime = 0.1f;
    WaitForSeconds waitRefreshSeconds;

    [SerializeField, Tooltip("damageRates ?????? ?? ???????? ?????? ??????")] int ultimateIndex;

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
                    damageAnimPosibleCounts[i] = 0;

                    if (i == ultimateIndex)
                    {
                        BossSystem.Instance.BossStateMachine.SetState(BossSystem.Instance.UltimateAttackState, true);
                        break;
                    }
                    Co_damaged = StartCoroutine(Co_Damaged());
                    break;
                }
            }

            if (hpBar == null || damagedHPBar == null) return;
            RefreshHPBar(value);
            if (GameManager.instance.isBattle && value <= 0)
            {
                CameraManager.instance.currentTarget = CameraManager.TargetType.PLAYER;
                CameraManager.instance.currentCamera = CameraManager.CameraType.ZOOM_IN;
                CameraManager.instance.EndGame();
                return;
            }
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
        Animator anim = BossSystem.Instance.Animator;

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
