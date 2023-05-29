using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour
{
    public Transform gunHandle; // 총을 장착할 위치
    [HideInInspector] public Gun CurrentGun; // 장착한 총

    public Gun testGun;
    public Gun assaultRifle;
    public Gun sniperRifle;

    public void Start()
    {
        SniperRifle();
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

    public void Reload()
    {
        if (CurrentGun)
        {
            CurrentGun.Reload();
        }
    }

    public void TestGun()
    {
        if (testGun)
        {
            EquipGun(testGun);

            CurrentGun.gunName = "TestGun";
            CurrentGun.shootCooldown = 0.75f;
            CurrentGun.bulletSpeed = 10.0f;
            CurrentGun.bulletDamage = 10.0f;
            CurrentGun.maxAmmo = 10;
            CurrentGun.reloadTime = 1.0f;
        }
    }

    public void AssaultRifle()
    {
        if (assaultRifle)
        {
            EquipGun(assaultRifle);

            CurrentGun.gunName = "AssaultRifle";
            CurrentGun.shootCooldown = 0.25f;
            CurrentGun.bulletSpeed = 20.0f;
            CurrentGun.bulletDamage = 5.0f;
            CurrentGun.maxAmmo = 30;
            CurrentGun.reloadTime = 1.5f;
        }
    }

    public void SniperRifle()
    {
        if (sniperRifle)
        {
            EquipGun(sniperRifle);

            CurrentGun.gunName = "SniperRifle";
            CurrentGun.shootCooldown = 1.5f;
            CurrentGun.bulletSpeed = 25.0f;
            CurrentGun.bulletDamage = 25.0f;
            CurrentGun.maxAmmo = 10;
            CurrentGun.reloadTime = 2.0f;
        }
    }

    public void IncreaseDamage() // 총 데미지 증가
    {
        if (CurrentGun.gunName.Equals("TestGun"))
        {
            CurrentGun.bulletDamage += 5.0f;
        }
        else if (CurrentGun.gunName.Equals("AssaultRifle"))
        {
            CurrentGun.bulletDamage += 5.0f;
        }
        else if (CurrentGun.gunName.Equals("SniperRifle"))
        {
            CurrentGun.bulletDamage += 5.0f;
        }
    }

    public void DecreaseShootCooldown() // 연사력 증가
    {
        if (CurrentGun.gunName.Equals("TestGun"))
        {
            CurrentGun.shootCooldown -= 0.1f;
        }
        else if(CurrentGun.gunName.Equals("AssaultRifle"))
        {
            CurrentGun.shootCooldown -= 0.02f;
        }
        else if (CurrentGun.gunName.Equals("SniperRifle"))
        {
            CurrentGun.shootCooldown -= 0.1f;
        }
    }
}
