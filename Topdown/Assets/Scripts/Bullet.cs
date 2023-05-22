using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float bulletSpeed = 10;
    public float bulletDamage = 10;
    public LayerMask collisionMask;

    public void SetSpeed(float newBulletSpeed)
    {
        bulletSpeed = newBulletSpeed;
    }

    void Update()
    {
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
