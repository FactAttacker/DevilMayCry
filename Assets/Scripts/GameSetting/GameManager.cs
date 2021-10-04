using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region instance;
    public static GameManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public int currentScene; 

    public bool isBattle = false;
  
    public void OnStartBattle() {
        isBattle = true;
    }

    void Start()
    {
        MakeDataDict();
    }

    public float SceneMove(int index)
    {
        return SceneManager.LoadSceneAsync(index).progress;
    }
    void MakeDataDict()
    {
        GlobalState.playerDict = new Dictionary<string, Excel_Player>();
        //GlobalState.skillDict = new Dictionary<string, Excel_Skill>();
        GlobalState.captionDict = new Dictionary<string, Excel_Caption>();

        //Player
        foreach (Excel_Player data in GlobalState.playerList)
        {
            //playerDict.Add(data.)
        };

        //Skill
        //foreach (Excel_Skill data in GlobalState.skillList)
        //    GlobalState.skillDict.Add(data.Name, data);

        //Caption
        foreach (Excel_Caption data in GlobalState.captionList)
        {
            GlobalState.captionDict.Add(data.SoundName, data);
        }
    }
}
