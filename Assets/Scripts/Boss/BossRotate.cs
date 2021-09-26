using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRotate : MonoBehaviour
{
    Boss_DetectPlayerAndCalcDistance boss_DetectPlayerAndCalcDistance;

    [SerializeField] float maxRateValue = 0.3f, maxRushRateValue = 1.0f;
    void Start()
    {
        boss_DetectPlayerAndCalcDistance = GetComponent<Boss_DetectPlayerAndCalcDistance>();
    }

    public IEnumerator Co_RotateToPlayer()
    {
        yield return new WaitUntil(() => boss_DetectPlayerAndCalcDistance.playerTr != null);
        Vector3 targetTr = boss_DetectPlayerAndCalcDistance.playerTr.position;
        float rate = 0;

        while(rate < maxRateValue)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetTr, transform.up), rate);
            rate += Time.deltaTime/40;
            yield return null;
        }
    }
    public void Rotate() => StartCoroutine(Co_RotateToPlayer());

    public IEnumerator Co_RushRotate()
    {
        yield return new WaitUntil(() => boss_DetectPlayerAndCalcDistance.playerTr != null);
        Vector3 targetTr = boss_DetectPlayerAndCalcDistance.playerTr.position;
        float rate = 0;

        // ���� ������ üũ�� �� RotateDir ������ ���� ȸ�� �ִϸ��̼��� �߰�

        while (rate < maxRushRateValue)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetTr, transform.up), rate);
            rate += Time.deltaTime;
            yield return null;
        }
    }
}
