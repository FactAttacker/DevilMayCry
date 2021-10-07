using UnityEngine;
using System.Collections;

public class CinematicCamera_6 : MonoBehaviour
{
    [SerializeField]
    Transform target;

    void Update()
    {
        Vector3 vec = target.position - transform.position;
        vec.Normalize();
        Quaternion q = Quaternion.LookRotation(vec);
        transform.rotation = q;
    }
}
