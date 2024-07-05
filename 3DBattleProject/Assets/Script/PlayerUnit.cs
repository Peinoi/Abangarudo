using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    private Animator animator;
    private bool isGrounded = true; // 땅에 닿았는지 여부를 추적
    public float moveSpeed = 5.0f;
    private Rigidbody rb;
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        AnimClear();
    }

    void Update()
    {
        PlayerMove();
        PlayerJump();
    }

    void AnimClear()
    {
        animator.SetBool("IDLE", true);
        animator.SetBool("RUN", false);
        animator.SetBool("DIE", false);
        animator.SetBool("JUMP", false);
    }


    void PlayerMove()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(hAxis, 0, vAxis).normalized;

        if (inputDir != Vector3.zero)
        {
            rb.velocity = new Vector3(inputDir.x * moveSpeed, rb.velocity.y, inputDir.z * moveSpeed);
            transform.rotation = Quaternion.LookRotation(inputDir);

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
         
            animator.SetBool("JUMP", true);
            isGrounded = false;
            Invoke("JumpReset", 1f);        }
       
    }

   void JumpReset()
    {
        animator.SetBool("JUMP", false);
        isGrounded = true;
    }
}
