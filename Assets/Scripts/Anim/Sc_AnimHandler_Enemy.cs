using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_AnimHandler_Enemy : MonoBehaviour
{
    Sc_Enemy sh => GetComponentInParent<Sc_Enemy>();

    [SerializeField] GameObject explosionSound;

    public void LaunchShoot()
    {
        sh.LaunchAttack();
    }

    public void PlaySound()
    {
        Instantiate(explosionSound, transform.position, Quaternion.identity);
    }

    public void Death()
    {
        sh.gameObject.SetActive(false);
    }
}
