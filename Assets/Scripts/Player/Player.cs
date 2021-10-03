using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool knuckBack = false;

    Sword sword;



    Transform myTransform;
    [Header("PlayerMoveRelated")]
    public float walk_Speed = 10f;
    public float speed;
    public float run_Speed = 20f;
    public float jumpSpeed;
    bool jumpPing = false;
    public float gravity;
    Rigidbody rb;
    private Vector3 MoveDir;


    [Header("PlayerAttackRelated")]

    public bool nextAttack = true;
    //bool swordJudge = false;
    public enum FighterState { WithoutSowrd, SwordMode }
    public FighterState fighterState = FighterState.WithoutSowrd;
    public enum FighterAttackState { Attack1, Attack2, Attack3, Attack4, hiddenAttack }
    public FighterAttackState attackState = FighterAttackState.Attack1;
    public Animator anim;


    [Header("PlayerWeaponRelated")]
    public GameObject swordCase;
    public GameObject katana;
    public GameObject rightHand;
    public bool outPutSword = false;
    public Vector3 firstSwordPosition;
    public Vector3 firstSwordRotation;


    //Hook
    public GameObject hook;


    public GameObject swordEffect;


    public void RigidOnOff(int onOff)
    {

        rb.isKinematic = onOff == 1;
    }



    void Start()
    {
        speed = 10;
        jumpSpeed = 15.0f;
        gravity = 20.0f;

        MoveDir = Vector3.zero;
        rb = GetComponent<Rigidbody>();

        anim = GetComponent<Animator>();
        //lr = leftHand.GetComponent<LineRenderer>();
        myTransform = GetComponent<Transform>();
        sword = katana.GetComponent<Sword>();



    }





    RaycastHit hit;
    void Update()
    {
        if (knuckBack == true)
        {
            rb.AddForce(-transform.forward * 7f, ForceMode.Impulse);
        }



        move();
        Dodge();
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            InvokeRepeating("Change_SpeedPlus", 0.3f, 0.3f);

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            CancelInvoke("Change_SpeedPlus");
            speed = walk_Speed;
        }

        InputControl();
        //AnimationFinished();
    }

    void Change_SpeedPlus()
    {
        speed += 0.3f;

        anim.SetFloat("runWithSword", speed);
        if (speed > run_Speed)
        {
            speed = run_Speed;
            CancelInvoke("Change_SpeedPlus");
        }
    }
    void move()
    {


        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        anim.SetBool("walkWithSword", h != 0 || v != 0);
        MoveDir = Camera.main.transform.right * h + Camera.main.transform.forward * v;
        MoveDir.y = 0;
        transform.position += speed * Time.deltaTime * MoveDir;
        transform.LookAt(transform.position + MoveDir);

        //MoveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));





        if (Input.GetButtonDown("Jump") && !jumpPing)
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);

            anim.SetTrigger("isJump");
            jumpPing = true;
        }

    }


    public float w_DoubleClickSecond = 0.25f;
    private bool w_IsOneClick = false;
    private double w_Timer = 0;

    public float a_DoubleClickSecond = 0.25f;
    private bool a_IsOneClick = false;
    private double a_Timer = 0;

    public float s_DoubleClickSecond = 0.25f;
    private bool s_IsOneClick = false;
    private double s_Timer = 0;


    public float d_DoubleClickSecond = 0.25f;
    private bool d_IsOneClick = false;
    private double d_Timer = 0;


    
    void Dodge()
    {
        if (w_IsOneClick && ((Time.time - w_Timer) > w_DoubleClickSecond))
        {
            Debug.Log("One Click");
            w_IsOneClick = false;
        }

        if (a_IsOneClick && ((Time.time - a_Timer) > a_DoubleClickSecond))
        {
            Debug.Log("One Click");
            a_IsOneClick = false;
        }

        if (s_IsOneClick && ((Time.time - s_Timer) > s_DoubleClickSecond))
        {
            Debug.Log("One Click");
            s_IsOneClick = false;
        }

        if (d_IsOneClick && ((Time.time - d_Timer) > d_DoubleClickSecond))
        {
            Debug.Log("One Click");
            d_IsOneClick = false;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!w_IsOneClick)
            {
                w_Timer = Time.time;
                w_IsOneClick = true;

            }

            else if (w_IsOneClick && ((Time.time - w_Timer) < w_DoubleClickSecond))
            {
                anim.SetTrigger("doDodge");
                Debug.Log("Double Click");
                w_IsOneClick = false;
                rb.AddForce(Camera.main.transform.forward * 5f, ForceMode.Impulse);
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!a_IsOneClick)
            {
                a_Timer = Time.time;
                a_IsOneClick = true;

            }

            else if (a_IsOneClick && ((Time.time - a_Timer) < a_DoubleClickSecond))
            {
                anim.SetTrigger("doDodge");
                Debug.Log("Double Click");
                a_IsOneClick = false;
                rb.AddForce(-Camera.main.transform.right * 5f, ForceMode.Impulse);
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (!s_IsOneClick)
            {
                s_Timer = Time.time;
                s_IsOneClick = true;

            }

            else if (s_IsOneClick && ((Time.time - s_Timer) < s_DoubleClickSecond))
            {
                anim.SetTrigger("doDodge");
                Debug.Log("Double Click");
                s_IsOneClick = false;
                rb.AddForce(-Camera.main.transform.forward * 5f, ForceMode.Impulse);
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!d_IsOneClick)
            {

                d_Timer = Time.time;
                d_IsOneClick = true;

            }

            else if (d_IsOneClick && ((Time.time - d_Timer) < d_DoubleClickSecond))
            {
                anim.SetTrigger("doDodge");
                Debug.Log("Double Click");
                d_IsOneClick = false;
                rb.AddForce(Camera.main.transform.right * 5f, ForceMode.Impulse);
            }
        }
    }

    IEnumerator OutPutSword()
    {
        yield return new WaitForSeconds(0.3f);
        katana.transform.localEulerAngles = new Vector3(66f, -230f, -60f);
        katana.transform.SetParent(rightHand.transform, false);
        katana.transform.localPosition = new Vector3(0.12f, -0.136f, 0.5f);
        outPutSword = !outPutSword;
    }

    IEnumerator InputSword()
    {
        yield return new WaitForSeconds(1.3f);
        katana.transform.SetParent(swordCase.transform, false);
        katana.transform.localPosition = new Vector3(-0.1f, -0.5f, 0.1f);
        katana.transform.localEulerAngles = new Vector3(0f, -180f, 20f);
        outPutSword = false;
        anim.SetBool("inputSword", false);
    }



    void InputControl()
    {

        if (Input.GetKeyDown(KeyCode.J))
        {
            //hook.SetActive(true);
            anim.SetTrigger("loop");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            swordEffect.SetActive(true);
            if (nextAttack)
            {
                if (outPutSword == false)
                {
                    anim.SetTrigger("outPutSword");
                    StartCoroutine(OutPutSword());
                    outPutSword = true;
                }
                switch (attackState)
                {

                    case FighterAttackState.Attack1:
                        
                        anim.SetTrigger("firstAttack");
                        nextAttack = false;
                        sword.damage = 10f;
                        attackState = FighterAttackState.Attack2;
                        StartCoroutine("Delay");
                        

                        break;
                    case FighterAttackState.Attack2:
                        //swordEffect.SetActive(true);
                        anim.SetTrigger("secoundAttack");
                        nextAttack = false;
                        sword.damage = 10f;

                        attackState = FighterAttackState.Attack3;
                        StartCoroutine("Delay");
                        break;
                    case FighterAttackState.Attack3:
                        
                        anim.SetTrigger("thirdAttack");
                        nextAttack = false;
                        sword.damage = 10f;
                        attackState = FighterAttackState.Attack4;

                        StartCoroutine("Delay");
                       
                        break;

                    case FighterAttackState.Attack4:
                        //swordEffect.SetActive(true);
                        anim.SetTrigger("lastAttack");
                        StartCoroutine("JumpAttack");
                        nextAttack = false;
                        sword.damage = 10f;
                        attackState = FighterAttackState.hiddenAttack;
                        StartCoroutine("Delay");
                        
                        break;

                    case FighterAttackState.hiddenAttack:
                        //swordEffect.SetActive(true);
                        anim.SetTrigger("hiddenAttack");
                        StartCoroutine("DownAttack");
                        nextAttack = false;
                        attackState = FighterAttackState.Attack1;
                        sword.damage = 10f;
                        StartCoroutine("Delay");
                        StartCoroutine(InputSword());
                        break;
                }
            }
        }
    }
    IEnumerator Delay()
    {
        float check = 0;
        float delaytime = 0.3f;
        while (check < delaytime)
        {
            yield return null;
            check += Time.deltaTime;

        }
        check = 0;
        nextAttack = true;


    }
    IEnumerator JumpAttack()
    {
        yield return new WaitForSeconds(0.2f);
        rb.AddForce(Vector3.up * 15f, ForceMode.Impulse);
    }
    IEnumerator DownAttack()
    {
        yield return new WaitForSeconds(0.1f);
        rb.AddForce(Vector3.down * jumpSpeed, ForceMode.Impulse);
    }

    bool effectOnOff = false;
    void OffSwordEffect(int onOff) 
    {
        effectOnOff = onOff == 1;

        swordEffect.SetActive(effectOnOff);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpPing = false;
        }
    }
}
