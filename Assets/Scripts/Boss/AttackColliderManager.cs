using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliderManager : MonoBehaviour
{
    public Collider[] ColliderArr => colliderArr;
    [SerializeField] Collider[] colliderArr;

    private void Start()
    {
        for (int i = 0; i < colliderArr.Length; i++)
        {
            colliderArr[i].enabled = false;
        }
    }

    /// <summary> For Collider's SetActive On/Off </summary>
    /// <param name="colliderIdx"> colliderIdx == 0 -> Left Hand Collider On
    /// <para/> colliderIdx == 1 -> Right Hand Collider On
    /// <para/> colliderIdx == 2 -> TwoHand Collider On </param>
    public void ColliderCheckOn(int colliderIdx = 0)
    {
        colliderArr[colliderIdx].enabled = true;
    }

    public void ColliderCheckOff()
    {
        for (int i = 0; i < colliderArr.Length; i++)
        {
            colliderArr[i].enabled = false;
        }
    }
}
