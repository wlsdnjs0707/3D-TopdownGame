using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform muzzle;
    public Bullet bullet;
    public float shootCooldown = 100;
    public float bulletSpeed = 10;

    private float nextShootTime; // 다음 발사가 가능한 시간

    public void Shoot()
    {
        if (Time.time > nextShootTime)
        {
            // shootCooldown만큼 기다린 뒤 발사
            nextShootTime = Time.time + shootCooldown;
            Bullet newBullet = Instantiate(bullet, muzzle.position, muzzle.rotation) as Bullet;
            newBullet.SetSpeed(bulletSpeed);
        }

    }

}
