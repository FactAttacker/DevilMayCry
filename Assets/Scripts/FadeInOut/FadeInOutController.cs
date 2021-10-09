using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeInOutController : MonoBehaviour
{
    #region 
    public static FadeInOutController instance;
    private void Awake() {
        if (instance == null) instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [SerializeField]
    [Header("▼ Fade Speed")]
    float speed = 0f;

    [SerializeField]
    [Header("▼ Fade Image")]
    Image fadeImg;

    public bool isFade = false;

    public void OnFadeInOut(int sceneIndex)
    {
        StartCoroutine(CoFadeIn(sceneIndex));
    }
    IEnumerator CoFadeIn(int sceneIndex)
    {
        float time = 0f;
        fadeImg.gameObject.SetActive(true);

        while (time < 1)
        {
            fadeImg.color = Color.Lerp(new Color(0, 0, 0, 0f), new Color(0, 0, 0, 1f), time);
            time += (Time.deltaTime * speed);
            yield return null;
        }
        fadeImg.color = Color.black;
        yield return new WaitForSeconds(1f);
        // Game Exit 
        if (sceneIndex == -1) Application.Quit();
        // Game Move
        float progress = GameManager.instance.SceneMove(sceneIndex);

        yield return new WaitUntil(() => progress == 0);
        yield return new WaitForSeconds(1f);

        time = 0f;
        while (time < 1)
        {
            fadeImg.color = Color.Lerp(new Color(0, 0, 0, 1f), new Color(0, 0, 0, 0f), time);
            time += (Time.deltaTime * speed);
            yield return null;
        }
        
        fadeImg.gameObject.SetActive(false);
        isFade = false;
    }
}
