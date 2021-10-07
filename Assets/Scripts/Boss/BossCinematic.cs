using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCinematic : MonoBehaviour
{
    [System.Serializable]
    public class Story
    {
        public enum Type
        {
            NONE,
            JUMP,
            ROAR
        }
        public Type type;
        public Transform tr;
        public bool isNext;
    }
    public Story[] story;

    private void Start()
    {
        StartCoroutine(Co_BossCinematic());
    }

    void OnJump(Transform tr)
    {
        TryGetComponent(out JumpAttackState jump);
        if(tr != null ) jump.targetPos = tr;
        BossSystem.Instance.BossStateMachine.SetState(jump);
    }

    void OnRoar()
    {
        BossSystem.Instance.BossStateMachine.SetState(GetComponent<RoarState>());
    }

    public IEnumerator Co_BossCinematic()
    {
        yield return new WaitUntil( () => story.Length != 0);
        foreach(Story temp in story)
        {
            yield return new WaitUntil(() => temp.isNext);
            switch (temp.type)
            {
                case Story.Type.JUMP:
                    OnJump(temp.tr);
                    break;
                case Story.Type.ROAR:
                    OnRoar();
                    break;
            }
        }
    }
}
