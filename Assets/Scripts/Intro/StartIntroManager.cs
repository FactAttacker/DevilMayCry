using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartIntroManager : MonoBehaviour
{
    [SerializeField]
    Transform menuTrans;
    List<GameObject> bgList = new List<GameObject>();
    int currentIndex = 0;
    bool isMenuSelected = true;
    Coroutine menuCorotine;
    WaitForSeconds menuWait = new WaitForSeconds(0.5f);
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

    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");
        if (isMenuSelected && v != 0)
        {
            isMenuSelected = false;
            if (menuCorotine != null) StopCoroutine(menuCorotine);
            menuCorotine = StartCoroutine(CoMenuSelecte(v));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (currentIndex)
            {
                case 0:
                    SceneManager.LoadSceneAsync(1);
                    break;
                case 3:
                    Application.Quit();
                    break;
            }
        }
    }
}
