using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenemachine_Player : MonoBehaviour
{
    public static Scenemachine_Player instance;
    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }

    //걷는 타겟위치
    public Transform destination;
    //걷는속도
    public float walkSpeed = 1f;
    Animator anim;
    public GameObject swordCase;
    public GameObject katana;

    //도는 속도
    public float rot_Speed = 1.5f;
    //얼마큼 돌지
    public float wantedRot_value = 250f;

    public float stopTime = 3f;

    [Header("In Sword Use")]
    public bool inputSword = false;
    bool isTurn = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        Invoke(nameof(OnInputSword), 1.3f * Time.deltaTime);
    }
    
 
    void Update()
    {
       
        if (transform.position != destination.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination.position, walkSpeed * Time.deltaTime);
        }
        if(Vector3.Distance(transform.position, destination.position) < 0.1)
        {
            transform.position = destination.position;
            OnTurn();
        }

        //if (Vector3.Distance(transform.position,destination.position)<0.2)
        
    }

    void OnInputSword()
    {
        inputSword = false;
        anim.SetTrigger("walkWithinput");
        AloneLeg(true);
        StartCoroutine(CoInputSword());
    }
    void OnTurn()
    {
        anim.SetTrigger("Stop");
        StartCoroutine(CoStopTime());
    }

    IEnumerator CoStopTime() 
    {
        yield return new WaitForSeconds(stopTime);
        CinematicManager.instance.StopSound();

        anim.SetTrigger("Turn");
        float lerpAngle = Mathf.LerpAngle(transform.eulerAngles.y, wantedRot_value, rot_Speed * Time.deltaTime);
        //transform.Rotate(0f, angle, 0.0f);
        //transform.eulerAngles = new Vector3(0, rotationy, 0);
        transform.eulerAngles = new Vector3(0, lerpAngle, 0);
    }

    public IEnumerator CoInputSword() 
    {             
        yield return new WaitForSeconds(0.4f); 
        yield return null;
        katana.transform.SetParent(swordCase.transform, false);
        katana.transform.localPosition = new Vector3(0.01705508f, -0.4062368f, -0.143f);
        katana.transform.localEulerAngles = new Vector3(0f, -180f, 20f);
        //anim.SetBool("inputSword", true);
        
        anim.SetBool("AloneLeg", false);
        
    }
    void AloneLeg(bool finish) 
    {
        anim.SetBool("AloneLeg", finish);
    }
}
