using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APBullet : Bullet
{
    private int count;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            count++;

            IDamageable damageableObject = other.GetComponent<IDamageable>();

            if (damageableObject != null)
            {
                damageableObject.TakeHit(bulletDamage);
            }

            if (count >= 3)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}
