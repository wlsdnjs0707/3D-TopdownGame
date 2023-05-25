using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float bulletSpeed = 10;
    private float bulletDamage = 10;
    public LayerMask collisionMask;

    public float bulletLifeTime = 2; // 총알이 사라지기까지의 시간

    public void SetSpeed(float newBulletSpeed)
    {
        bulletSpeed = newBulletSpeed;
    }

    public void SetDamage(float newBulletDamage)
    {
        bulletDamage = newBulletDamage;
    }

    private void Start()
    {
        // 일정 시간 이후 오브젝트 제거
        Destroy(gameObject, bulletLifeTime);
    }

    void Update()
    {
        // 총알 이동
        transform.Translate(bulletSpeed * Time.deltaTime * Vector3.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            IDamageable damageableObject = other.GetComponent<IDamageable>();

            if (damageableObject != null)
            {
                damageableObject.TakeHit(bulletDamage);
            }

            GameObject.Destroy(gameObject);
        }
    }
}
