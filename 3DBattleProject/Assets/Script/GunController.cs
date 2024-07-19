using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // 현재 장착된 총
    [SerializeField]
    private Gun currentGun;

    // 연사 속도 계산
    private float currentFireRate;

    // 상태 변수
    private bool isReload = false;

    // 본래 포지션 값
    private Vector3 originPos;

    // 레이저 충돌 정보 받아옴
    private RaycastHit hitInfo;

    // 필요한 컴포넌트
    [SerializeField]
    private Camera theCam;

    // 총알 프리팹
    [SerializeField]
    private GameObject bulletPrefab;

    // 총알 발사 위치
    [SerializeField]
    private Transform bulletSpawnPoint;

    void Start()
    {
        originPos = Vector3.zero;
    }

    void Update()
    {
        GunFireRateCalc();
        TryFire();
    }

    // 연사속도 재계산
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
    }

    // 발사 시도
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    // 발사 전 계산
    private void Fire()
    {
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0)
                Shoot();
            else
            {
                Debug.Log("총알이 없습니다. 재장전이 필요합니다.");
            }
        }
    }

    // 발사 후 계산
    private void Shoot()
    {
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate; // 연사 속도 재계산

        // 총알 생성 및 발사
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.velocity = bulletSpawnPoint.forward * currentGun.bulletSpeed;
    }

    public Gun GetGun()
    {
        return currentGun;
    }
}
