using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameOption : MonoBehaviour
{
    #region instance;
    public static GameOption instance;
    private void Awake()
    {
        if (instance != null) return;
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [Header("BGM Bar")]
    public Slider bgmSlider;

    [Header("Effect Bar")]
    public Slider effectSlider;

    [System.Serializable]
    public class Modal
    {
        public GameObject modalObj;
        public enum Type
        {
            Option,
            Creator
        }
        [System.Serializable]
        public class Box
        {
            public Type type;
            public GameObject box;
        }
        [SerializeField]
        public Box[] boxes;
    }
    public Modal modal;

    [System.Serializable]
    class FocusMg
    {
        public int index = 0;
        [System.Serializable]
        public class Item{
            public enum Type
            {
                BGM,
                EFFECT,
                LOBBY,
                EXIT
            }
            public Type type;
            public Text text;
            public GameObject On;
        }
        public Item[] items;
    }
    [SerializeField]
    FocusMg focusMg;

    private void Start()
    {
        bgmSlider.value = VoiceSoundManager.instatnce.BgmVolum;
        effectSlider.value = VoiceSoundManager.instatnce.EffectVolum;
        SetVolumText(0, (int)(VoiceSoundManager.instatnce.BgmVolum * 10));
        SetVolumText(1, (int)(VoiceSoundManager.instatnce.EffectVolum * 10));
    }
    public void OpenOptionWindow()
    {
        modal.modalObj.SetActive(true);
    }

    /// <summary>
    /// Game Exit
    /// </summary>
    public void ExitGame()
    {
        FadeInOutController.instance.OnFadeInOut(-1);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="i"></param>
    public void OpenIndexModal(int i)
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        focusMg.items[2].text.gameObject.SetActive(index != 0);
        focusMg.items[3].text.gameObject.SetActive(index != 0);

        OpenOptionWindow();
        modal.boxes[i - 1].box.SetActive(true);
        switch (i)
        {
            case 1://Option

                break;
            case 2://Creator
                modal.boxes[i - 1].box.transform.Find("Text").TryGetComponent(out Text text);
                text.text = $"PARK <b><color=#C12C20>D</color></b>O HYUN\n"
                           + "LEE SEUNG <b><color=#C12C20>M</color></b>IN\n"
                           + "YOO HYUN <b><color=#C12C20>C</color></b>HANG";
                break;
        }
    }

    void SetVolumText(int i, int value)
    {
        focusMg.items[i].text.text = $"<color=#938C27> < </color> {value} <color=#938C27>  > </color>";
    }

    void LateUpdate()
    {
        if (modal.modalObj.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                foreach (Modal.Box content in modal.boxes) content.box.SetActive(false);
                modal.modalObj.SetActive(false);
                if (SceneManager.GetActiveScene().buildIndex != 0){
                    GameManager.instance.isPause = false; //추후 isPause로 플레이어 움직임 정지
                    Time.timeScale = 1;
                }
            }

            if (modal.boxes[0].box.activeSelf)
            {
                int v = 0;

                if (Input.GetKeyUp(KeyCode.UpArrow)
                 || Input.GetKeyUp(KeyCode.W)) v = -1;

                if (Input.GetKeyUp(KeyCode.DownArrow)
                 || Input.GetKeyUp(KeyCode.S)) v = 1;

                if(v != 0) focusMg.index += v;

                // index calc
                if (focusMg.index >= focusMg.items.Length) focusMg.index = 0;
                if(focusMg.index < 0) focusMg.index = focusMg.items.Length - 1;

                // On Selected
                for (int i =0; i < focusMg.items.Length; i++) focusMg.items[i].On.SetActive(i == focusMg.index);

                float x = 0.0f;
                if (Input.GetKeyUp(KeyCode.LeftArrow)
                 || Input.GetKeyUp(KeyCode.A)) x = -0.1f;

                if (Input.GetKeyUp(KeyCode.RightArrow)
                 || Input.GetKeyUp(KeyCode.D)) x = 0.1f;

                if(x != 0.0f)
                {
                    float value = 0;
                    switch (focusMg.index)
                    {
                        case 0: //BGM
                            value = VoiceSoundManager.instatnce.BgmVolum + x;
                            if (value < 0.0) value = 0;
                            if (value > 1) value = 1;
                            VoiceSoundManager.instatnce.BgmVolum = (float)(Math.Truncate(value * 10)/ 10);
                            SetVolumText(focusMg.index, (int)(VoiceSoundManager.instatnce.BgmVolum * 10));
                            break;
                        case 1: //Effect
                            value = VoiceSoundManager.instatnce.EffectVolum + x;
                            if (value < 0.0) value = 0;
                            if (value > 1) value = 1;
                            VoiceSoundManager.instatnce.EffectVolum = (float)(Math.Truncate(value * 10) / 10);
                            SetVolumText(focusMg.index, (int)(VoiceSoundManager.instatnce.EffectVolum * 10));
                            break;
                    }
                }

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    switch (focusMg.index)
                    {
                        case 2: //Lobby
                            GameManager.instance.isBattle = false;
                            modal.modalObj.SetActive(false);
                            VoiceSoundManager.instatnce.SetBGMChange(0).PlayDelayed(0.2f);
                            Time.timeScale = 1;
                            FadeInOutController.instance.OnFadeInOut(0);
                            break;
                        case 3: //Exit
                            modal.modalObj.SetActive(false);
                            Time.timeScale = 1;
                            FadeInOutController.instance.OnFadeInOut(-1);
                            break;
                    }
                }
            }
        }
        else
        {
            if(SceneManager.GetActiveScene().buildIndex != 0
                && Input.GetKeyDown(KeyCode.Escape))
            {
                OpenIndexModal(1);
                Time.timeScale = 0;
                VoiceSoundManager.instatnce.OnAllControlSound(VoiceSoundManager.AllSoundControlType.PAUSE);
                GameManager.instance.isPause = true; //추후 isPause로 플레이어 움직임 정지
            }
        }
    }
}
