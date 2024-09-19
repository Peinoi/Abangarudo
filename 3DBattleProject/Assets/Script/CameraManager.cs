using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraManager : MonoBehaviourPunCallbacks
{
    public Camera firstPersonCam;
    public Camera overheadCam;
    public Transform parentObject;  // �θ� ������Ʈ�� �����ϱ� ���� ����
    bool FPS = false;
    public float width;
    public float height;
    public float lookSpeed = 2f;
    float rotationX = 0f;
    float rotationY = 0f;
    public GameObject openTarget;
    public GameObject gunInfo;
    public void Awake()
    {
        if (!photonView.IsMine)
        {
            Destroy(firstPersonCam.gameObject);
            Destroy(overheadCam.gameObject);
            return;
        }

        firstPersonCam.enabled = true;
        overheadCam.enabled = true;
        ShowoverheadCam();
        openTarget = GameObject.FindWithTag("Target");
        gunInfo = GameObject.FindWithTag("GunInfo");
        if (openTarget != null)
        {
            openTarget.transform.localScale = Vector3.one;
            gunInfo.transform.localScale = Vector3.one;
            openTarget.SetActive(false);
            gunInfo.SetActive(false);
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetMouseButtonDown(1) && !FPS)
        {
            ShowfirstPersonCam();
            overheadCam.gameObject.SetActive(false);
            if (openTarget != null)
            {
                openTarget.SetActive(true);
                gunInfo.SetActive(true);
            }
            FPS = true;
        }
        else if (Input.GetMouseButtonDown(1) && FPS)
        {
            ShowoverheadCam();
            overheadCam.gameObject.SetActive(true);
            if (openTarget != null)
            {
                openTarget.SetActive(false);
                gunInfo.SetActive(false);
            }
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

        // 3��Ī ī�޶��� ȸ���� 1��Ī ī�޶�� �θ� ������Ʈ�� ����
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
        // ���콺 �̵����� ī�޶� ȸ��
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeed;
        rotationY += Input.GetAxis("Mouse X") * lookSpeed;

        // ȸ�� ���� ����
        rotationX = Mathf.Clamp(rotationX, -10f, 10f);

        firstPersonCam.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // �θ� ������Ʈ�� �����̼��� ������Ʈ
        if (parentObject != null)
        {
            parentObject.localRotation = Quaternion.Euler(0f, rotationY, 0f);
        }
    }
}
