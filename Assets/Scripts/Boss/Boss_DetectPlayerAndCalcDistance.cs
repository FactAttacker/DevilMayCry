using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_DetectPlayerAndCalcDistance : MonoBehaviour
{
    public Transform playerTr;
    public Player playerScript { get; private set; }
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

    /// <summary> Player ????? ???? ?? </summary>
    /// <returns> "Player" ??? ?? ????? ??? ??? ?? </returns>
    IEnumerator SearchPlayer()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player"));

        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = BossSystem.Instance.Boss_DetectPlayerAndCalcDistance.playerTr.GetComponent<Player>();
        StopCoroutine(Co_searchPlayer);
    }
}
