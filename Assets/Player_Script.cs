using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Script : MonoBehaviour
{
    [Header("이동관련속성")]
    public float walk_Speed = 10f;
    public float speed;      // 캐릭터 움직임 스피드.
    public float run_Speed = 20f;
    public float jumpSpeed; // 캐릭터 점프 힘.
    public float gravity;    // 캐릭터에게 작용하는 중력.
    private CharacterController controller; // 현재 캐릭터가 가지고있는 캐릭터 컨트롤러 콜라이더.
    private Vector3 MoveDir;                // 캐릭터의 움직이는 방향.0

    [Header("캐릭터공격상태")]
    public FighterAttackState attackState = FighterAttackState.Attack1;
    public bool nextAttack = false;
    public enum FighterAttackState {Attack1, Attack2,Attack3, Attack4 }
    //public GameObject anim_child;
    Animator anim;


    [Header("칼 , 손")]
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
        // 현재 캐릭터가 땅에 있는가?
        if (controller.isGrounded)
        {
            // 위, 아래 움직임 셋팅. 
           
            MoveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            // 벡터를 로컬 좌표계 기준에서 월드 좌표계 기준으로 변환한다.
            //MoveDir = transform.TransformDirection(MoveDir);

            // 스피드 증가.
            
           MoveDir *= speed;
           

            transform.LookAt(transform.position + MoveDir);

            // 캐릭터 점프
            if (Input.GetButton("Jump")) 
            {
                
                MoveDir.y = jumpSpeed;
                anim.SetTrigger("isJump");
            }

           
                

        }

        // 캐릭터에 중력 적용.
        MoveDir.y -= gravity * Time.deltaTime;
       // transform.LookAt(transform.position + MoveDir);
        // 캐릭터 움직임.
        controller.Move(MoveDir * Time.deltaTime);

        anim.SetBool("isWalk", MoveDir.x != 0 || MoveDir.z != 0 );
        print(MoveDir);
        //캐릭터 에니메이션
       
        
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
