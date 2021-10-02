using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;
    //GameObject wall;
    public Transform returnPosition;
    Vector3 hitPosition;

    LineRenderer lr;

    bool isHooking = false;
    bool enemyHooked = false;

    public float ropeSpeed;
    float hookMaxDistance = 10f;
    float hookDistance = 0f;
    float hook_Speed = 1500f;

    Rigidbody rb;

    Vector3 originalPosition;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
        //transform.position = hitPosition;
    }



    private void Update()
    {
        originalPosition = returnPosition.position;
        lr.SetPosition(0, originalPosition);
        lr.SetPosition(1, transform.position);
        if (Input.GetKeyDown(KeyCode.J) && !isHooking && !enemyHooked) 
        {
            player.transform.LookAt(enemy.transform);
            
            StartCoroutine("StartHooking");
        }
        float time = Time.deltaTime;
        ReturnHook();
        BringTowardsPlayer();
    }


    IEnumerator StartHooking() 
    {
        yield return new WaitForSeconds(0.1f);
        isHooking = true;
        rb.isKinematic = false;
        rb.AddForce(player.transform.forward * hook_Speed);
    }

    void ReturnHook() 
    {
        if (isHooking) 
        {
            
            hookDistance = Vector3.Distance(transform.position, originalPosition);
            if (hookDistance>hookMaxDistance || enemyHooked) 
            {
                rb.isKinematic = true;
                transform.position = originalPosition;
                isHooking = false;
            }
            
        }
        
    }

    Animator playerAnim;
    void BringTowardsPlayer() 
    {
        if (enemyHooked) 
        {
            playerAnim = player.GetComponent<Animator>();
            playerAnim.SetTrigger("prepair");
            playerAnim.SetBool("isFlying",enemyHooked);
            player.transform.position = Vector3.Lerp(player.transform.position , hitPosition, hookMaxDistance*Time.deltaTime);
            if (Vector3.Distance(player.transform.position, hitPosition) < 2f) 
            {
                enemyHooked = false;
                //playerAnim.SetTrigger("prepair");
                playerAnim.SetBool("isFlying", enemyHooked);
                //playerAnim.ResetTrigger("prepair");
            }
            
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cube") 
        {
            enemyHooked = true;
            //wall = collision.gameObject;
            hitPosition = collision.contacts[0].point;
        }
    }
}
