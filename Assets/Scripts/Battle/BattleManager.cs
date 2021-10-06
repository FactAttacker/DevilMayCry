using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    void Start()
    {
        GameManager.instance.isBattle = false;
        AudioSource audio = VoiceSoundManager.instatnce.SetBGMChange(VoiceSoundManager.BGMType.BATTLE);
        audio.Play();
        audio.time = (audio.clip.length / 50f);
        StartCoroutine(CoStartStroy());
    }
    IEnumerator CoStartStroy()
    {
        VoiceSoundManager.instatnce.OnBossVoice("Boss-2");
        BossSystem.Instance.Boss.TryGetComponent(out IdleState idleBossState);
        //GameObject.FindGameObjectWithTag("Boss").TryGetComponent(out IdleState idleBossState);
        idleBossState.isRoar = true;
        CameraManager.instance.SetChromaticEffect(2.7f);
        yield return new WaitForSeconds(VoiceSoundManager.instatnce.bossVoiceDict["Boss-2"].length + 0.5f);

        VoiceSoundManager.instatnce.OnDanteVoice("Dante-HaHaLet'sGoForAWalkShallWe");
        yield return new WaitForSeconds(VoiceSoundManager.instatnce.danteVoiceDict["Dante-HaHaLet'sGoForAWalkShallWe"].length + 0.5f);
        GameManager.instance.OnStartBattle();
    }
}
