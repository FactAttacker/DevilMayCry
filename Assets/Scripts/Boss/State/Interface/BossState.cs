using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : MonoBehaviour
{
    protected BossStateMachine bossStateMachine;
    protected Boss_DetectPlayerAndCalcDistance boss_DetectPlayerAndCalcDistance;

    #region 유니티 생명 주기

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

    // 가상 함수, 자식에서 오버라이드해서 사용
    public virtual void OnAwake() { } // 초기화용

    public virtual void OnStart() { } // 해당 상태로 진입했을 때

    public virtual void OnFixedUpdate() { } // 물리 작용 원할 때 매 프레임당

    public virtual void OnUpdate() { } // 매 프레임 당

    public virtual void OnEnd() { } // 상태가 끝날 때

    public virtual void OnReset() { } // 체크를 했을 때 데이터 리셋용
}
