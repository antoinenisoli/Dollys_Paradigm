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
        StartCoroutine(HurtColor());
    }

    public virtual IEnumerator HurtColor()
    {
        yield return null;
    }
}
