using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_CloneCorpse : Sc_GameItem
{
    public override void Effect(Sc_Character chara)
    {
        if (!chara.Health.isDead)
        {
            chara.GetComponent<Sc_PlayerController>().HasCorpse = true;
            Destroy(transform.root.gameObject);
        }
    }
}
