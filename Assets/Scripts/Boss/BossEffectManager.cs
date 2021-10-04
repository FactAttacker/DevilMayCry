using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class BossEffect
{
    public string effectName;
    /*[HideInInspector] */public List<GameObject> bossEffectPool = new List<GameObject>();
}

public class BossEffectManager : MonoBehaviour
{
    [SerializeField, Header("오브젝트 풀")] BossEffect[] bossEffectArr;
    [SerializeField, Header("보스가 쓸 이펙트 목록")] GameObject[] bossEffects;
    Dictionary<string, GameObject> effectListsArray = new Dictionary<string, GameObject>();

    void Start()
    {
        bossEffectArr = new BossEffect[bossEffects.Length];
        
        for (int i = 0; i < bossEffects.Length; i++)
        {
            if (bossEffectArr[i] == null) bossEffectArr[i] = new BossEffect();

            bossEffectArr[i].effectName = bossEffects[i].gameObject.name;
            for (int j = 0; j < 10; j++)
            {
                GameObject clone;
                bossEffectArr[i].bossEffectPool.Add(clone = Instantiate(bossEffects[i]));
                clone.SetActive(false);
            }
        }
    }

    public GameObject GetEffectToName(string _effectName)
    {
        GameObject returnEffect = null;
        for (int i = 0; i < bossEffectArr.Length; i++)
        {
            if (bossEffectArr[i].effectName != _effectName) continue;
            foreach (GameObject effect in bossEffectArr[i].bossEffectPool)
            {
                if(!effect.activeInHierarchy)
                {
                    returnEffect = effect;
                    break;
                }
            }
            break;
        }
        print(returnEffect.name);
        return returnEffect;
    }
}
