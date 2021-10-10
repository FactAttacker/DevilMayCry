using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    //public GameObject effect_Electronic_attach;
    //public GameObject effect_Electronic_hand;
    Animator anim;
    public GameObject enemy;
    public GameObject player;
    //GameObject wall;
    public Transform returnPosition;
    Vector3 hitPosition;

    LineRenderer lr;
    RaycastHit hit;

    bool isHooking = false;
    bool enemyHooked = false;

    public float ropeSpeed;
    float hookMaxDistance = 40f;
    float hookDistance = 0f;
    float hook_Speed = 1500f;

    Rigidbody rb;

    Vector3 originalPosition;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
        //effect = GetComponent<RFX4_EffectEvent>();
        //transform.position = hitPosition;
    }


    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) 
        {
            
        }
        originalPosition = returnPosition.position;
        lr.SetPosition(0, originalPosition);
        lr.SetPosition(1, transform.position);
        if (Input.GetKeyDown(KeyCode.J) && !isHooking && !enemyHooked)
        {
            //Instantiate(effect_Electronic_hand);
            //Instantiate(effect_Electronic);
            if (enemy == null) GameObject.FindGameObjectWithTag("Boss");
            player.transform.LookAt(enemy.transform);
            anim.SetTrigger("hook");
            StartCoroutine("StartHooking");
           
        }
        float time = Time.deltaTime;
        ReturnHook();
        BringTowardsPlayer();
    }


    IEnumerator StartHooking()
    {
        yield return new WaitForSeconds(0.1f);
        RaycastHit raycastHit;
        //Debug.DrawRay(transform.position, transform.forward * hook_Speed, Color.black);
        if (Physics.Raycast(transform.position, transform.forward, out raycastHit, hookMaxDistance))
        {
            print("launch");
            if (raycastHit.transform.gameObject.tag == "Boss")
            {
                enemyHooked = true;
                hitPosition = raycastHit.point;
            }
        }
    }

    void ReturnHook()
    {
        if (isHooking)
        {
            hookDistance = Vector3.Distance(transform.position, originalPosition);

            if (hookDistance > hookMaxDistance || enemyHooked)
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
            playerAnim.SetBool("isFlying", enemyHooked);
            player.transform.position = Vector3.Lerp(player.transform.position, hitPosition, (hookMaxDistance / 2) * Time.deltaTime);
            
            if (Vector3.Distance(player.transform.position, hitPosition) < 2f)
            {
                enemyHooked = false;
                //playerAnim.SetTrigger("prepair");
                playerAnim.SetBool("isFlying", enemyHooked);
                //playerAnim.ResetTrigger("prepair");
            }

        }
    }



    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Boss")
    //    {
    //        //CameraManager.instance.currentCamera = CameraManager.CameraType.ZOOM_HOCK;
    //        //Instantiate(effect_Electronic_attach);
    //        //wall = collision.gameObject;
    //        enemyHooked = true;
    //        hitPosition = collision.contacts[0].point;
    //        //Transform hit = hitPosition;
    //        //effect.OverrideAttachPointToTarget.position = hitPosition;
    //    }


    //}
}
