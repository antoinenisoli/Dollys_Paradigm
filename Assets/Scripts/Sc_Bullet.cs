using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Bullet : Sc_GameItem
{
    public override void Effect(Sc_Character chara)
    {
        base.Effect(chara);
        Destroy(gameObject);
    }
}
