using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject chargingCollider;
    //att
    float damage_Right = 300f;
    float damage_Left = 300f;
    float damage_Forward = 300f;
    float damage_Up =200f;
    float damage_Down = 300f;
    //KnuckBackrelated
    public bool knuckBack = false;
    public bool flyingBack = false;
    public bool flying = false;
    public Transform boss;

    Sword sword;

    public Transform danteToe;

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
    public GameObject swordCollider;
    public bool outPutSword = false;
    public Vector3 firstSwordPosition;
    public Vector3 firstSwordRotation;
    public GameObject swordEffect;

    //Hook
    public GameObject hook;
    Vector3 fixPosition;


    //AnimationEvent
    public void FlyingBack() 
    {
        VoiceSoundManager.instatnce.OnDanteVoice("Dante-1");
        transform.LookAt(boss);
        rb.AddForce(-transform.forward * 30f,ForceMode.Impulse);
    }
    public void Flying(float knucktime) 
    {
        anim.SetBool("MoveAttack", false);
        if (flying && states.die == false) 
        {
            CancelInvoke(nameof(EndFlying));

        }
        flying = true;
        //print(flying);
        Invoke(nameof(EndFlying), knucktime);
        //print(knucktime);
    }

    public void EndFlying() 
    {
        flying = false;
    }


    PlayerState states;
    public AudioClip[] dante_Audio;

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
        sword = swordCollider.GetComponent<Sword>();

        states = GetComponent<PlayerState>();

        boss = GameObject.Find("Boss_Perderos").transform;
        fixPosition= new Vector3(1, 0, 1);
        //dante_Audio = VoiceSoundManager.instatnce.OnDanteVoice("Dante-2"), VoiceSoundManager.instatnce.OnDanteVoice("Dante-3"),VoiceSoundManager.instatnce.OnDanteVoice("Dante-2");
    }

   
    RaycastHit hit;
    //public bool death = false;
    void Update()
    {
        //bool death = false;
        if (states.die == true) 
        {
            anim.SetTrigger("Death");
            //anim.ResetTrigger("Death");
            //death = false;
        }
        if (knuckBack == true && states.die == false)
        {
            
            VoiceSoundManager.instatnce.OnDanteVoice("Dante-2");
            knuckBack = false;
            transform.LookAt(boss);
            anim.SetTrigger("KnuckBack");
            rb.AddForce(-transform.forward * 7f, ForceMode.Impulse);
        }

        if (flyingBack == true && states.die == false) 
        {
            flyingBack = false;
            anim.SetTrigger("flyingBack");

        }
        if ((GameManager.instance == null || GameManager.instance.isBattle|| GameManager.instance.isPause) && flying == false && charging == false && states.die == false)
        {
            move();
            if (jumpPing == false) 
            {
                Dodge();
            }
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

            
        }
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
    public float h;
    float v;
    void move()
    {


        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
       
        MoveDir = Camera.main.transform.right * h + Camera.main.transform.forward * v;
        MoveDir.y = 0;
        transform.position += speed * Time.deltaTime * MoveDir;
        transform.LookAt(transform.position + MoveDir);


        anim.SetBool("walkWithSword", h != 0 || v != 0);
       
        //MoveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (Input.GetButtonDown("Jump") && !jumpPing)
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);

            anim.SetBool("isJump",true);
            anim.SetTrigger("jumping");
            jumpPing = true;
            Debug.Log(transform.position.y);
        }

        if (transform.position.y < -1f)
        {
            transform.position = new Vector3(transform.position.x, 0.2f, transform.position.z);
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
        yield return new WaitForSeconds(0.2f);
        katana.transform.localEulerAngles = new Vector3(66f, -230f, -60f);
        katana.transform.SetParent(rightHand.transform, false);
        katana.transform.localPosition = new Vector3(0.12f, -0.136f, 0.5f);
        outPutSword = !outPutSword;
    }

    IEnumerator InputSword()
    {
        yield return null;
        katana.transform.SetParent(swordCase.transform, false);
        katana.transform.localPosition = new Vector3(0.01705508f, -0.4062368f, -0.143f);
        katana.transform.localEulerAngles = new Vector3(0f, -180f, 20f);
        outPutSword = false;
        anim.SetBool("inputSword", false);
    }


    void ResetAttackState() 
    {
        //StartCoroutine(InputSword());
        attackState = FighterAttackState.Attack1;

        rb.AddForce(Vector3.down * 6f, ForceMode.Impulse);
        //if (outPutSword == true)
        //{
        //    anim.ResetTrigger("outPutSword");
        //}
    }
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
            anim.SetTrigger("CharginAttack");
            charging = false;
        }
        if (flying == true)
        {
            chargetime = 0f;
            charging = false;
        }
    }
    bool charging = false;
    void InputControl()
    {
        anim.SetBool("MoveAttack", v != 0 || h != 0);

        if (!flying)
        {
            if (sword == true) 
            {
                if (Input.GetKeyDown(KeyCode.L))
                {
                    charging = true;
                    //transform.LookAt(boss.transform.position);
                    anim.SetBool("Charging", true);
                    InvokeRepeating("ChargeTime", 0.1f, 0.1f);
                    //anim.SetTrigger("CharginAttack");
                    //print(chargetime);  
                   
                }
                if (Input.GetKeyUp(KeyCode.L))
                {
                    charging = false;
                    CancelInvoke("ChargeTime");
                    anim.SetBool("Charging", false);
                    chargetime = 0f;
                }
            }
            


            if (Input.GetKeyDown(KeyCode.J))
            {
                
                //hook.SetActive(true);
                //VoiceSoundManager.instatnce.OnDanteVoice("Dante-HoHou");
                anim.SetTrigger("loop");

            }
            if (Input.GetKeyDown(KeyCode.K))
            {

                swordEffect.SetActive(true);
                swordCollider.SetActive(true);
                if (nextAttack)
                {

                    switch (attackState)
                    {
                        case FighterAttackState.Attack1:

                            if (outPutSword == true)
                            {
                                anim.SetTrigger("firstAttack");
                                //VoiceSoundManager.instatnce.OnDanteVoice("Dante-0");
                                //print(outPutSword);
                            }
                            else if (outPutSword == false)
                            {
                                anim.SetTrigger("outPutSword");
                                StartCoroutine(OutPutSword());

                            }
                            //Sound
                            //VoiceSoundManager.instatnce.OnDanteVoice("Dante-0");
                            SwordSound.instatnce.OnSwordSound(0);
                            //Damage
                            sword.damage = damage_Right;
                            //Attack Change
                            nextAttack = false;
                            attackState = FighterAttackState.Attack2;
                            StartCoroutine(Delay());
                            Invoke(nameof(ResetAttackState), 1f);

                            break;

                        case FighterAttackState.Attack2:
                            //VoiceSoundManager.instatnce.OnDanteVoice("Dante-0");
                            SwordSound.instatnce.OnSwordSound(0);
                            CancelInvoke(nameof(ResetAttackState));
                            anim.SetTrigger("secoundAttack");
                            nextAttack = false;
                            sword.damage = damage_Left;
                            attackState = FighterAttackState.Attack3;
                            StartCoroutine(Delay());

                            Invoke(nameof(ResetAttackState), 1f);

                            break;
                        case FighterAttackState.Attack3:
                            //VoiceSoundManager.instatnce.OnDanteVoice("Dante-Chua");
                            SwordSound.instatnce.OnSwordSound(0);
                            CancelInvoke(nameof(ResetAttackState));
                            anim.SetTrigger("thirdAttack");
                            nextAttack = false;
                            sword.damage = damage_Forward;
                            attackState = FighterAttackState.Attack4;
                            StartCoroutine(Delay());
                            Invoke(nameof(ResetAttackState), 1f);

                            break;

                        case FighterAttackState.Attack4:
                            anim.SetBool("MoveAttack", false);
                            //VoiceSoundManager.instatnce.OnDanteVoice("Dante-Haaaaaaaa");
                            SwordSound.instatnce.OnSwordSound(1);
                            CancelInvoke(nameof(ResetAttackState));
                            anim.SetTrigger("lastAttack");
                            StartCoroutine("JumpAttack");
                            nextAttack = false;
                            sword.damage = damage_Up;
                            attackState = FighterAttackState.hiddenAttack;
                            StartCoroutine(Delay());
                            Invoke(nameof(ResetAttackState), 1f);

                            break;

                        case FighterAttackState.hiddenAttack:
                            if (CameraManager.instance != null) CameraManager.instance.currentCamera = CameraManager.CameraType.ZOOM_ATT_DOWN;
                            SwordSound.instatnce.OnSwordSound(2);
                            //VoiceSoundManager.instatnce.OnDanteVoice("Dante-Down");
                            anim.SetBool("MoveAttack", false);
                            CancelInvoke(nameof(ResetAttackState));
                            anim.SetTrigger("hiddenAttack");
                            StartCoroutine("DownAttack");
                            nextAttack = false;
                            attackState = FighterAttackState.Attack1;
                            sword.damage = damage_Down;
                            //StartCoroutine("Delay");
                            StartCoroutine(Delay(0.8f));
                            //StartCoroutine(InputSword());
                            outPutSword = false;
                            //Invoke(nameof(ResetAttackState), 1f);

                            break;
                    }
                }
            }
            //anim.SetBool("MoveAttack", h != 0 || v != 0);
        }


    }
    IEnumerator Delay(float delaytime = 0.3f)
    {
        float check = 0;
        //float delaytime = 0.3f;
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
        rb.AddForce(Vector3.up * 25f, ForceMode.Impulse);
    }
    IEnumerator DownAttack()
    {
        yield return new WaitForSeconds(0.1f);
        rb.AddForce(Vector3.down * (jumpSpeed+15f), ForceMode.Impulse);
    }

    bool effectOnOff = false;
    void OffSwordEffect(int onOff) 
    {
        effectOnOff = onOff == 1;

        swordEffect.SetActive(effectOnOff);
        swordCollider.SetActive(effectOnOff);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpPing = false;
            anim.SetBool("isJump", false);
        }
    }
}
