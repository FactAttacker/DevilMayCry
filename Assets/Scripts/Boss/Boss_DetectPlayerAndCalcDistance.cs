using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_DetectPlayerAndCalcDistance : MonoBehaviour
{
    public Transform playerTr;
    public float distance { get; private set; }
    Coroutine Co_searchPlayer;

    void Start()
    {
        Co_searchPlayer = StartCoroutine(SearchPlayer());
        //PlayerSystem.Instance.Player.transform = playerTr;
    }

    void Update()
    {
        if (!playerTr) return;
        distance = Vector3.Distance(playerTr.position, transform.position);
    }

    /// <summary> Player 오브젝트를 검색하는 함수 </summary>
    /// <returns> "Player" 태그를 가진 오브젝트가 나타날 때까지 대기 </returns>
    IEnumerator SearchPlayer()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player"));

        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        StopCoroutine(Co_searchPlayer);
    }
}
