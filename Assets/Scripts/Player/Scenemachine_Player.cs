using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenemachine_Player : MonoBehaviour
{
    public Transform destination;
    public float walkSpeed;
    Animator anim;
    public GameObject swordCase;
    public GameObject katana;
    public float rot_Speed = 1.5f;
    public float wantedRot_value = 250f;
    private void Start()
    {
        anim = GetComponent<Animator>();
        walkSpeed = 1f;
    }
    bool fine = true;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            
            anim.SetTrigger("walkWithinput");
            StartCoroutine(InputSword());
        }
        AloneLeg(fine);
        transform.position = Vector3.MoveTowards(transform.position, destination.position, walkSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position,destination.position)<0.1) 
        {
            anim.SetTrigger("Stop");
            StartCoroutine(StopTime());
        }

    }

    IEnumerator StopTime() 
    {
        yield return new WaitForSeconds(3f);
        anim.SetTrigger("Turn");
        float lerpAngle = Mathf.LerpAngle(transform.eulerAngles.y, wantedRot_value, rot_Speed * Time.deltaTime);
        //transform.Rotate(0f, angle, 0.0f);
        //transform.eulerAngles = new Vector3(0, rotationy, 0);
        transform.eulerAngles = new Vector3(0, lerpAngle, 0);
    }

    public IEnumerator InputSword() 
    {             
        yield return new WaitForSeconds(0.4f); 
        yield return null;
        katana.transform.SetParent(swordCase.transform, false);
        katana.transform.localPosition = new Vector3(0.01705508f, -0.4062368f, -0.143f);
        katana.transform.localEulerAngles = new Vector3(0f, -180f, 20f);
        //anim.SetBool("inputSword", true);
        fine = false;
        anim.SetBool("AloneLeg", fine);
        
    }
    void AloneLeg(bool finish) 
    {
        anim.SetBool("AloneLeg", finish);
    }
}
