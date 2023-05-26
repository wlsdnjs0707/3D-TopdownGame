using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPack : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (player.GetComponent<Player>().health < player.GetComponent<Player>().maxHealth)
            {
                player.GetComponent<Player>().health += 10;
            }

            GameObject.Destroy(gameObject);
        }
    }
}
