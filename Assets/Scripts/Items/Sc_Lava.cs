using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Lava : Sc_GameItem
{
    public override void Effect(Sc_Character chara)
    {
        base.Effect(chara);
        chara.Hurt(value);
    }
}
