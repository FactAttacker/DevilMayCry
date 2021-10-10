using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioProcess : MonoBehaviour
{
    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Destroy(gameObject);
        }
        TryGetComponent(out AudioSource audio);
        VoiceSoundManager.instatnce.SetEffectSound(audio);
    }
}
