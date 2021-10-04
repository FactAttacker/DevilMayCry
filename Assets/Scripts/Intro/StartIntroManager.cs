using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartIntroManager : MonoBehaviour
{
    [SerializeField]
    [Header("▼ Menu Box")]
    Transform menuTrans;

    List<GameObject> bgList = new List<GameObject>();
    int currentIndex = 0;
    bool isMenuSelected = true;
    Coroutine menuCorotine;
    WaitForSeconds menuWait = new WaitForSeconds(0.2f);

    [SerializeField]
    AudioSource audio;

    [SerializeField]
    AudioClip clip;

    void Start()
    {
        for(int i =0; i < menuTrans.childCount; i++)
        {
            bgList.Add(menuTrans.GetChild(i).Find("BG").gameObject);
        }
    }
  
   
    IEnumerator CoMenuSelecte(float v)
    {
        isMenuSelected = false;
        if (!isMenuSelected)
        {
            switch (v)
            {
                case 1:
                    if (currentIndex == 0) currentIndex = bgList.Count - 1;
                    else --currentIndex;
                    break;
                case -1:
                    if (currentIndex == (bgList.Count - 1)) currentIndex = 0;
                    else ++currentIndex;
                    break;
            }

            for (int i = 0; i < bgList.Count; i++)
            {
                bgList[i].SetActive(i == currentIndex);
            }

        }
        yield return menuWait;
        isMenuSelected = true;
    }
    void ChangeBGM()
    {
        audio.Stop();
        audio.clip = clip;
        audio.loop = false;
        audio.Play();
        StartCoroutine(COChangeBGM());
    }
    IEnumerator COChangeBGM()
    {
        yield return new WaitUntil(() => audio.time < (clip.length/10f) );
        FadeInOutController.instance.OnFadeInOut(1);
    }

    void Update()
    {
        float v = 0;

        if (Input.GetKeyUp(KeyCode.UpArrow)
         || Input.GetKeyUp(KeyCode.W)) v = 1;

        if (Input.GetKeyUp(KeyCode.DownArrow)
         || Input.GetKeyUp(KeyCode.S)) v = -1;

        if (isMenuSelected && v != 0.0f)
        {
            isMenuSelected = false;
            if (menuCorotine != null) StopCoroutine(menuCorotine);
            menuCorotine = StartCoroutine(CoMenuSelecte(v));
        }
        if (!FadeInOutController.instance.isFade &&
            Input.GetKeyDown(KeyCode.Space))
        {
            switch (currentIndex)
            {
                case 0:
                    FadeInOutController.instance.isFade = true;
                    ChangeBGM();
                    break;
                case 3:
                    Application.Quit();
                    break;
            }
        }
    }
}
