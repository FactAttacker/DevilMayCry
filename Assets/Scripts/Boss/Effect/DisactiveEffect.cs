using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisactiveEffect : MonoBehaviour
{
    [SerializeField] float disactiveSecond = 2.0f;
    private void OnEnable()
    {
        Invoke(nameof(Disactive), disactiveSecond);
    }

    void Disactive()
    {
        gameObject.SetActive(false);
    }
}
