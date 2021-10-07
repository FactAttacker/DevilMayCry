using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine : MonoBehaviour
{
    public BossState CurrState { get; private set; }
    BossState prevState;

    void Start()
    {
        SetState(BossSystem.Instance.IdleState);
    }

    public void SetState(BossState _nextState, bool _isReset = true)
    {
        prevState = CurrState;
        prevState?.OnEnd();
        if(_isReset) prevState?.OnReset();
        CurrState = _nextState;
        CurrState.OnStart();
    }
}
