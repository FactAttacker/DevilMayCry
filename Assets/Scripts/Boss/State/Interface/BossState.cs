using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : MonoBehaviour
{
    protected BossStateMachine bossStateMachine;
    protected Boss_DetectPlayerAndCalcDistance boss_DetectPlayerAndCalcDistance;

    #region ����Ƽ ���� �ֱ�

    protected void Awake()
    {
        bossStateMachine = GetComponent<BossStateMachine>();
        boss_DetectPlayerAndCalcDistance = GetComponent<Boss_DetectPlayerAndCalcDistance>();
        OnAwake();
    }

    protected void FixedUpdate()
    {
        if (bossStateMachine.CurrState != this) return;
        OnFixedUpdate();
    }

    protected void Update()
    {
        if (bossStateMachine.CurrState != this) return;
        OnUpdate();
    }

    #endregion

    // ���� �Լ�, �ڽĿ��� �������̵��ؼ� ���
    public virtual void OnAwake() { } // �ʱ�ȭ��

    public virtual void OnStart() { } // �ش� ���·� �������� ��

    public virtual void OnFixedUpdate() { } // ���� �ۿ� ���� �� �� �����Ӵ�

    public virtual void OnUpdate() { } // �� ������ ��

    public virtual void OnEnd() { } // ���°� ���� ��

    public virtual void OnReset() { } // üũ�� ���� �� ������ ���¿�
}
