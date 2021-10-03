using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptionManager : MonoBehaviour
{
    [SerializeField]
    Text textBG;

    [SerializeField]
    Text text;

    public void OnText(int id)
    {
        textBG.gameObject.SetActive(true);
        string str = GlobalState.captionList[id].KR;
        textBG.text = str;
        text.text = str;
    }

    public void OffText()
    {
        textBG.gameObject.SetActive(false);
    }

}
