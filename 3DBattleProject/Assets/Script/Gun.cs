using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName; // ���� �̸�.
    public float range; // ���� �Ÿ�
    public float fireRate; // ����ӵ�.
    public float reloadTime; // ������ �ӵ�.
    public int damage; // ���� ������.
    public int reloadBulletCount; // �Ѿ� ������ ����.
    public int currentBulletCount; // ���� ź������ �����ִ� �Ѿ��� ����.
    public int maxBulletCount; // �ִ� ���� ���� �Ѿ� ����.
    public int carryBulletCount; // ���� �����ϰ� �ִ� �Ѿ� ����.

    public float bulletSpeed; // �Ѿ� �ӵ�
}
