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
    [SerializeField, Header("������Ʈ Ǯ")] BossEffect[] bossEffectArr;
    [SerializeField, Header("������ �� ����Ʈ ���")] GameObject[] bossEffects;
    Dictionary<string, GameObject> effectListsArray = new Dictionary<string, GameObject>();

    #region Unity Life Cycle

    private void Awake()
    {
        Object[] effects = Resources.LoadAll("Effect/BossEffect", typeof(GameObject));
        bossEffects = new GameObject[effects.Length];
        for (int i = 0; i < bossEffects.Length; i++)
        {
            bossEffects[i] = effects[i] as GameObject;
        }

        for (int i = 0; i < effects.Length; i++)
        {
            print(effects[i].name);
        }
    }

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

    #endregion

    #region Implementation Space

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

    #endregion
}
