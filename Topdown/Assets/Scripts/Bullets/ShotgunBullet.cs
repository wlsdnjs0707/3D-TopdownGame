using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : Bullet
{
    private void Start()
    {
        Destroy(gameObject, 0.25f);
    }
}
