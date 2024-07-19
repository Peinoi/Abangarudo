using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // ���� ������ ��
    [SerializeField]
    private Gun currentGun;

    // ���� �ӵ� ���
    private float currentFireRate;

    // ���� ����
    private bool isReload = false;

    // ���� ������ ��
    private Vector3 originPos;

    // ������ �浹 ���� �޾ƿ�
    private RaycastHit hitInfo;

    // �ʿ��� ������Ʈ
    [SerializeField]
    private Camera theCam;

    // �Ѿ� ������
    [SerializeField]
    private GameObject bulletPrefab;

    // �Ѿ� �߻� ��ġ
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

    // ����ӵ� ����
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
    }

    // �߻� �õ�
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    // �߻� �� ���
    private void Fire()
    {
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0)
                Shoot();
            else
            {
                Debug.Log("�Ѿ��� �����ϴ�. �������� �ʿ��մϴ�.");
            }
        }
    }

    // �߻� �� ���
    private void Shoot()
    {
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate; // ���� �ӵ� ����

        // �Ѿ� ���� �� �߻�
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.velocity = bulletSpawnPoint.forward * currentGun.bulletSpeed;
    }

    public Gun GetGun()
    {
        return currentGun;
    }
}
