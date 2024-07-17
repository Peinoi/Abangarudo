using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject player;
    public Transform playerHead;  // 플레이어 머리 위치
    public float rotationSpeed = 5.0f;      // 마우스 회전 속도

    private float yaw = 0.0f;               // 마우스 yaw (좌우 회전)
    private float pitch = 0.0f;             // 마우스 pitch (상하 회전)
    private bool isFirstPerson = true;

    void Start()
    {
        mainCamera.enabled = true;
        if (playerHead != null)
        {
            transform.position = playerHead.position;
            transform.rotation = playerHead.rotation;
        }

        // 초기 yaw와 pitch 설정
        yaw = player.transform.eulerAngles.y;
        pitch = player.transform.eulerAngles.x;
    }

    void Update()
    {
        // 마우스 입력을 감지하여 yaw와 pitch 업데이트
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;

        // pitch 값을 제한하여 카메라가 위/아래로 너무 많이 회전하지 않도록 함
        pitch = Mathf.Clamp(pitch, -45f, 45f);

        // 플레이어의 회전을 업데이트
        player.transform.rotation = Quaternion.Euler(0, yaw, 0);
    }

    void LateUpdate()
    {
        if (playerHead != null)
        {
            // 카메라 위치를 플레이어 머리 위치로 고정
            transform.position = playerHead.position;

            // 카메라 회전 업데이트 (플레이어의 회전을 따름)
            transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        }
    }
}
