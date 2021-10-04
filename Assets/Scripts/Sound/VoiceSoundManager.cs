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

    void Start()
    {
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
        }
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

        if (!CaptionManager.instatnce.isPlay)
        {
            OnCaption(str);
            StartCoroutine(OffCaption(audio.clip.length + 0.5f));
        }
    }

    void OnCaption(string str)
    {
        if (GlobalState.captionDict.ContainsKey(str)
          && GlobalState.captionDict[str].KR != "")
        {
            CaptionManager.instatnce.OnText(GlobalState.captionDict[str].ID);
        }
    }

    IEnumerator OffCaption(float time)
    {
        yield return new WaitForSeconds(time);
        CaptionManager.instatnce.OffText();
    }
}
