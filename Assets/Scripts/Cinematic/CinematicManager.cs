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
            BreakBuiling,
            Boos_2
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

    [SerializeField]
    Camera[] cameras;

    Dictionary<SoundMg.Type, AudioClip> soundDict = new Dictionary<SoundMg.Type, AudioClip>();
    Scenemachine_Player player;

    public BossCinematic bossCine;
    public bool isWallBreak = false;

    public GameObject title;
    public GameObject wall;

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
        title.SetActive(false);
        player = Scenemachine_Player.instance;
        if (VoiceSoundManager.instatnce != null) VoiceSoundManager.instatnce.SetEffectSound(soundMg._audio);
        OnScenario();
    }

    void OnScenario()
    {
        StartCoroutine(CoScenario());
    }
    IEnumerator CoScenario()
    {
        // 단테 걷는 소리
        soundMg._audio.clip = soundDict[SoundMg.Type.Walking];
        soundMg._audio.Play();

        Invoke(nameof(OnInputSword), 4f);

        //단테 걷는 이벤트
        bool isWalk = true;
        while (isWalk)
        {
            if (player.transform.position != player.destination.position)
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, player.destination.position, player.walkSpeed * Time.deltaTime);
            }
            if (Vector3.Distance(player.transform.position, player.destination.position) < 0.1)
            {
                player.transform.position = player.destination.position;
                OnTurn();
                isWalk = false;
            }
            yield return null;
        }
        yield return new WaitUntil(() => !isWalk);

        player.katana.transform.SetParent(player.swordCase.transform, false);
        player.katana.transform.localPosition = new Vector3(0.01705508f, -0.4062368f, -0.143f);
        player.katana.transform.localEulerAngles = new Vector3(0f, -180f, 20f);
        player.anim.SetBool("AloneLeg", false);

        yield return new WaitForSeconds(1f);
        bossCine.story[0].isNext = true;
        isWallBreak = true;
        yield return new WaitForSeconds(2f);
        cameras[1].gameObject.SetActive(false);
        cameras[2].gameObject.SetActive(true);
        bossCine.story[1].isNext = true;
        yield return new WaitForSeconds(2f);
        soundMg._audio.clip = soundDict[SoundMg.Type.Boos_2];
        soundMg._audio.Play();

        yield return new WaitForSeconds(2f);
        title.SetActive(true);
        yield return new WaitForSeconds(3f);
        title.SetActive(false);
        VoiceSoundManager.instatnce.OnBossVoice("Boss-Mother");
        
        yield return new WaitForSeconds(VoiceSoundManager.instatnce.bossVoiceDict["Boss-Mother"].length + 0.05f);

        yield return new WaitForSeconds(1f);
        cameras[2].gameObject.SetActive(false);
        cameras[3].gameObject.SetActive(true);
        wall.SetActive(false);
        VoiceSoundManager.instatnce.OnDanteVoice("Dante-um");
        yield return new WaitForSeconds(VoiceSoundManager.instatnce.danteVoiceDict["Dante-um"].length + 0.5f);
        VoiceSoundManager.instatnce.OnDanteVoice("Dante-WhatYouLack");
        yield return new WaitForSeconds(VoiceSoundManager.instatnce.danteVoiceDict["Dante-WhatYouLack"].length + 0.5f);
        VoiceSoundManager.instatnce.OnDanteVoice("Dante-WannaHaveSomeFun");
        yield return new WaitForSeconds(VoiceSoundManager.instatnce.danteVoiceDict["Dante-WannaHaveSomeFun"].length + 0.5f);
        cameras[3].gameObject.SetActive(false);
        cameras[4].gameObject.SetActive(true);
        VoiceSoundManager.instatnce.OnBossVoice("Boss-Fuck");
        yield return new WaitForSeconds(VoiceSoundManager.instatnce.bossVoiceDict["Boss-Fuck"].length + 0.5f);
        VoiceSoundManager.instatnce.OnBossVoice("Boss-WhatTheHell");
        yield return new WaitForSeconds(VoiceSoundManager.instatnce.bossVoiceDict["Boss-WhatTheHell"].length + 0.5f);
        cameras[4].gameObject.SetActive(false);
        cameras[5].gameObject.SetActive(true);

        bossCine.story[2].isNext = true;
        yield return new WaitForSeconds(0.7f);
        FadeInOutController.instance.OnFadeInOut(2);

    }
    void OnTurn()
    {
        //걷기 멈춤
        player.anim.SetTrigger("Stop");
        //걷는 사운드 종료
        soundMg._audio.Stop();
        StartCoroutine(CoStopTime());
    }
    IEnumerator CoStopTime()
    {
        //Boss Sound
        soundMg._audio.clip = soundDict[SoundMg.Type.BreakBuiling];
        soundMg._audio.time = soundMg._audio.clip.length / 3.5f;
        soundMg._audio.Play();
        
        yield return new WaitForSeconds(2f);

        cameras[0].gameObject.SetActive(false);
        cameras[1].gameObject.SetActive(true);

        player.anim.SetTrigger("Turn");
        float time = 0;
        while (time < 2f)
        {
            time += Time.deltaTime;
            float lerpAngle = Mathf.LerpAngle(player.transform.eulerAngles.y, player.wantedRot_value, player.rot_Speed * Time.deltaTime);
            //transform.Rotate(0f, angle, 0.0f);
            //transform.eulerAngles = new Vector3(0, rotationy, 0);
            player.transform.eulerAngles = new Vector3(0, lerpAngle, 0);
            yield return null;
        }
    }

    public void OnInputSword()
    {
        player.inputSword = false;
        player.anim.SetTrigger("walkWithinput");
        player.AloneLeg(true);
        StartCoroutine(CoInputSword());
    }
    IEnumerator CoInputSword()
    {
        yield return null;
        player.katana.transform.SetParent(player.swordCase.transform, false);
        player.katana.transform.localPosition = new Vector3(0.01705508f, -0.4062368f, -0.143f);
        player.katana.transform.localEulerAngles = new Vector3(0f, -180f, 20f);
        //anim.SetBool("inputSword", true);

        player.anim.SetBool("AloneLeg", false);
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
