using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHP : MonoBehaviour
{
    public float CurrHP
    {
        get => currHP;
        private set
        {
            currHP = value;
        }
    }
    float currHP;

    public float MaxHP
    {
        get => maxHP;
        private set => maxHP = value;
    }
    float maxHP;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
