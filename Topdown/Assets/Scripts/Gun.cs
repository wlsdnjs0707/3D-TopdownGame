using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform muzzle;
    public Bullet bullet;
    [HideInInspector] public string gunName;
    [HideInInspector] public float shootCooldown = 0.0f;
    [HideInInspector] public float bulletSpeed = 0.0f;
    [HideInInspector] public float bulletDamage = 0.0f;
    [HideInInspector] public int maxAmmo = 0;
    [HideInInspector] public int currentAmmo = 0;
    [HideInInspector] public float reloadTime = 0;

    private bool isReloading = false;

    private float nextShootTime; // 다음 발사가 가능한 시간

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    public void Shoot()
    {
        if (currentAmmo >0 && Time.time > nextShootTime)
        {
            currentAmmo--;

            // shootCooldown만큼 기다린 뒤 발사
            nextShootTime = Time.time + shootCooldown;
            Bullet newBullet = Instantiate(bullet, muzzle.position, muzzle.rotation) as Bullet;
            newBullet.SetSpeed(bulletSpeed);
            newBullet.SetDamage(bulletDamage);
        }

    }

    public void Reload()
    {
        if (!isReloading)
        {
            isReloading = true;
            StartCoroutine(AbleReload());
        }
    }

    IEnumerator AbleReload()
    {
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().isReloading = false;
    }

}
