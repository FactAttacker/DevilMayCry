using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int x_Value = 0;
    int z_Value = 0;
    //CameraTransform
    public GameObject cam;
    Transform myTransform;
    [Header("??????")]
    public float walk_Speed = 10f;
    public float speed;      // ??? ??? ???.
    public float run_Speed = 20f;
    public float jumpSpeed; // ??? ?? ?.
    bool jumpPing = false;
    public float gravity;    // ????? ???? ??.

    Rigidbody rb;
    private Vector3 MoveDir;                // ???? ???? ??.0


    [Header("???????")]

    public bool nextAttack = true;
    public enum FighterState { WithoutSowrd, SwordMode }
    public FighterState fighterState = FighterState.WithoutSowrd;
    public enum FighterAttackState { Attack1, Attack2, Attack3, Attack4 }
    public FighterAttackState attackState = FighterAttackState.Attack1;
    public Animator anim;


    [Header("????")]
    public GameObject swordCase;
    public GameObject katana;
    public GameObject rightHand;
    public bool outPutSword = false;
    public Vector3 firstSwordPosition;
    public Vector3 firstSwordRotation;


    //Hook
    public GameObject hook;



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
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.down * gravity);

    }
    RaycastHit hit;
    void Update()
    {





        move();

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

        anim.SetBool("walkWithSword", MoveDir.x != 0 || MoveDir.z != 0);

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    MoveDir = cam.transform.forward;
        //}
        //// s->뒤
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    MoveDir = -cam.transform.forward;
        //}
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    MoveDir = -cam.transform.right;
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    MoveDir = cam.transform.right;
        //}
        //MoveDir = new Vector3(transform.position.x, 0, transform.position.z);
        //transform.Translate(MoveDir *Time.deltaTime);

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        MoveDir = cam.transform.right * h + cam.transform.forward * v;

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


    void OutPutSword_Ver()
    {
        if (outPutSword == true)
        {

            if (Input.GetKeyDown(KeyCode.K))
            {
                anim.SetTrigger("throwSword");
            }
        }
    }

    IEnumerator OutPutSword()
    {
        yield return new WaitForSeconds(0.6f);
        katana.transform.localEulerAngles = new Vector3(-30f, 129f, 433f);
        katana.transform.SetParent(rightHand.transform, false);
        katana.transform.localPosition = new Vector3(0.02f, -0.77f, -1.04f);
        outPutSword = !outPutSword;
    }

    IEnumerator InputSword()
    {
        yield return new WaitForSeconds(1.3f);
        katana.transform.SetParent(swordCase.transform, false);
        katana.transform.localPosition = new Vector3(-6.32f, 0.13f, 0f);
        //katana.transform.localEulerAngles = new Vector3(124f, 187f, 370);
        outPutSword = false;
        anim.SetBool("inputSword", false);
    }



    void InputControl()
    {





        if (Input.GetKeyDown(KeyCode.J))
        {
            hook.SetActive(true);
            anim.SetTrigger("loop");
        }



        if (Input.GetKeyDown(KeyCode.L))
        {
            if (outPutSword == false)
            {
                anim.SetTrigger("outPutSword");
                StartCoroutine("OutPutSword");
                fighterState = FighterState.SwordMode;

            }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (outPutSword == true)
            {
                anim.SetBool("inputSword", outPutSword);
                StartCoroutine("InputSword");
                fighterState = FighterState.WithoutSowrd;
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            
            if (nextAttack) 
            {
                switch (attackState)
                {

                    case FighterAttackState.Attack1:
                        //print("gggggggg");

                        anim.SetTrigger("firstAttack");
                        nextAttack = false;
                        attackState = FighterAttackState.Attack2;
                        StartCoroutine("Delay");
                        //attackState = FighterAttackState.Attack2;

                        break;
                    case FighterAttackState.Attack2:

                        anim.SetTrigger("secoundAttack");
                        nextAttack = false;
                        attackState = FighterAttackState.Attack3;
                        StartCoroutine("Delay");
                        //attackState = FighterAttackState.Attack3;

                        break;
                    case FighterAttackState.Attack3:

                        anim.SetTrigger("thirdAttack");
                        nextAttack = false;
                        attackState = FighterAttackState.Attack4;
                        StartCoroutine("Delay");
                        //attackState = FighterAttackState.Attack1;

                        break;

                    case FighterAttackState.Attack4:
                        anim.SetTrigger("lastAttack");
                        rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                        nextAttack = false;
                        attackState = FighterAttackState.Attack1;
                        StartCoroutine("Delay");
                        break;
                }
            }


          
        }

        if (Input.GetKeyDown(KeyCode.K)&& jumpPing)
        {
            gravity = 0f;

            if (nextAttack)
            {
                switch (attackState)
                {

                    case FighterAttackState.Attack1:
                        //print("gggggggg");

                        anim.SetTrigger("firstAttack");
                        nextAttack = false;
                        attackState = FighterAttackState.Attack2;
                        StartCoroutine("Delay");
                        //attackState = FighterAttackState.Attack2;

                        break;
                    case FighterAttackState.Attack2:

                        anim.SetTrigger("secoundAttack");
                        nextAttack = false;
                        attackState = FighterAttackState.Attack3;
                        StartCoroutine("Delay");
                        //attackState = FighterAttackState.Attack3;

                        break;
                    case FighterAttackState.Attack3:

                        anim.SetTrigger("thirdAttack");
                        nextAttack = false;
                        attackState = FighterAttackState.Attack4;
                        StartCoroutine("Delay");
                        //attackState = FighterAttackState.Attack1;

                        break;

                    case FighterAttackState.Attack4:
                        anim.SetTrigger("lastAttack");
                        rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                        nextAttack = false;
                        attackState = FighterAttackState.Attack1;
                        StartCoroutine("Delay");
                        break;
                }
            }



        }
    }

    Coroutine delayCoroutine;
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
        //공격하고 다음 공격할때 아이들로 상태 넘긴다 넘긴 후 일정 시간이 넘어가면 attack1로 돌아간다.
        
    }

    


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpPing = false;
        }
    }
}
