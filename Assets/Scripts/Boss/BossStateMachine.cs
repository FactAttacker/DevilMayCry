using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine : MonoBehaviour
{
    [HideInInspector] public Animator anim;

    public BossState CurrState { get; private set; }
    BossState prevState;

    void Start()
    {
        anim = GetComponent<Animator>();
        SetState(GetComponent<IdleState>());
    }

    public void SetState(BossState _nextState, bool _isReset = true)
    {
        prevState = CurrState;
        prevState?.OnEnd();
        if(_isReset) prevState?.OnReset();
        CurrState = _nextState;
        CurrState.OnStart();
    }

    public void PrevState()
    {
        Debug.Log($"{CurrState} -> {prevState}");
        CurrState?.OnEnd();
        CurrState = prevState;
        CurrState?.OnStart();
    }

    [ContextMenu("?????? ????")]
    public void DebugProperty()
    {
        Debug.Log($"m_prevState : {prevState}");
        Debug.Log($"m_currentState : {CurrState}");
    }


    /// <summary> 카메라 흔들림을 구현하는 OnShake 함수를 애니메이션으로 제어할 수 있도록 변환한 함수 </summary>
    /// <param name="timeAndAmount"> time과 amount를 쉼표(,)를 이용하여 구분  </param>
    public void OnCameraShake(string timeAndAmount)
    {
        string[] temps = timeAndAmount.Split(',');
        CameraManager.instance.OnShake(float.Parse(temps[0]), float.Parse(temps[1]));
    }
}
