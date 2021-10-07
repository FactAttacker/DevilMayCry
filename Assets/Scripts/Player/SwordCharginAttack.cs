using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCharginAttack : MonoBehaviour
{
   // public GameObject launch;
    public Transform attachPoint;
    public Transform boss;
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    float chargetime = 0f;

    void ChargeTime()
    {
        chargetime += Time.deltaTime;
        print(chargetime);
        if (chargetime > 0.2f)
        {
            anim.SetBool("Charging", false);
            CancelInvoke("ChargeTime");
            chargetime = 0f;
            anim.SetTrigger("ChargeAttack");
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            anim.SetBool("Charging", true);
            InvokeRepeating("ChargeTime", 0.1f, 0.1f);
            
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            
            //attachPoint.gameObject.SetActive(true);
            attachPoint = boss;
            CancelInvoke("ChargeTime");
            anim.SetBool("Charging", false);
            chargetime = 0f;
            
        }
    }
}
