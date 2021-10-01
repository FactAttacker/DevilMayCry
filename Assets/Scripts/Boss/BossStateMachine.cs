using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine : MonoBehaviour
{
    [HideInInspector] public Animator anim;

    public BossState currState { get; private set; }
    BossState prevState;

    void Start()
    {
        anim = GetComponent<Animator>();
        SetState(GetComponent<IdleState>());
    }

    public void SetState(BossState _nextState, bool _isReset = true)
    {
        prevState = currState;
        prevState?.OnEnd();
        if(_isReset) prevState?.OnReset();
        currState = _nextState;
        currState.OnStart();
    }

    public void PrevState()
    {
        Debug.Log($"{currState} -> {prevState}");
        currState?.OnEnd();
        currState = prevState;
        currState?.OnStart();
    }

    [ContextMenu("�ڼ��� ����")]
    public void DebugProperty()
    {
        Debug.Log($"m_prevState : {prevState}");
        Debug.Log($"m_currentState : {currState}");
    }


    /// <summary> ī�޶� ��鸲�� �����ϴ� OnShake �Լ��� �ִϸ��̼����� ������ �� �ֵ��� ��ȯ�� �Լ� </summary>
    /// <param name="timeAndAmount"> time�� amount�� ��ǥ(,)�� �̿��Ͽ� ����  </param>
    public void OnCameraShake(string timeAndAmount)
    {
        string[] temps = timeAndAmount.Split(',');
        CameraManager.instance.OnShake(float.Parse(temps[0]), float.Parse(temps[1]));
    }
}
