using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptionManager : MonoBehaviour
{
    public static CaptionManager instatnce;
    private void Awake()
    {
        if (instatnce != null) return;
        instatnce = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField]
    Text textBG;

    [SerializeField]
    Text text;

    public bool isPlay = false;

    public void OnText(int id)
    {
        isPlay = true;
        textBG.gameObject.SetActive(isPlay);
        string str = GlobalState.captionList[id].KR;
        textBG.text = str;
        text.text = str;
    }

    public void OffText()
    {
        isPlay = false;
        textBG.gameObject.SetActive(isPlay);
    }

}
