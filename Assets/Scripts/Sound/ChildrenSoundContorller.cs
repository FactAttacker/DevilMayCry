using UnityEngine;
using System.Collections;

public class ChildrenSoundContorller : MonoBehaviour
{
    void Start()
    {
        AudioSource[] childrenSound = GetComponentsInChildren<AudioSource>();
        foreach (AudioSource _sound in childrenSound)
        {
            VoiceSoundManager.instatnce.SetEffectSound(_sound);
        }

        //RFX4_AudioCurves[] libSoundList = GetComponentsInChildren<RFX4_AudioCurves>();
        //foreach (RFX4_AudioCurves lib in libSoundList)
        //{
        //    VoiceSoundManager.instatnce.SetEffectSound(lib.audioSource);
        //}

    }
}
