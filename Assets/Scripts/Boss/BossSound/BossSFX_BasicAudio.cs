using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSFX_BasicAudio : MonoBehaviour
{
    private void Awake()
    {
        TryGetComponent(out AudioSource audio);
        VoiceSoundManager.instatnce.SetEffectSound(audio);
    }
}
