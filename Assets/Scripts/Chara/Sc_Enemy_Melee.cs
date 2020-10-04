using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sc_Enemy_Melee : Sc_Enemy
{
    [SerializeField] int damage = 5;

    public override void Awake()
    {
        surface = GameObject.Find("NavMeshSurface_Melee").GetComponent<NavMeshSurface>();
        base.Awake();
    }

    public override void Fight()
    {
        base.Fight();

        if (isClose)
        {
            timer += Time.deltaTime;

            if (player != null && timer > attackDelay)
            {
                timer = 0;
                anim.SetTrigger("Attack");
            }
        }
        else
        {
            timer = 0;
        }
    }

    public override void LaunchAttack()
    {
        base.LaunchAttack();
        player.Hurt(damage);
    }
}
