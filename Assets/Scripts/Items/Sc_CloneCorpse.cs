using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_CloneCorpse : Sc_GameItem
{
    public override void Effect(Sc_Character chara)
    {
        Sc_PlayerController player = chara.GetComponent<Sc_PlayerController>();
        if (!player.Health.isDead)
        {
            player.HasCorpse = true;
            player.lootSound.Play();
            Destroy(transform.root.gameObject);
        }
    }
}
