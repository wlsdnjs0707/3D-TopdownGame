using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour
{
    public Transform gunHandle; // 총을 장착할 위치
    private Gun CurrentGun; // 장착한 총

    public Gun testGun;

    public void Start()
    {
        if (testGun)
        {
            EquipGun(testGun);
        }
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
}
