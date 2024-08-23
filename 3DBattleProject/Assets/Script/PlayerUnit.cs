using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class PlayerUnit : MonoBehaviourPunCallbacks
{
    private Animator animator; // 애니메이터
    private bool isGrounded = true; // 땅에 닿았는지 여부를 추적
    public float moveSpeed = 5.0f; // 전체 스피드
    public float jumpForce = 5.0f; // 점프 힘
    public float v; // Vertical
    public float h; // Horizontal
    private Rigidbody rb; // 리지드 컴포넌트
    private Transform cameraTransform;
    public Transform firstPersonCamera;

    Vector3 movement;
    private int mode = 0;

    [Header("총")]
    public GameObject[] bullet;
    public float maxBullet = 100f; //최대 총탄
    float currentBullet; //현재 총탄
    public float fireDamp; // 연사 지연 시간
    float currentDamp;
    public float reloadTime; //재장전 시간
    bool isReload = false; // 재장전 판단 변수
    public int reloadItem = 0;
    public Transform firePos; // 총구
        
    [Header("FPS")]
    //1인칭 사격 모드
    public Camera personCam;
    public float distance = 10f;
    bool person = false;
    //피격체 데이터
    RaycastHit hitData;
    // 목표 지점으로 총알을 발사
    Vector3 targetPoint;

    [Header("Sound")]
    [SerializeField] AudioSource player_Audio;
    [SerializeField] AudioSource gun_Audio;
    [SerializeField] private AudioClip[] audioAttack;

    private void Start()
    {
        if (personCam == null)
        {
            Debug.LogError("카메라가 설정되지 않았습니다.");
        }
        if (!photonView.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
        }

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing from " + gameObject.name);
        }
        cameraTransform = Camera.main.transform;

        // 총
        currentBullet = maxBullet;
        currentDamp = 0;

        AnimClear();


        UIManager.instance.bulletText.text = currentBullet + " / " + maxBullet;
        UIManager.instance.reloadItem.text = reloadItem.ToString();
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");
        Cursor.lockState = CursorLockMode.Locked;
        PlayerJump();
        AutoShot();
        ChangeMode();
        PlayerMove();

        currentDamp -= Time.deltaTime;
        if (Input.GetMouseButton(0) && mode == 1)
        {
            BulletShot();
        }
        if (Input.GetKeyDown(KeyCode.R) && currentBullet < maxBullet && !isReload)
        {
            isReload = true;
            StartCoroutine(ReloadBullet());
            gun_Audio.clip = audioAttack[1];
            gun_Audio.Play();
            Debug.Log(gun_Audio.clip.name);
            animator.SetBool("Reload", true);
        }

       



        BulletLine();
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
            bool isMoving = inputDir != Vector3.zero;
            if (isMoving)
            {
                // 플레이어의 이동 방향을 카메라의 회전에 맞춤
                Vector3 moveDirection = cameraTransform.TransformDirection(inputDir);
                moveDirection.y = 0; // 수직 방향은 무시

                // 플레이어 이동
                Vector3 velocity = moveDirection * moveSpeed;
                velocity.y = rb.velocity.y; // 기존 y 속도 유지 (점프 등)
                rb.velocity = velocity;
                transform.rotation = Quaternion.LookRotation(moveDirection);

                animator.SetBool("IDLE", false);
                animator.SetBool("RUN", true);

                if (!player_Audio.isPlaying)
                {
                    player_Audio.Play(); // 이동이 시작될 때만 재생
                }
            }
            else
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
                animator.SetBool("RUN", false);
                animator.SetBool("IDLE", true);

                if (player_Audio.isPlaying)
                {
                    player_Audio.Stop(); // 이동이 멈추면 재생 중지
                }
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
        Vector3 moveDirection = firstPersonCamera.TransformDirection(inputDir);
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
        isGrounded = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("IDLE", true);
        }
        

        if (collision.gameObject.CompareTag("Bullet"))
        {
            gun_Audio.clip = audioAttack[2];
            gun_Audio.Play();
            if (UIManager.instance.hp.value > 0)
            {
                UIManager.instance.hp.value -= 100;
                UIManager.instance.hp_Txt.text = UIManager.instance.hp.value.ToString();
                if (UIManager.instance.hp.value <= 0)
                {
                    this.gameObject.SetActive(false);
                    Debug.Log("Fail");
                }
                Debug.Log("Damage");
            }
            else
            {
                this.gameObject.SetActive(false);
                Debug.Log("Fail");
            }
            
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

    void BulletShot()
    {
        if (currentDamp <= 0 && currentBullet > 0 && !isReload)
        {
            currentDamp = fireDamp;
            currentBullet--;
            UIManager.instance.bulletText.text = currentBullet + " / " + maxBullet;
            if(hitData.collider != null)
            {
                if (hitData.collider.tag == "Enmey")
                {
                    targetPoint = hitData.point;
                }
            }      
            else
            {
                targetPoint = personCam.transform.position + personCam.transform.forward * distance; // 레이캐스트가 적중하지 않은 경우
            }

            Vector3 direction = (targetPoint - firePos.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);
            
                gun_Audio.clip = audioAttack[0];
                gun_Audio.Play();
                Debug.Log(gun_Audio.isPlaying + "쀼슝빠슝");
            
            
            Instantiate(bullet[0], firePos.position, rotation);
            
        }
        
    }

    IEnumerator ReloadBullet()
    {
        
        for (float i = reloadTime; i > 0; i -= 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
        }
        isReload = false;
        currentBullet = maxBullet;
        UIManager.instance.bulletText.text = currentBullet + " / " + maxBullet;
        animator.SetBool("Reload", false);
    }



    void BulletLine()
    {
        // 오른쪽 마우스 버튼이 눌렸을 때 토글
        if (Input.GetMouseButtonDown(1))
        {
            person = !person;
        }

        // 사격 모드가 활성화되었을 때만 실행
        if (person && personCam != null)
        {
            Vector3 rayOrigin = personCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            Vector3 rayDir = personCam.transform.forward;

            // 레이캐스트
            if (Physics.Raycast(rayOrigin, rayDir, out hitData, distance))
            {

                Debug.Log("레이 발사~~");
                Debug.DrawRay(rayOrigin, rayDir * distance, Color.red);
                targetPoint = hitData.point; // 레이캐스트가 적중한 지점
                if (hitData.collider.tag == "Enemy")
                {

                    Debug.Log("적개체 발견");
                }
            }
        }
        else { Debug.Log("레이 작동 안함?"); }
    }
}
