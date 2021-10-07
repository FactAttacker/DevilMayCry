using UnityEngine;
using System.Collections;

public class CinematicCamera_3 : MonoBehaviour
{
    [SerializeField]
    Transform target;

    public float cmSpeed = 3;
    public bool canShake = false;

    float amount = 1f;
    void Start()
    {
        //vector = new Vector3(0f, 0f, -5f);
        Invoke(nameof(ExeShake), 1f);
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
        //vector = transform.position;
       
        float time = 0.5f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            transform.position = (Random.insideUnitSphere * amount) + target.position;//shakeInfo.vector; 
            amount -= amount * 0.05f;
            yield return null;
        }
        canShake = true;
        transform.position = target.position;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position, cmSpeed * Time.deltaTime);
    }
}
