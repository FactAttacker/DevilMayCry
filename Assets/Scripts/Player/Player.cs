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
       
        rb.isKinematic = onOff==1;
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
    public float m_DoubleClickSecond = 0.25f;
    private bool m_IsOneClick = false;
    private double m_Timer = 0;

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

        if (m_IsOneClick && ((Time.time - m_Timer) > m_DoubleClickSecond))
        {
            Debug.Log("One Click");
            m_IsOneClick = false;
        }

        


        if (Input.GetButtonDown("Jump") && !jumpPing)
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);

            anim.SetTrigger("isJump");
            jumpPing = true;
        }

    }

    
    void Dodge() 
    {
        //bool input = Input.GetKeyDown(KeyCode.W);
        //switch(input) 
        //{

        //}
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!m_IsOneClick)
            {
                m_Timer = Time.time;
                m_IsOneClick = true;

            }

            else if (m_IsOneClick && ((Time.time - m_Timer) < m_DoubleClickSecond))
            {
                anim.SetTrigger("doDodge");
                Debug.Log("Double Click");
                m_IsOneClick = false;
                rb.AddForce(Camera.main.transform.forward * 5f, ForceMode.Impulse);
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!m_IsOneClick)
            {
                m_Timer = Time.time;
                m_IsOneClick = true;

            }

            else if (m_IsOneClick && ((Time.time - m_Timer) < m_DoubleClickSecond))
            {
                anim.SetTrigger("doDodge");
                Debug.Log("Double Click");
                m_IsOneClick = false;
                rb.AddForce(-Camera.main.transform.right * 5f, ForceMode.Impulse);
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!m_IsOneClick)
            {
                m_Timer = Time.time;
                m_IsOneClick = true;

            }

            else if (m_IsOneClick && ((Time.time - m_Timer) < m_DoubleClickSecond))
            {
                anim.SetTrigger("doDodge");
                Debug.Log("Double Click");
                m_IsOneClick = false;
                rb.AddForce(Camera.main.transform.right * 5f, ForceMode.Impulse);
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (!m_IsOneClick)
            {
               
                m_Timer = Time.time;
                m_IsOneClick = true;

            }

            else if (m_IsOneClick && ((Time.time - m_Timer) < m_DoubleClickSecond))
            {
                anim.SetTrigger("doDodge");
                Debug.Log("Double Click");
                m_IsOneClick = false;
                rb.AddForce(-Camera.main.transform.forward * 5f, ForceMode.Impulse);
            }
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
        katana.transform.localEulerAngles = new Vector3(66f, -230f, -60f);
        katana.transform.SetParent(rightHand.transform, false);
        katana.transform.localPosition = new Vector3(0.12f, -0.136f, 0.5f);
        outPutSword = !outPutSword;
    }

    IEnumerator InputSword()
    {
        yield return new WaitForSeconds(1.3f);
        katana.transform.SetParent(swordCase.transform, false);
        katana.transform.localPosition = new Vector3(-0.41f, 0.76f, 1.92f);
        katana.transform.localEulerAngles = new Vector3(0f, -180f, 0f);
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



        if (Input.GetKeyDown(KeyCode.L))
        {
            if (outPutSword == false)
            {
                anim.SetTrigger("outPutSword");
                StartCoroutine("OutPutSword");
                fighterState = FighterState.SwordMode;

            }
            else if (outPutSword == true)
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
                        //swordEffect.SetActive(true);
                        anim.SetTrigger("firstAttack");
                        nextAttack = false;
                        sword.damage = 10f;
                        attackState = FighterAttackState.Attack2;

                        
                        StartCoroutine("Delay");
                        //attackState = FighterAttackState.Attack2;
                        //swordEffect.SetActive(false);
                        break;
                    case FighterAttackState.Attack2:
                        //swordEffect.SetActive(true);
                        anim.SetTrigger("secoundAttack");
                        nextAttack = false;
                        sword.damage = 10f;

                        attackState = FighterAttackState.Attack3;
                        StartCoroutine("Delay");
                        //attackState = FighterAttackState.Attack3;
                        //swordEffect.SetActive(false);
                        break;
                    case FighterAttackState.Attack3:
                        //swordEffect.SetActive(true);
                        anim.SetTrigger("thirdAttack");
                        nextAttack = false;
                        sword.damage = 10f;
                        attackState = FighterAttackState.Attack4;
                       
                        StartCoroutine("Delay");
                        //attackState = FighterAttackState.Attack1;
                        //swordEffect.SetActive(false);
                        break;

                    case FighterAttackState.Attack4:
                        //swordEffect.SetActive(true);
                        anim.SetTrigger("lastAttack");
                        StartCoroutine("JumpAttack");
                        nextAttack = false;
                        sword.damage = 10f;
                        attackState = FighterAttackState.hiddenAttack;
                        StartCoroutine("Delay");
                        // print("Attack4");
                        //swordEffect.SetActive(false);
                        break;

                    case FighterAttackState.hiddenAttack:
                        //swordEffect.SetActive(true);
                        anim.SetTrigger("hiddenAttack");
                        StartCoroutine("DownAttack");
                        nextAttack = false;
                        attackState = FighterAttackState.Attack1;
                        sword.damage = 10f;
                        StartCoroutine("Delay");
                       // swordEffect.SetActive(false);
                        //anim.ti
                        //Animation myAnim = anim.time;
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
        //RigidOnOff(0);
        

    }




    IEnumerator JumpAttack() 
    {
        yield return new WaitForSeconds(0.4f);
        rb.AddForce(Vector3.up * 12f, ForceMode.Impulse);


    }
    IEnumerator DownAttack() 
    {
        yield return new WaitForSeconds(0.3f);
        rb.AddForce(Vector3.down * jumpSpeed, ForceMode.Impulse);
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpPing = false;
        }
    }
}
