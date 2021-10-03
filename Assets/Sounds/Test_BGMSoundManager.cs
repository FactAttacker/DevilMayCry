using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_BGMSoundManager : MonoBehaviour
{
    public float cnt = 10f;
    void Start()
    {
        TryGetComponent(out AudioSource audio);
        audio.time = (audio.clip.length / cnt);
    }

    void Update()
    {
        
    }
}
