using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSound : MonoBehaviour
{
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
    }
    public void OnSwordSound(int count) 
    {
        swordAudio.clip = swordSoundClip[count];
        swordAudio.Play();
    }
}
