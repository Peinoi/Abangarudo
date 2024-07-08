using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    private Animator animator; // 애니메이터
    private bool isGrounded = true; // 땅에 닿았는지 여부를 추적
    public float moveSpeed = 5.0f; // 전체 스피드
    public float jumpForce = 5.0f; // 점프 힘
    private Rigidbody rb; // 리지드 컴포넌트
    private Transform cameraTransform;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing from " + gameObject.name);
        }
        cameraTransform = Camera.main.transform;

        AnimClear();
    }

    void Update()
    {
        MoveShot();
        PlayerMove();
        PlayerJump();
        AutoShot();
    }

    void AnimClear()
    {
        animator.SetBool("IDLE", true);
        animator.SetBool("RUN", false);
        animator.SetBool("DIE", false);
        animator.SetBool("JUMP", false);
        animator.SetBool("SHOT", false);
        animator.SetBool("WalkFront_Shoot_AR", false);
        animator.SetBool("WalkLeft_Shoot_AR", false);
        animator.SetBool("WalkRight_Shoot_AR", false);
        animator.SetBool("WalkBack_Shoot_AR", false);
    }

    void PlayerMove()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(hAxis, 0, vAxis).normalized;

        if (inputDir != Vector3.zero)
        {
            // 카메라 방향을 기준으로 이동 방향 변환
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0;
            Vector3 cameraRight = cameraTransform.right;
            cameraRight.y = 0;
            Vector3 moveDir = (cameraForward * inputDir.z + cameraRight * inputDir.x).normalized;

            rb.velocity = new Vector3(moveDir.x * moveSpeed, rb.velocity.y, moveDir.z * moveSpeed);
            transform.rotation = Quaternion.LookRotation(moveDir);

            animator.SetBool("IDLE", false);
            animator.SetBool("RUN", true);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            animator.SetBool("RUN", false);
            animator.SetBool("IDLE", true);
        }
    }


    void PlayerJump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("JUMP");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            Invoke("JumpReset", 0.5f);
        }
    }

    void JumpReset()
    {
        //animator.SetBool("JUMP", false);
        isGrounded = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("IDLE", true);
        }
    }

    void AutoShot()
    {
        if (isGrounded && Input.GetMouseButton(0)) // 마우스 좌클릭을 누르고 있을 때
        {
            animator.SetBool("SHOT", true);
            animator.SetBool("IDLE", false);
            // 여기서 추가적인 공격 로직을 추가할 수 있습니다.
        }else if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("SHOT", false);
            animator.SetBool("IDLE", true);
        }
    }


    void MoveShot()
    {
        if (isGrounded && Input.GetKey(KeyCode.W) && Input.GetMouseButton(0))
        {
            animator.SetBool("WalkFront_Shoot_AR", true);
            animator.SetBool("WalkLeft_Shoot_AR", false);
            animator.SetBool("WalkRight_Shoot_AR", false);
            animator.SetBool("WalkBack_Shoot_AR", false);
            animator.SetBool("RUN", false);
            animator.SetBool("IDLE", false);
        }
        else if (isGrounded && Input.GetKey(KeyCode.A) && Input.GetMouseButton(0))
        {
            animator.SetBool("WalkFront_Shoot_AR", false);
            animator.SetBool("WalkLeft_Shoot_AR", true);
            animator.SetBool("WalkRight_Shoot_AR", false);
            animator.SetBool("WalkBack_Shoot_AR", false);
            animator.SetBool("RUN", false);
            animator.SetBool("IDLE", false);
        }
        else if (isGrounded && Input.GetKey(KeyCode.D) && Input.GetMouseButton(0))
        {
            animator.SetBool("WalkFront_Shoot_AR", false);
            animator.SetBool("WalkLeft_Shoot_AR", false);
            animator.SetBool("WalkRight_Shoot_AR", true);
            animator.SetBool("WalkBack_Shoot_AR", false);
            animator.SetBool("RUN", false);
            animator.SetBool("IDLE", false);
        }
        else if (isGrounded && Input.GetKey(KeyCode.S) && Input.GetMouseButton(0))
        {
            animator.SetBool("WalkFront_Shoot_AR", false);
            animator.SetBool("WalkLeft_Shoot_AR", false);
            animator.SetBool("WalkRight_Shoot_AR", false);
            animator.SetBool("WalkBack_Shoot_AR", true);
            animator.SetBool("RUN", false);
            animator.SetBool("IDLE", false);
        }
        else
        {
            animator.SetBool("WalkFront_Shoot_AR", false);
            animator.SetBool("WalkLeft_Shoot_AR", false);
            animator.SetBool("WalkRight_Shoot_AR", false);
            animator.SetBool("WalkBack_Shoot_AR", false);
            animator.SetBool("IDLE", true);
        }
    }
}
