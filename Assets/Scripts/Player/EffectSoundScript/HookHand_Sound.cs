using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookHand_Sound : MonoBehaviour
{
    private void Awake()
    {
        TryGetComponent(out AudioSource audio);
        VoiceSoundManager.instatnce.SetEffectSound(audio);
    }
}
