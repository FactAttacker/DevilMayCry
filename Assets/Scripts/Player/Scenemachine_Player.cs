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
    public float walkSpeed = 1.5f;
    public Animator anim;
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
        //Invoke(nameof(OnInputSword), 1.3f * Time.deltaTime);
    }

    /// <summary>
    /// Walking
    /// </summary>
    public void OnWalking()
    {
        StartCoroutine(CoOnWalking());
    }
    IEnumerator CoOnWalking()
    {
        bool isWalk = true;
        while (isWalk)
        {
            if (transform.position != destination.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination.position, walkSpeed * Time.deltaTime);
            }
            if(Vector3.Distance(transform.position, destination.position) < 0.1)
            {
                transform.position = destination.position;
                OnTurn();
                isWalk = false;
            }
            yield return null;
        }
    }

    public void OnInputSword()
    {
        inputSword = false;
        anim.SetTrigger("walkWithinput");
        AloneLeg(true);
        StartCoroutine(CoInputSword());
    }
    IEnumerator CoInputSword()
    {
        yield return null;
        katana.transform.SetParent(swordCase.transform, false);
        katana.transform.localPosition = new Vector3(0.01705508f, -0.4062368f, -0.143f);
        katana.transform.localEulerAngles = new Vector3(0f, -180f, 20f);
        //anim.SetBool("inputSword", true);

        anim.SetBool("AloneLeg", false);
    }

    public void OnTurn()
    {
        anim.SetTrigger("Stop");
        StartCoroutine(CoStopTime());
    }
    IEnumerator CoStopTime() 
    {
        yield return new WaitForSeconds(stopTime);

        anim.SetTrigger("Turn");
        float lerpAngle = Mathf.LerpAngle(transform.eulerAngles.y, wantedRot_value, rot_Speed * Time.deltaTime);
        //transform.Rotate(0f, angle, 0.0f);
        //transform.eulerAngles = new Vector3(0, rotationy, 0);
        transform.eulerAngles = new Vector3(0, lerpAngle, 0);
    }

    public void AloneLeg(bool finish)
    {
        anim.SetBool("AloneLeg", finish);
    }
}
