using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.ammo))
        {
            other.gameObject.SetActive(false);
            GameManager.instance.player.AddBullet(25);
        }
        else if (other.CompareTag(Tags.heal))
        {
            other.gameObject.SetActive(false);
            GameManager.instance.player.AddHealth(20);
        }
    }
}
