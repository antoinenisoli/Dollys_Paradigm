using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Character : MonoBehaviour
{
    public Sc_Health Health => GetComponent<Sc_Health>();
    protected Vector3 spawnPos;

    public virtual void Start()
    {
        spawnPos = transform.position;
        Sc_EventManager.current.onGlobalRespawn += Respawn;
        Respawn();
    }

    public virtual void Respawn()
    {
        print(gameObject.name + " respawn");
        Health.CurrentHealth = Health.MaxHealth;
        transform.position = spawnPos;
        Health.isDead = false;
    }

    public virtual void Hurt(int _dmg)
    {
        Health.TakeDamages(_dmg);
        
        if (_dmg > 0)
            StartCoroutine(ChangeLifeColor(Color.red));
        else
            StartCoroutine(ChangeLifeColor(Color.green));
    }

    public virtual void Death()
    {
        
    }

    public virtual IEnumerator ChangeLifeColor(Color color)
    {
        yield return null;
    }
}
