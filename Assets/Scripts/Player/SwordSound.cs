using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSound : MonoBehaviour
{
    public bool danteSay = false;
    public static SwordSound instatnce;
    private void Awake()
    {
        if (instatnce != null) return;
        instatnce = this;
        DontDestroyOnLoad(gameObject);
    }
   
    [SerializeField]
    public AudioSource swordAudio;
    public AudioClip[] swordSoundClip;
    


    private void Start()
    {

        swordAudio = GetComponent<AudioSource>();
        VoiceSoundManager.instatnce.SetEffectSound(swordAudio);
    }
    public void OnSwordSound(int count) 
    {
        
        swordAudio.Stop();
        swordAudio.clip = swordSoundClip[count];
        swordAudio.Play();

        

    }
}
