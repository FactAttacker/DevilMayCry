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
        currState = _nextState;
        prevState?.OnEnd();
        if(_isReset) prevState?.OnReset();
        currState.OnStart();
    }

    public void PrevState()
    {
        Debug.Log($"{currState} -> {prevState}");
        currState?.OnEnd();
        currState = prevState;
        currState?.OnStart();
    }

    [ContextMenu("자세한 정보")]
    public void DebugProperty()
    {
        Debug.Log($"m_prevState : {prevState}");
        Debug.Log($"m_currentState : {currState}");
    }
}
