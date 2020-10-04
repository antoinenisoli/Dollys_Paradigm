using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Enemy_Melee : Sc_Enemy
{
    public override void Attack()
    {
        timer += Time.deltaTime;
        if (isClose && player != null && timer > attackDelay)
        {
            timer = 0;
            player.Hurt(5);
        }
    }
}
