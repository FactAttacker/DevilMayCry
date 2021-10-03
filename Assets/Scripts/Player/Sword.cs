using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float damage;
    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
        
        if (collision.gameObject.tag == "Cube") 
        {
            
            collision.gameObject.GetComponent<BossHP>().TakeDamage = damage;
            print("demage");

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            GameObject.Find("Boss_Perderos").TryGetComponent(out BossHP bossHP);
            bossHP.TakeDamage = damage;
            print("demage");
            CameraManager.instance.OnShake(0.3f, 0.1f);
        }
    }

}
