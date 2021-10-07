using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;



public class PlayerState : MonoBehaviour

{
    public static PlayerState instatnce;
    public bool die = false;

    #region ����



    Coroutine Co_RefreshHPBar;



    // ��� HP�� �̹���, �߰� �� ���� �̹���(HP�� �� �̹���), HP�� �̹���. Damage Hp�� �̹���

    [SerializeField] Slider hpBar, damagedHPBar;


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



    #region ����Ƽ ���� �ֱ�



    private void Start()

    {

        waitRefreshSeconds = new WaitForSeconds(refreshWaitTime);

    }



    private void OnEnable()

    {

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



    #region �׽�Ʈ��



    //TakeDamage = dmg;



    #endregion

}




