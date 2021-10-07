using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownAttackEffectSound : MonoBehaviour
{
    public float Delay = 1;

    private float currentTime = 0;
    private bool isEnabled;

    private void Awake()
    {
        TryGetComponent(out AudioSource audio);
        VoiceSoundManager.instatnce.SetEffectSound(audio);
    }

    // Use this for initialization
    void OnEnable()
    {
        isEnabled = false;
        // Invoke("ActivateGO", Delay);
        currentTime = 0;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        if (!isEnabled && currentTime >= Delay)
        {
            isEnabled = true;

        }
    }
}
