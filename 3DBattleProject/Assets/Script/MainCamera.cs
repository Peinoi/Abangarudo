using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    public GameObject Target;               // 카메라가 따라다닐 타겟

    public float offsetX = 0.0f;            // 카메라의 x좌표
    public float offsetY = 2f;              // 카메라의 y좌표
    public float offsetZ = -3f;             // 카메라의 z좌표

    public float CameraSpeed = 10.0f;       // 카메라의 속도
    public float rotationSpeed = 5.0f;      // 마우스 회전 속도
    private Vector3 targetPos;

    private float yaw = 0.0f;               // 마우스 yaw (좌우 회전)
    private float pitch = 0.0f;             // 마우스 pitch (상하 회전)

    void Start()
    {

        // 초기 yaw와 pitch 설정
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    void Update()
    {
        // 마우스 입력을 감지하여 yaw와 pitch 업데이트
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;

        // pitch 값을 제한하여 카메라가 위/아래로 너무 많이 회전하지 않도록 함
        pitch = Mathf.Clamp(pitch, -45f, 45f);


    }

    void LateUpdate()
    {

        // 3인칭 모드일 때
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        targetPos = Target.transform.position + rotation * new Vector3(offsetX, offsetY, offsetZ);

        // 카메라의 움직임을 부드럽게 하는 함수(Lerp)
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * CameraSpeed);

        // 타겟을 바라보도록 카메라 회전
        transform.LookAt(Target.transform.position);

    }


}
