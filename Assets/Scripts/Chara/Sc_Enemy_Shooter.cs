using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Enemy_Shooter : Sc_Enemy
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] float bulletSpeed;

    public override void Attack()
    {
        timer += Time.deltaTime;
        if (isClose && player != null && timer > attackDelay)
        {
            timer = 0;
            GameObject _bullet = Instantiate(bullet, shootPos.position, Quaternion.identity);
            Rigidbody rb = _bullet.GetComponent<Rigidbody>();
            rb.velocity = (player.transform.position - shootPos.position).normalized * bulletSpeed;
        }
    }
}
