using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalState : MonoBehaviour
{
    public static List<Excel_Player> playerList;
    public static List<Excel_Skill> skillList;

    public static Dictionary<string, Excel_Player> playerDict;
    public static Dictionary<string, Excel_Skill> skillDict;

    public static float currentHpCnt;
}
