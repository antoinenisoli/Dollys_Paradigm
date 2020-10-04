using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Character : MonoBehaviour
{
    public Sc_Health Health => GetComponent<Sc_Health>();
    public Color hurtColor = Color.red;

    public virtual void Hurt(int _dmg)
    {
        Health.TakeDamages(_dmg);
        
        if (_dmg > 0)
        {
            StartCoroutine(HurtColor());
        }
        else
        {
            StartCoroutine(HealColor());
        }
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }

    public virtual IEnumerator HurtColor()
    {
        yield return null;
    }

    public virtual IEnumerator HealColor()
    {
        yield return null;
    }
}
