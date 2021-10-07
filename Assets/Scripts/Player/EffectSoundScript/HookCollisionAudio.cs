using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCollisionAudio : MonoBehaviour
{
    private void Awake()
    {
        TryGetComponent(out AudioSource audio);
        VoiceSoundManager.instatnce.SetEffectSound(audio);
    }
}
