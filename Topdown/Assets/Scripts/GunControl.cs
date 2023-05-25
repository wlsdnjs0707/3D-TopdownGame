using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour
{
    public Transform gunHandle; // 총을 장착할 위치
    [HideInInspector] public Gun CurrentGun; // 장착한 총

    public Gun testGun;

    public void Start()
    {
        EquipTestGun();
    }

    public void EquipGun(Gun gun)
    {
        // 이미 장착한 총이 있으면 제거
        if (CurrentGun)
        {
            Destroy(CurrentGun.gameObject);
        }

        // 총 장착
        CurrentGun = Instantiate(gun, gunHandle.position, gunHandle.rotation) as Gun;
        CurrentGun.transform.parent = gunHandle;
    }

    public void Shoot()
    {
        if (CurrentGun)
        {
            CurrentGun.Shoot();
        }
    }

    public void EquipTestGun()
    {
        if (testGun)
        {
            EquipGun(testGun);

            CurrentGun.shootCooldown = 0.75f;
            CurrentGun.bulletSpeed = 10.0f;
            CurrentGun.bulletDamage = 10.0f;
        }
    }

    public void IncreaseDamage() // 총 데미지 증가
    {
        CurrentGun.bulletDamage += 5;
    }

    public void DecreaseShootCooldown() // 연사력 증가
    {
        CurrentGun.shootCooldown -= 0.1f; 
    }
}
