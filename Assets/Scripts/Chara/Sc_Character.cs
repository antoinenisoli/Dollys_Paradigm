using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sc_Character : MonoBehaviour
{
    protected Sc_GameManager manager => FindObjectOfType<Sc_GameManager>();
    public Sc_Health Health => GetComponent<Sc_Health>();
    protected bool hit;
    protected Vector3 spawnPos;

    public virtual void Awake()
    {
        manager.ActiveSurfaces();
        spawnPos = transform.position;
    }

    public virtual void Start()
    {
        Sc_EventManager.current.onGlobalRespawn += Respawn;
    }

    public virtual void Respawn()
    {
        Health.CurrentHealth = Health.MaxHealth;
        transform.position = spawnPos;
        Health.isDead = false;
    }

    public virtual void Hurt(int _dmg)
    {
        if (_dmg > 0 && hit)
            return;

        Health.TakeDamages(_dmg);
    }

    public abstract void Death();

    public virtual IEnumerator ChangeLifeColor(Color color)
    {
        yield return null;
    }
}
