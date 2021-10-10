using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class CameraManager : MonoBehaviour
{
    #region instance
    public static CameraManager instance;
    #endregion

    [SerializeField]
    Transform _camera;

    [SerializeField]
    GameObject playerObj;

    [SerializeField]
    GameObject bossObj;

    [SerializeField]
    Transform[] zoomPos;

    [SerializeField]
    Transform[] cameraPos;

    [SerializeField]
    float cmSpeed = 1.5f;

    [SerializeField]
    PostProcessVolume chromatic;
    bool isChromatic = false;

    public bool canBossUlimate = false;

    #region Effect
    enum EffectType
    {
        WAVE
    }

    [System.Serializable]
    class Effect
    {
        public EffectType type;
        public GameObject obj;
    }
    [SerializeField]
    Effect[] effects;
    #endregion

    #region Shake
    [System.Serializable]
    class ShakeInfo
    {
        [HideInInspector]
        public Vector3 vector;
        public float time = 1.5f;
        public float amount = 0.05f;
    }
    [SerializeField]
    ShakeInfo shakeInfo;
    bool canShake = true;
    #endregion

    #region Type
    public enum TargetType { PLAYER, BOSS };
    public enum CameraMove { NORMAL, BACK };
    public enum CameraType {
        FOLLWER,
        ZOOM_IN,
        ZOOM_OUT,
        SHAKE,
        ZOOM_HOCK,
        ZOOM_ATT_DOWN,
    };
    public TargetType currentTarget = TargetType.BOSS;
    public CameraType currentCamera = CameraType.FOLLWER;
    public CameraMove currentPos = CameraMove.NORMAL;
    #endregion

    bool isBossUlimate = false;
    Dictionary<EffectType, GameObject> effectDict;

    bool isBattleEnd = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        effectDict = new Dictionary<EffectType, GameObject>();
        foreach(Effect data in effects)
        {
            effectDict.Add(data.type, data.obj);
        }
    }

    private void Start()
    {
        shakeInfo.vector = new Vector3(0f, 0f, -5f);
    }
    
    public void OnTargetLook(TargetType type)
    {
        if(type == TargetType.BOSS)
        {
            Transform target = type == TargetType.BOSS ? bossObj.transform : playerObj.transform;
            Vector3 vec = target.position - transform.position;
            vec.Normalize();

            Quaternion q = Quaternion.LookRotation(vec);
            transform.rotation = q;
            _camera.rotation = q;
        }
        //CameraMultiTargeter.instance._camera.transform.rotation = q;

        //transform.rotation = Quaternion.Euler(new Vector3(0, q.y, q.z));
        //transform.LookAt(type == TargetType.BOSS ? bossObj.transform : playerObj.transform);
        //CameraMultiTargeter.instance._camera.transform.rotation = Quaternion.Euler(new Vector3(q.x,0,0));
    }

    public void EndGame()
    {
        OnPlayerZoomIn();
    }
    public void OnPlayerZoomIn()
    {
        isBattleEnd = true;
        GameManager.instance.isBattle = false;
        StartCoroutine(CoEndGame());
    }
    IEnumerator CoEndGame()
    {
        float time = 1f;
        while(time > 0)
        {
            time -= Time.deltaTime;
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, zoomPos[0].position, 3f * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        FadeInOutController.instance.OnFadeInOut(3);
    }

    public void OnPlayerZoomOut()
    {
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, zoomPos[1].position, 3f * Time.deltaTime);
    }

    public void OnPlayerZoomHook()
    {
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, zoomPos[2].position, 2f * Time.deltaTime);
        Invoke(nameof(ResetZoom), 1f);
    }
    public void OnPlayerZoomAttackDown()
    {
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, zoomPos[3].position, 3f * Time.deltaTime);
        Invoke(nameof(ResetZoom), 1f);
    }

    public void OnBossUltimateAttack()
    {
        currentPos = CameraMove.BACK;
        if (!isBossUlimate)
        {
            isBossUlimate = true;
            SetChromaticEffect(0.1f);
            StartCoroutine(CoBossUltimateAttack());
        }
    }
    IEnumerator CoBossUltimateAttack()
    {
        yield return new WaitForSeconds(0.5f);
        currentPos = CameraMove.NORMAL;
        isBossUlimate = false;
    }
    void ResetZoom()
    {
        currentCamera = CameraType.FOLLWER;
        _camera.transform.position = zoomPos[1].position;
    }

    /// <summary>
    /// Camera Shake execution
    /// </summary>
    /// <param name="time">Shake Time</param>
    /// <param name="amount">Shake Strong</param>
    public void OnShake(float time = 1.5f, float amount = 0.5f)
    {
        shakeInfo.time = time;
        shakeInfo.amount = amount;
        currentCamera = CameraType.SHAKE;
    }

    /// <summary>
    /// Camera Shake
    /// </summary>
    void ExeShake()
    {
        if (canShake)
        {
            canShake = false;
            StartCoroutine(ShakeCoroutine());
        }
    }
    IEnumerator ShakeCoroutine()
    {
        shakeInfo.vector = _camera.position;
        float amount = shakeInfo.amount;
        //if (effectDict[EffectType.WAVE].activeInHierarchy) effectDict[EffectType.WAVE].SetActive(false);

        //effectDict[EffectType.WAVE].SetActive(true);
        //effectDict[EffectType.WAVE].TryGetComponent(out RFX4_EffectSettings wave);

        //Wave Setting
        //wave.ParticlesBudget = 1;
        //wave.FadeoutTime = shakeInfo.time;

        while (shakeInfo.time > 0)
        {
            shakeInfo.time -= Time.deltaTime;
            _camera.position = (Random.insideUnitSphere * amount) + zoomPos[1].position;//shakeInfo.vector; 
            amount -= amount * 0.05f;
            yield return null;
        }
        //effectDict[EffectType.WAVE].SetActive(false);
        canShake = true;
        currentCamera = CameraType.FOLLWER;
        shakeInfo.time = 0.0f;
        _camera.transform.position = zoomPos[1].position;
    }

    public void SetChromaticEffect(float speed = 0.5f)
    {
        if (!isChromatic)
        {
            isChromatic = true;
            StartCoroutine(CoChromaticEffect(speed));
        }
    }
    IEnumerator CoChromaticEffect(float speed)
    {
        float time = 0f;
        chromatic.weight = 0.5f;
        while (time < 1)
        {
            chromatic.weight = Mathf.Lerp(1f,0f, time);
            time += (Time.deltaTime * speed);
            yield return null;
        }
        chromatic.weight = 1;
        time = 0f;
        while (time < 1)
        {
            chromatic.weight = Mathf.Lerp(0f,1f, time);
            time += (Time.deltaTime * speed);
            yield return null;
        }
        chromatic.weight = 0;
        isChromatic = false;
    }

    void Update()
    {
        if (isBattleEnd) return;
        OnTargetLook(currentTarget);
        switch (currentCamera)
        {
            case CameraType.FOLLWER:
                //transform.position = Vector3.MoveTowards(transform.position, playerObj.transform.position, 0.7f);
                break;
            case CameraType.ZOOM_IN:
                OnPlayerZoomIn();
                break;
            case CameraType.ZOOM_OUT:
                OnPlayerZoomOut();
                break;
            case CameraType.ZOOM_HOCK:
                OnPlayerZoomHook();
                SetChromaticEffect(2f);
                break;
            case CameraType.ZOOM_ATT_DOWN:
                OnPlayerZoomAttackDown();
                break;
            case CameraType.SHAKE:
                ExeShake();
                break;
        }
        if (canBossUlimate)
        {
            canBossUlimate = false;
            OnBossUltimateAttack();
        }
        switch (currentPos)
        {
            case CameraMove.NORMAL:
                transform.position = Vector3.Lerp(transform.position, playerObj.transform.position, cmSpeed * Time.deltaTime);
                break;
            case CameraMove.BACK:
                transform.position = Vector3.Lerp(transform.position, cameraPos[0].transform.position, cmSpeed * Time.deltaTime);
                break;
        }
    }
}
 