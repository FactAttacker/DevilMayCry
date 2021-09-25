using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Script : MonoBehaviour
{
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

    public bool nextAttack = false;
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

    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.down * gravity);

    }
    RaycastHit hit;
    void Update()
    {



        //indicator.transform.position += -Vector3.forward;

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


        MoveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.position += speed * Time.deltaTime * MoveDir;

        transform.LookAt(transform.position + MoveDir);


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
        katana.transform.localEulerAngles = new Vector3(208.191f, -46f, -85f);
        katana.transform.SetParent(rightHand.transform, false);
        katana.transform.localPosition = new Vector3(-0.068f, -0.021f, -0.131f);
        outPutSword = !outPutSword;
    }

    IEnumerator InputSword()
    {
        yield return new WaitForSeconds(1.3f);
        katana.transform.SetParent(swordCase.transform, false);
        katana.transform.localPosition = new Vector3(-0.25f, 0.15f, 0.29f);
        katana.transform.localEulerAngles = new Vector3(124f, 187f, 730);
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
            switch (attackState)
            {
                case FighterAttackState.Attack1:
                    attackState = FighterAttackState.Attack2;
                    anim.SetTrigger("firstAttack");
                    //anim.SetBool("firstAttack", false);
                    break;
                case FighterAttackState.Attack2:
                    attackState = FighterAttackState.Attack3;
                    anim.SetTrigger("secoundAttack");
                    break;
                case FighterAttackState.Attack3:
                    attackState = FighterAttackState.Attack1;
                    anim.SetTrigger("thirdAttack");
                    break;
            }
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpPing = false;
        }
    }

}