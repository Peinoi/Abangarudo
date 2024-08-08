using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonCamerRay : MonoBehaviour
{
    public Camera personCam;
    public float distance = 10f;

    // ��� ���
    bool person = false;

    //�ǰ�ü ������
    RaycastHit hitData;

    void Start()
    {
        if (personCam == null)
        {
            Debug.LogError("ī�޶� �������� �ʾҽ��ϴ�.");
        }
    }

    void BulletLine()
    {
        // ������ ���콺 ��ư�� ������ �� ���
        if (Input.GetMouseButtonDown(1))
        {
            person = !person;
        }

        // ��� ��尡 Ȱ��ȭ�Ǿ��� ���� ����
        if (person && personCam != null)
        {
            Vector3 rayOrigin = personCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            Vector3 rayDir = personCam.transform.forward;

            // ����ĳ��Ʈ
            if (Physics.Raycast(rayOrigin, rayDir,out hitData, distance))
            {

                Debug.Log("���� �߻�~~");
                Debug.DrawRay(rayOrigin, rayDir * distance, Color.red);
                if(hitData.collider.tag == "Enemy")
                {
                    
                    Debug.Log("����ü �߰�");
                }
            }
        }
    }
}
