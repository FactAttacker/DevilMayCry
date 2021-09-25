using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;

public class GlobalDataImporter : MonoBehaviour
{
    [SerializeField]
    GoogleSheetData googleSheetData;

    [System.Serializable]
    public class GoogleSheetData
    {
        [Header("플레이어")]
        public TextAsset player;

        [Header("스킬")]
        public TextAsset skill;

        [Header("자막")]
        public TextAsset caption;

        [Header("아이템")]
        public TextAsset item;
    }

    void Awake()
    {
        //Player 정보 담기
        GlobalState.playerList = JsonConvert.DeserializeObject<List<Excel_Player>>(googleSheetData.player.ToString());
        
        //Skill 정보 담기
        GlobalState.skillList = JsonConvert.DeserializeObject<List<Excel_Skill>>(googleSheetData.skill.ToString());//Skill 정보 담기

        //Skill 정보 담기
        GlobalState.captionList = JsonConvert.DeserializeObject<List<Excel_Caption>>(googleSheetData.caption.ToString());

        //item 정보 담기
        GlobalState.itemList = JsonConvert.DeserializeObject<List<Excel_Item>>(googleSheetData.item.ToString());
    }
}
