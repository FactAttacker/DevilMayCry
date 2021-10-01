using UnityEngine;
using System.Collections;

public class CinematicCameraMovingManager : MonoBehaviour
{
    #region instance
    public static CinematicCameraMovingManager instance;
    #endregion
    private void Awake()
    {
        if (instance == null) instance = this;
    }

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

    [SerializeField]
    GameObject boss;

    [SerializeField]
    GameObject player;

    public TargetType currentTarget = TargetType.BOSS;
    public CameraType currentCamera = CameraType.FOLLWER;

    public enum TargetType { PLAYER, BOSS };
    public enum CameraType { FOLLWER, ZOOM_IN, ZOOM_OUT, SHACKE };

    void Start()
    {

    }

    void OnFollwer()
    {
        if(currentTarget == TargetType.BOSS)
        {
            Transform target = currentTarget == TargetType.BOSS ? boss.transform : player.transform;
            Vector3 vec = target.position - transform.position;
            vec.Normalize();

            Quaternion q = Quaternion.LookRotation(vec);
            transform.rotation = q;
        }
    }
    void ExeShake(float time = 1.5f, float amount = 0.5f)
    {
        shakeInfo.time = time;
        shakeInfo.amount = amount;
        currentCamera = CameraType.SHACKE;
        StartCoroutine(ShakeCoroutine());
    }
    IEnumerator ShakeCoroutine()
    {
        shakeInfo.vector = transform.position;
        float amount = shakeInfo.amount;
        while (shakeInfo.time > 0)
        {
            shakeInfo.time -= Time.deltaTime;
            transform.position = (Random.insideUnitSphere * amount) + shakeInfo.vector;
            amount -= amount * 0.05f;
            yield return null;
        }
        currentCamera = CameraType.FOLLWER;
        shakeInfo.time = 0.0f;
    }


    void Update()
    {
        switch (currentCamera)
        {
            case CameraType.FOLLWER:
                OnFollwer();
                break;
            case CameraType.ZOOM_IN:
                break;
            case CameraType.ZOOM_OUT:
                break;
            case CameraType.SHACKE:
                ExeShake();
                break;
        }
        //transform.position = Vector3.Lerp(transform.position, playerObj.transform.position, cmSpeed * Time.deltaTime);
    }
}
