using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject player;
    public Transform playerHead;  // 플레이어 머리 위치
    public float cameraSpeed = 10.0f;       // 카메라의 속도
    public float rotationSpeed = 5.0f;      // 마우스 회전 속도

    private float yaw = 0.0f;               // 마우스 yaw (좌우 회전)
    private float pitch = 0.0f;            // 마우스 pitch (상하 회전)
    private bool isFirstPerson = false;

    // 기본 오프셋 값
    public Vector3 thirdPersonOffset = new Vector3(0.0f, 1f, -2f);
    public Vector3 firstPersonOffset = new Vector3(0.0f, 1.5f, 0.5f);
    private Vector3 currentOffset;

    int mouse = 1;
    public GameObject playerunit;
    public GameObject openTarget;
    void Start()
    {
        mainCamera.enabled = true;
        currentOffset = thirdPersonOffset;
        openTarget.SetActive(false);
        // 초기 yaw와 pitch 설정
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    void Update()
    {
        if (mouse == 1)
        {
            // 마우스 입력을 감지하여 yaw와 pitch 업데이트
            yaw += Input.GetAxis("Mouse X") * rotationSpeed;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;

            // pitch 값을 제한하여 카메라가 위/아래로 너무 많이 회전하지 않도록 함
            pitch = Mathf.Clamp(pitch, -45f, 45f);
        }

        // 마우스 오른쪽 버튼 클릭 시 1인칭/3인칭 모드 전환
/*        if (Input.GetMouseButtonDown(1))
        {
            if (isFirstPerson)
            {
                currentOffset = thirdPersonOffset;
                openTarget.SetActive(false);
            }
            else
            {
                openTarget.SetActive(true);
                currentOffset = firstPersonOffset;
                transform.position = playerHead.position; // 1인칭 모드로 전환 시 카메라 위치를 플레이어 머리 위치로 설정
            }

            isFirstPerson = !isFirstPerson;
        }*/
    }

    void LateUpdate()
    {
        if (isFirstPerson)
        {
            // 1인칭 모드일 때 카메라 위치를 플레이어 머리 위치로 설정
           // transform.position = playerHead.position;
           // transform.rotation = Quaternion.Euler(pitch, yaw, 0);
           // playerunit.transform.rotation = Quaternion.Euler(0, yaw, 0);
        }
        else
        {
            // 3인칭 모드일 때 카메라 위치와 회전을 업데이트
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 targetPos = player.transform.position + rotation * currentOffset;

            // 카메라의 움직임을 부드럽게 하는 함수(Lerp)
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * cameraSpeed);

            // 타겟을 바라보도록 카메라 회전
            transform.LookAt(player.transform.position + Vector3.up * 1.5f);
        }
    }
}
