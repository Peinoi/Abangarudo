using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonCamerRay : MonoBehaviour
{
    public Camera personCam;
    public float distance = 10f;

    // 사격 모드
    bool person = false;

    //피격체 데이터
    RaycastHit hitData;

    void Start()
    {
        if (personCam == null)
        {
            Debug.LogError("카메라가 설정되지 않았습니다.");
        }
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
            if (Physics.Raycast(rayOrigin, rayDir,out hitData, distance))
            {

                Debug.Log("레이 발사~~");
                Debug.DrawRay(rayOrigin, rayDir * distance, Color.red);
                if(hitData.collider.tag == "Enemy")
                {
                    
                    Debug.Log("적개체 발견");
                }
            }
        }
    }
}
