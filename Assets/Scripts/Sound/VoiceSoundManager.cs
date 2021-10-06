using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceSoundManager : MonoBehaviour
{
    public static VoiceSoundManager instatnce;
    private void Awake()
    {
        if (instatnce != null) return;
        instatnce = this;
        DontDestroyOnLoad(gameObject);
    }
    [Header("BGM Sound")]
    public AudioSource bgmSound;

    #region BGM Clips
    public enum BGMType
    {
        OPENING = 0,
        START = 1,
        BATTLE =2
    }
    [System.Serializable]
    class BGMItem
    {
        public BGMType type;
        public AudioClip clip;
    }
    [SerializeField]
    BGMItem[] bgmItems;
    public BGMType currentBGMType;
    #endregion

    #region Voice Sound
    public enum VoiceType
    {
        Boss,
        Player
    }
    [System.Serializable]
    class VoiceItem
    {
        public VoiceType type;
        public AudioSource audio;
        public AudioClip[] clip;
    }
    [SerializeField]
    VoiceItem[] voiceItems;
    #endregion

    float bgmVolum = 0.5f;
    public float BgmVolum
    {
        get { return bgmVolum; }
        set {
            bgmVolum = value;
            SetBGMVolum(value);
        }
    }
    float effectVolum = 0.5f;
    public float EffectVolum
    {
        get { return effectVolum; }
        set
        {
            effectVolum = value;
            SetEffectVolum(value);
        }
    }

    [System.Serializable]
    class Test
    {
        public VoiceType testType;
        public string testName;
    }
    [SerializeField]
    [ContextMenuItem("목소리 실행",nameof(TestVoice))]
    Test voiceTest;

    public Dictionary<string, AudioClip> bossVoiceDict = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> danteVoiceDict = new Dictionary<string, AudioClip>();
    //Sound Delegate
    delegate void SoundDele();
    SoundDele effectSoundFunc;
    SoundDele bgmSoundFunc;


    void Start()
    {
        // Boss & Dante Dictionay Processing
        foreach(VoiceItem item in voiceItems)
        {
            switch (item.type)
            {
                case VoiceType.Boss:
                    foreach(AudioClip clip  in item.clip)
                        bossVoiceDict.Add(clip.name, clip);
                    break;
                case VoiceType.Player:
                    foreach (AudioClip clip in item.clip)
                        danteVoiceDict.Add(clip.name, clip);
                    break;
            }
            // All Effect Sound Managemet
            SetEffectSound(item.audio);
        }
        // BGM Sound Management
        SetBGMSound(bgmSound);

        BgmVolum = BgmVolum;
        EffectVolum = EffectVolum;
    }

    public void TestVoice()
    {
        if (voiceTest.testName == "") return;
        switch (voiceTest.testType)
        {
            case VoiceType.Boss:
                OnBossVoice(voiceTest.testName);
                break;
            case VoiceType.Player:
                OnDanteVoice(voiceTest.testName);
                break;
        }
    }

    /// <summary>
    /// BGM Audio Clip Change
    /// </summary>
    /// <param name="type"></param>
    public AudioSource SetBGMChange(BGMType type)
    {
        BGMItem item = bgmItems[(int)type];
        bgmSound.clip = item.clip;
        bgmSound.Stop();
        switch (currentBGMType)
        {
            case BGMType.OPENING:
                break;
            case BGMType.START:
                break;
            case BGMType.BATTLE:
                bgmSound.time = (item.clip.length / 10f);
                break;
        }
        return bgmSound;
    }

    /// <summary>
    /// Effect Sound Add
    /// </summary>
    /// <param name="audio"></param>
    public void SetEffectSound(AudioSource audio)
    {
        audio.volume = effectVolum;
        effectSoundFunc += () =>
        {
            if(audio != null) audio.volume = effectVolum;
        };
    }
    /// <summary>
    /// BGM Sound Add
    /// </summary>
    /// <param name="audio"></param>
    public void SetBGMSound(AudioSource audio)
    {
        //audio.volume = bgmVolum;
        bgmSoundFunc += () =>
        {
            if(audio != null) audio.volume = bgmVolum;
        };
    }

    /// <summary>
    /// BGM Sound Volume Processing
    /// </summary>
    public void SetBGMVolum(float volum)
    {
        GameOption.instance.bgmSlider.value = volum;
        bgmSoundFunc();
    }

    /// <summary>
    /// Effect Sound Volume Processing
    /// </summary>
    public void SetEffectVolum(float volum)
    {
        GameOption.instance.effectSlider.value = volum;
        effectSoundFunc();
    }

    /// <summary>
    /// On Boss Voice
    /// </summary>
    /// <param name="str">Key</param>
    public void OnBossVoice(string str)
    {
        AudioSource audio = voiceItems[0].audio;
        audio.Stop();
        audio.clip = bossVoiceDict[str];
        audio.Play();

        if (!CaptionManager.instatnce.isPlay)
        {
            print(str);
            OnCaption(str);
            StartCoroutine(OffCaption(audio.clip.length + 0.5f));
        }
    }

    /// <summary>
    /// On Dante Voice
    /// </summary>
    /// <param name="str">Key</param>
    public void OnDanteVoice(string str)
    {
        AudioSource audio = voiceItems[1].audio;
        audio.Stop();
        audio.clip = danteVoiceDict[str];
        audio.Play();

        if (!CaptionManager.instatnce.isPlay &&
            GlobalState.captionDict.ContainsKey(str)
            && GlobalState.captionDict[str].KR != ""
        )
        {
            print(str);
            OnCaption(str);
            StartCoroutine(OffCaption(audio.clip.length + 0.5f));
        }
    }

    void OnCaption(string str)
    {
        CaptionManager.instatnce.OnText(GlobalState.captionDict[str].ID);
    }

    IEnumerator OffCaption(float time)
    {
        yield return new WaitForSeconds(time);
        CaptionManager.instatnce.OffText();
    }
}
