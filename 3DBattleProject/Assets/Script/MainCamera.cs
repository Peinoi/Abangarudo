using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject player;
    public Transform playerHead;  // �÷��̾� �Ӹ� ��ġ
    public float cameraSpeed = 10.0f;       // ī�޶��� �ӵ�
    public float rotationSpeed = 5.0f;      // ���콺 ȸ�� �ӵ�

    private float yaw = 0.0f;               // ���콺 yaw (�¿� ȸ��)
    private float pitch = 0.0f;            // ���콺 pitch (���� ȸ��)
    private bool isFirstPerson = false;

    // �⺻ ������ ��
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
        // �ʱ� yaw�� pitch ����
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    void Update()
    {
        if (mouse == 1)
        {
            // ���콺 �Է��� �����Ͽ� yaw�� pitch ������Ʈ
            yaw += Input.GetAxis("Mouse X") * rotationSpeed;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;

            // pitch ���� �����Ͽ� ī�޶� ��/�Ʒ��� �ʹ� ���� ȸ������ �ʵ��� ��
            pitch = Mathf.Clamp(pitch, -45f, 45f);
        }

        // ���콺 ������ ��ư Ŭ�� �� 1��Ī/3��Ī ��� ��ȯ
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
                transform.position = playerHead.position; // 1��Ī ���� ��ȯ �� ī�޶� ��ġ�� �÷��̾� �Ӹ� ��ġ�� ����
            }

            isFirstPerson = !isFirstPerson;
        }*/
    }

    void LateUpdate()
    {
        if (isFirstPerson)
        {
            // 1��Ī ����� �� ī�޶� ��ġ�� �÷��̾� �Ӹ� ��ġ�� ����
           // transform.position = playerHead.position;
           // transform.rotation = Quaternion.Euler(pitch, yaw, 0);
           // playerunit.transform.rotation = Quaternion.Euler(0, yaw, 0);
        }
        else
        {
            // 3��Ī ����� �� ī�޶� ��ġ�� ȸ���� ������Ʈ
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 targetPos = player.transform.position + rotation * currentOffset;

            // ī�޶��� �������� �ε巴�� �ϴ� �Լ�(Lerp)
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * cameraSpeed);

            // Ÿ���� �ٶ󺸵��� ī�޶� ȸ��
            transform.LookAt(player.transform.position + Vector3.up * 1.5f);
        }
    }
}
