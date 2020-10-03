using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Enemy : Sc_Character
{
    SpriteRenderer spr => GetComponentInChildren<SpriteRenderer>();

    public override IEnumerator HurtColor()
    {
        spr.color = hurtColor;
        yield return new WaitForSeconds(0.3f);
        spr.color = Color.white;
    }
}
