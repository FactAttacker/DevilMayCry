using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CinematicManager : MonoBehaviour
{
    public static CinematicManager instance;

    public delegate void WallBreakDel();
    public WallBreakDel wallBreakFunc;

    [System.Serializable]
    public class SoundMg
    {
        public AudioSource _audio;
        public enum Type
        {
            Walking,
            BreakBuiling
        }
        [System.Serializable]
        public class SoundItem
        {
            public Type type;
            public AudioClip clip;
        }
        public SoundItem[] soundItems;
    }
    [SerializeField]
    public SoundMg soundMg;

    Dictionary<SoundMg.Type, AudioClip> soundDict = new Dictionary<SoundMg.Type, AudioClip>();

    private void Awake()
    {
        if (instance == null) instance = this;
        foreach(SoundMg.SoundItem item in soundMg.soundItems)
        {
            soundDict.Add(item.type, item.clip);
        }
    }
    void Start()
    {
        if(VoiceSoundManager.instatnce != null) VoiceSoundManager.instatnce.SetEffectSound(soundMg._audio);
        soundMg._audio.clip = soundDict[SoundMg.Type.Walking];
        soundMg._audio.Play();
    }

    public void SetCinematicSound(SoundMg.Type type)
    {
        if(soundMg._audio.isPlaying) soundMg._audio.Stop();
        soundMg._audio.clip = soundDict[type];
        soundMg._audio.Play();
    }

    public void StopSound()
    {
        soundMg._audio.Stop();
    }

    public void OnWallBreak()
    {
        wallBreakFunc();
    }

    void Update()
    {

    }
}
