using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName; // 총의 이름.
    public float range; // 사정 거리
    public float fireRate; // 연사속도.
    public float reloadTime; // 재장전 속도.
    public int damage; // 총의 데미지.
    public int reloadBulletCount; // 총알 재장전 개수.
    public int currentBulletCount; // 현재 탄알집에 남아있는 총알의 개수.
    public int maxBulletCount; // 최대 소유 가능 총알 개수.
    public int carryBulletCount; // 현재 소유하고 있는 총알 개수.

    public float bulletSpeed; // 총알 속도
}
