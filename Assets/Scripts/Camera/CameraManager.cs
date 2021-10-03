using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    float cmSpeed = 1.5f;

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

    public enum TargetType { PLAYER, BOSS };
    public enum CameraType { FOLLWER, ZOOM_IN, ZOOM_OUT,SHACKE};
    public TargetType currentTarget = TargetType.BOSS;
    public CameraType currentCamera = CameraType.FOLLWER;

    
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        shakeInfo.vector = new Vector3(0f, 0f, -5f);
    }
    
    public void OnTargetLook(TargetType type)
    {
        Transform target = type == TargetType.BOSS ? bossObj.transform : playerObj.transform;
        Vector3 vec = target.position - transform.position;
        vec.Normalize();

        if(type == TargetType.BOSS)
        {
            Quaternion q = Quaternion.LookRotation(vec);
            transform.rotation = q;
            _camera.rotation = q;
        }
        //CameraMultiTargeter.instance._camera.transform.rotation = q;

        //transform.rotation = Quaternion.Euler(new Vector3(0, q.y, q.z));
        //transform.LookAt(type == TargetType.BOSS ? bossObj.transform : playerObj.transform);
        //CameraMultiTargeter.instance._camera.transform.rotation = Quaternion.Euler(new Vector3(q.x,0,0));
    }

    public void OnPlayerZoomIn()
    {
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, zoomPos[0].position, 3f * Time.deltaTime);
    }

    public void OnPlayerZoomOut()
    {
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, zoomPos[1].position, 3f * Time.deltaTime);
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
        currentCamera = CameraType.SHACKE;
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
        while (shakeInfo.time > 0)
        {
            shakeInfo.time -= Time.deltaTime;
            _camera.position = (Random.insideUnitSphere * amount) + zoomPos[1].position;//shakeInfo.vector; 
            amount -= amount * 0.05f;
            yield return null;
        }
        canShake = true;
        currentCamera = CameraType.FOLLWER;
        shakeInfo.time = 0.0f;
        _camera.transform.position = zoomPos[1].position;
    }

    void Update()
    {
        switch (currentCamera)
        {
            case CameraType.FOLLWER:
                OnTargetLook(currentTarget);
                //transform.position = Vector3.MoveTowards(transform.position, playerObj.transform.position, 0.7f);
                break;
            case CameraType.ZOOM_IN:
                OnPlayerZoomIn();
                break;
            case CameraType.ZOOM_OUT:
                OnPlayerZoomOut();
                break;
            case CameraType.SHACKE:
                ExeShake();
                break;
        }
        transform.position = Vector3.Lerp(transform.position, playerObj.transform.position, cmSpeed * Time.deltaTime);
    }
}
 