using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject player;
    public Transform playerHead;  // �÷��̾� �Ӹ� ��ġ
    public float rotationSpeed = 5.0f;      // ���콺 ȸ�� �ӵ�

    private float yaw = 0.0f;               // ���콺 yaw (�¿� ȸ��)
    private float pitch = 0.0f;             // ���콺 pitch (���� ȸ��)
    private bool isFirstPerson = true;

    void Start()
    {
        mainCamera.enabled = true;
        if (playerHead != null)
        {
            transform.position = playerHead.position;
            transform.rotation = playerHead.rotation;
        }

        // �ʱ� yaw�� pitch ����
        yaw = player.transform.eulerAngles.y;
        pitch = player.transform.eulerAngles.x;
    }

    void Update()
    {
        // ���콺 �Է��� �����Ͽ� yaw�� pitch ������Ʈ
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;

        // pitch ���� �����Ͽ� ī�޶� ��/�Ʒ��� �ʹ� ���� ȸ������ �ʵ��� ��
        pitch = Mathf.Clamp(pitch, -45f, 45f);

        // �÷��̾��� ȸ���� ������Ʈ
        player.transform.rotation = Quaternion.Euler(0, yaw, 0);
    }

    void LateUpdate()
    {
        if (playerHead != null)
        {
            // ī�޶� ��ġ�� �÷��̾� �Ӹ� ��ġ�� ����
            transform.position = playerHead.position;

            // ī�޶� ȸ�� ������Ʈ (�÷��̾��� ȸ���� ����)
            transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        }
    }
}
