using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{   
    public GameObject bloodEffect ;
    public float damage;

    public bool hit = false;
    private void OnEnable()
    {
        bloodEffect.SetActive(false);
    }
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
            bloodEffect.SetActive(true);
            hit = true;
            StartCoroutine(FadeBloodEffect());
        }
    }



    IEnumerator FadeBloodEffect()
    {
        yield return new WaitForSeconds(1f);
        bloodEffect.SetActive(false);
    }
    private void OnDisable()
    {
        bloodEffect.SetActive(false);
        hit = false;
    }

}
