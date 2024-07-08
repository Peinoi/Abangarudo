using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    public GameObject Target;               // ī�޶� ����ٴ� Ÿ��

    public float offsetX = 0.0f;            // ī�޶��� x��ǥ
    public float offsetY = 2f;              // ī�޶��� y��ǥ
    public float offsetZ = -3f;             // ī�޶��� z��ǥ

    public float CameraSpeed = 10.0f;       // ī�޶��� �ӵ�
    public float rotationSpeed = 5.0f;      // ���콺 ȸ�� �ӵ�
    private Vector3 targetPos;

    private float yaw = 0.0f;               // ���콺 yaw (�¿� ȸ��)
    private float pitch = 0.0f;             // ���콺 pitch (���� ȸ��)

    void Start()
    {

        // �ʱ� yaw�� pitch ����
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    void Update()
    {
        // ���콺 �Է��� �����Ͽ� yaw�� pitch ������Ʈ
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;

        // pitch ���� �����Ͽ� ī�޶� ��/�Ʒ��� �ʹ� ���� ȸ������ �ʵ��� ��
        pitch = Mathf.Clamp(pitch, -45f, 45f);


    }

    void LateUpdate()
    {

        // 3��Ī ����� ��
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        targetPos = Target.transform.position + rotation * new Vector3(offsetX, offsetY, offsetZ);

        // ī�޶��� �������� �ε巴�� �ϴ� �Լ�(Lerp)
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * CameraSpeed);

        // Ÿ���� �ٶ󺸵��� ī�޶� ȸ��
        transform.LookAt(Target.transform.position);

    }


}
