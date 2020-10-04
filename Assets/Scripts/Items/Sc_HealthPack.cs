using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_HealthPack : Sc_GameItem
{
    public override void Effect(Sc_Character chara)
    {
        if (chara.Health.CurrentHealth < chara.Health.MaxHealth)
        {
            chara.Hurt(-value);
            Destroy(gameObject);
        }
    }
}
