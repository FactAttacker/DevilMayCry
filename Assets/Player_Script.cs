using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Script : MonoBehaviour
{
    [Header("�̵����üӼ�")]
    public float walk_Speed = 10f;
    public float speed;      // ĳ���� ������ ���ǵ�.
    public float run_Speed = 20f;
    public float jumpSpeed; // ĳ���� ���� ��.
    public float gravity;    // ĳ���Ϳ��� �ۿ��ϴ� �߷�.
    private CharacterController controller; // ���� ĳ���Ͱ� �������ִ� ĳ���� ��Ʈ�ѷ� �ݶ��̴�.
    private Vector3 MoveDir;                // ĳ������ �����̴� ����.0

    [Header("ĳ���Ͱ��ݻ���")]
    public FighterAttackState attackState = FighterAttackState.Attack1;
    public bool nextAttack = false;
    public enum FighterAttackState {Attack1, Attack2,Attack3, Attack4 }
    //public GameObject anim_child;
    Animator anim;


    [Header("Į , ��")]
    public GameObject katana;
    public GameObject rightHand;
    public bool inputSword = false;


    void Start()  
    {
        speed = 10;
        jumpSpeed = 5.0f;
        gravity = 20.0f;

        MoveDir = Vector3.zero;
        
        controller = GetComponent<CharacterController>();


        anim = GetComponent<Animator>();
    }

    void Update()
    {
        move();
        if (inputSword == true) 
        {
            katana.transform.position = rightHand.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            InvokeRepeating("Change_SpeedPlus", 0.3f,0.3f);
            
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) 
        {
            CancelInvoke("Change_SpeedPlus");
            speed = walk_Speed;
        }

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
        if (Input.GetKeyDown(KeyCode.L))
        {
            inputSword = !inputSword;
            anim.SetTrigger("isOutputSword");
        }
        if (Input.GetKeyDown(KeyCode.K)) 
        {
            anim.SetTrigger("throwSword");
        }
        // ���� ĳ���Ͱ� ���� �ִ°�?
        if (controller.isGrounded)
        {
            // ��, �Ʒ� ������ ����. 
           
            MoveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            // ���͸� ���� ��ǥ�� ���ؿ��� ���� ��ǥ�� �������� ��ȯ�Ѵ�.
            //MoveDir = transform.TransformDirection(MoveDir);

            // ���ǵ� ����.
            
           MoveDir *= speed;
           

            transform.LookAt(transform.position + MoveDir);

            // ĳ���� ����
            if (Input.GetButton("Jump")) 
            {
                
                MoveDir.y = jumpSpeed;
                anim.SetTrigger("isJump");
            }

           
                

        }

        // ĳ���Ϳ� �߷� ����.
        MoveDir.y -= gravity * Time.deltaTime;
       // transform.LookAt(transform.position + MoveDir);
        // ĳ���� ������.
        controller.Move(MoveDir * Time.deltaTime);

        anim.SetBool("isWalk", MoveDir.x != 0 || MoveDir.z != 0 );
        print(MoveDir);
        //ĳ���� ���ϸ��̼�
       
        
    }

    void NormalAttack_Q() 
    {
        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            switch (attackState) 
            {
               // case FighterAttackState.Attack1:
            }
        }
    }
}
