using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingColider : MonoBehaviour
{
    public float damage = 1000f;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + 10), 2 * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boss") 
        {
            //other.gameObject.GetComponent<BossHP>().TakeDamage = damage;
            Destroy(other.gameObject);
            
        }
    }
}
