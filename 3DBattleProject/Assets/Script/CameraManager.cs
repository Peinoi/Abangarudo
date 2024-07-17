using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera firstPersonCam;
    public Camera overheadCam;
    public Transform parentObject;  // 부모 오브젝트를 참조하기 위한 변수
    bool FPS = false;
    public float width;
    public float height;
    public float lookSpeed = 2f;
    float rotationX = 0f;
    float rotationY = 0f;

    // Start is called before the first frame update
    public void Awake()
    {
        firstPersonCam.enabled = true;
        overheadCam.enabled = true;
        ShowoverheadCam();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !FPS)
        {
            ShowfirstPersonCam();
            FPS = true;
        }
        else if (Input.GetMouseButtonDown(1) && FPS)
        {
            ShowoverheadCam();
            FPS = false;
        }

        if (FPS)
        {
            HandleFirstPersonView();
        }
    }

    void ShowfirstPersonCam()
    {
        overheadCam.rect = new Rect(width, width, height, height);
        firstPersonCam.rect = new Rect(0, 0, 1, 1);
        overheadCam.depth = 1;
        firstPersonCam.depth = -1;

        // 3인칭 카메라의 회전을 1인칭 카메라와 부모 오브젝트에 복사
        Vector3 eulerAngles = overheadCam.transform.eulerAngles;
        rotationX = eulerAngles.x;
        rotationY = eulerAngles.y;
        firstPersonCam.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        if (parentObject != null)
        {
            parentObject.localRotation = Quaternion.Euler(0f, rotationY, 0f);
        }
    }

    void ShowoverheadCam()
    {
        firstPersonCam.rect = new Rect(width, width, height, height);
        overheadCam.rect = new Rect(0, 0, 1, 1);
        overheadCam.depth = -1;
        firstPersonCam.depth = 1;
    }

    void HandleFirstPersonView()
    {
        // 마우스 이동으로 카메라 회전
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeed;
        rotationY += Input.GetAxis("Mouse X") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, 0f, 0f);
        firstPersonCam.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // 부모 오브젝트의 로테이션을 업데이트
        if (parentObject != null)
        {
            parentObject.localRotation = Quaternion.Euler(0f, rotationY, 0f);
        }
    }
}
