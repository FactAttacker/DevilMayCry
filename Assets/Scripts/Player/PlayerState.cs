using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;



public class PlayerState : MonoBehaviour

{
    public static PlayerState instatnce;
    public bool die = false;

    #region 변수



    Coroutine Co_RefreshHPBar;



    // 배경 HP바 이미지, 중간 흰 막대 이미지(HP바 끝 이미지), HP바 이미지. Damage Hp바 이미지

    [SerializeField] Slider hpBar, damagedHPBar;


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

            if (currHP <= 0)
            {
                die = true;
            }
            RefreshHPBar(value);
            print(currHP +"Hp");
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

        damagedHPBar.value = hpBar.value;

        hpBar.value = _value / MaxHP;

        yield return waitRefreshSeconds;



        float rate = 0;

        while (rate < 1)

        {

            rate += Time.deltaTime * refreshSpeed;

            damagedHPBar.value = Mathf.Lerp(damagedHPBar.value, hpBar.value, rate);

            yield return null;

        }

    }



    #endregion



    #endregion



    #region 테스트용



    //TakeDamage = dmg;



    #endregion

}




