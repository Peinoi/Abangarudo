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
    public float v; // Vertical
    public float h; // Horizontal
    private Rigidbody rb; // 리지드 컴포넌트
    private Transform cameraTransform;
    Vector3 movement;
    private int mode = 0;

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
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");
        Cursor.lockState = CursorLockMode.Locked;
        PlayerJump();
        AutoShot();
        ChangeMode();
        PlayerMove();
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
        if (mode == 0)
        {
            float hAxis = Input.GetAxisRaw("Horizontal");
            float vAxis = Input.GetAxisRaw("Vertical");
            Vector3 inputDir = new Vector3(hAxis, 0, vAxis).normalized;

            if (inputDir != Vector3.zero)
            {
                // 플레이어의 이동 방향을 카메라의 회전에 맞춤
                Vector3 moveDirection = cameraTransform.TransformDirection(inputDir);
                moveDirection.y = 0; // 수직 방향은 무시

                // 플레이어 이동
                rb.velocity = moveDirection * moveSpeed;
                transform.rotation = Quaternion.LookRotation(moveDirection);

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

        if (mode == 1)
        {
            MoveShot();
        }
    }

    void Zoom(float h, float v)
    {
        Vector3 inputDir = new Vector3(h, 0, v).normalized;

        // 이동 방향을 카메라의 회전에 맞춤
        Vector3 moveDirection = cameraTransform.TransformDirection(inputDir);
        moveDirection.y = 0; // 수직 방향은 무시

        movement = moveDirection * moveSpeed * Time.deltaTime;

        rb.MovePosition(transform.position + movement);
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
            animator.SetBool("RUN", false);
            // 여기서 추가적인 공격 로직을 추가할 수 있습니다.
        }
        else if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("SHOT", false);
            animator.SetBool("IDLE", true);
            animator.SetBool("RUN", false);
        }
    }


    void MoveShot()
    {
        Zoom(h, v);
        if (isGrounded && Input.GetKey(KeyCode.W))
        {
            animator.SetBool("WalkFront_Shoot_AR", true);
            animator.SetBool("WalkLeft_Shoot_AR", false);
            animator.SetBool("WalkRight_Shoot_AR", false);
            animator.SetBool("WalkBack_Shoot_AR", false);
            animator.SetBool("RUN", false);
            animator.SetBool("IDLE", false);
        }
        else if (isGrounded && Input.GetKey(KeyCode.A))
        {
            animator.SetBool("WalkFront_Shoot_AR", false);
            animator.SetBool("WalkLeft_Shoot_AR", true);
            animator.SetBool("WalkRight_Shoot_AR", false);
            animator.SetBool("WalkBack_Shoot_AR", false);
            animator.SetBool("RUN", false);
            animator.SetBool("IDLE", false);
        }
        else if (isGrounded && Input.GetKey(KeyCode.D))
        {
            animator.SetBool("WalkFront_Shoot_AR", false);
            animator.SetBool("WalkLeft_Shoot_AR", false);
            animator.SetBool("WalkRight_Shoot_AR", true);
            animator.SetBool("WalkBack_Shoot_AR", false);
            animator.SetBool("RUN", false);
            animator.SetBool("IDLE", false);
        }
        else if (isGrounded && Input.GetKey(KeyCode.S))
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

    void ChangeMode()
    {
        if (Input.GetMouseButtonDown(1) && mode == 0)
        {
            mode = 1;
            animator.SetBool("IDLE2", true);
            animator.SetBool("IDLE", false);
            animator.SetBool("RUN", false);
            Debug.Log(mode);
        }
        else if (Input.GetMouseButtonDown(1) && mode == 1)
        {
            mode = 0;
            animator.SetBool("IDLE2", false);
            animator.SetBool("WalkFront_Shoot_AR", false);
            animator.SetBool("WalkLeft_Shoot_AR", false);
            animator.SetBool("WalkRight_Shoot_AR", false);
            animator.SetBool("WalkBack_Shoot_AR", false);
            animator.SetBool("IDLE", true);
            Debug.Log(mode);
        }
    }
}
