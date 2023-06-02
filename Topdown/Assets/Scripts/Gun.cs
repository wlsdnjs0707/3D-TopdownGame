using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private GameObject muzzle;
    public Bullet bullet;
    [HideInInspector] public string gunName;
    [HideInInspector] public float shootCooldown = 0.0f;
    [HideInInspector] public float bulletSpeed = 0.0f;
    [HideInInspector] public float bulletDamage = 0.0f;
    [HideInInspector] public int maxAmmo = 0;
    [HideInInspector] public int currentAmmo = 0;
    [HideInInspector] public float reloadTime = 0;

    private bool isReloading = false;

    private IEnumerator reload;

    private float nextShootTime; // 다음 발사가 가능한 시간

    private void Start()
    {
        currentAmmo = maxAmmo;
        reload = ShotgunReload();
        muzzle = GameObject.FindGameObjectWithTag("Muzzle");
    }

    public void Shoot()
    {
        if (currentAmmo >0 && Time.time > nextShootTime)
        {
            currentAmmo--;

            // shootCooldown만큼 기다린 뒤 발사
            nextShootTime = Time.time + shootCooldown;
            Bullet newBullet = Instantiate(bullet, muzzle.transform.position, muzzle.transform.rotation) as Bullet;
            newBullet.SetSpeed(bulletSpeed);
            newBullet.SetDamage(bulletDamage);
        }
    }

    public void ShotgunShoot()
    {
        if (isReloading)
        {
            isReloading = false;
            StopCoroutine(reload);
        }

        if (currentAmmo > 0 && Time.time > nextShootTime)
        {
            currentAmmo--;

            // shootCooldown만큼 기다린 뒤 발사
            nextShootTime = Time.time + shootCooldown;

            for (int i=0; i<5; i++)
            {
                float index = (i-1)*5;

                Vector3 rot = new(muzzle.transform.rotation.eulerAngles.x, muzzle.transform.rotation.eulerAngles.y + index, muzzle.transform.rotation.eulerAngles.z);

                Bullet newBullet = Instantiate(bullet, muzzle.transform.position, Quaternion.Euler(rot)) as Bullet;
                newBullet.SetSpeed(bulletSpeed);
                newBullet.SetDamage(bulletDamage);
            }
        }
    }

    public void Reload(bool isShotgun)
    {
        if (!isReloading)
        {
            isReloading = true;

            if (isShotgun)
            {
                StartCoroutine(reload);
            }
            else
            {
                StartCoroutine(DefaultReload());
            }
        }
    }

    IEnumerator DefaultReload()
    {
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().isReloading = false;
    }

    IEnumerator ShotgunReload()
    {
        while (true)
        {
            yield return new WaitForSeconds(reloadTime);

            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().isReloading = false;

            if (currentAmmo < maxAmmo)
            {
                currentAmmo++;
            }
            else
            {
                isReloading = false;
                StopCoroutine(reload);
            }
        }
    }

}
