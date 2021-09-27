using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRotate : MonoBehaviour
{
    Boss_DetectPlayerAndCalcDistance boss_DetectPlayerAndCalcDistance;

    void Start()
    {
        boss_DetectPlayerAndCalcDistance = GetComponent<Boss_DetectPlayerAndCalcDistance>();
    }

    public IEnumerator Co_RotateToPlayer()
    {
        yield return new WaitUntil(() => boss_DetectPlayerAndCalcDistance.playerTr != null);
        Vector3 targetTr = boss_DetectPlayerAndCalcDistance.playerTr.position;
        float rate = 0;

        while (rate < 1)
        {
            rate += Time.deltaTime;
            Quaternion forwardDir = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetTr - transform.position, transform.up), rate);
            forwardDir.x = 0;
            forwardDir.z = 0;
            transform.rotation = forwardDir;
            yield return null;
        }
    }
    public void Rotate() => StartCoroutine(Co_RotateToPlayer());

    [SerializeField] float rotTime = 0.6f;

    public IEnumerator Co_RushRotate()
    {
        yield return new WaitUntil(() => boss_DetectPlayerAndCalcDistance.playerTr != null);
        float rate = 0, time = 0;

        // ���� ������ üũ�� �� RotateDir ������ ���� ȸ�� �ִϸ��̼��� �߰�

        while (time <= rotTime)
        {
            time += Time.deltaTime;
            Quaternion forwardDir = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(boss_DetectPlayerAndCalcDistance.playerTr.position - transform.position, transform.up), rate += Time.deltaTime / 12);
            forwardDir.x = 0;
            forwardDir.z = 0;
            transform.rotation = forwardDir;
            yield return null;
        }
    }
}