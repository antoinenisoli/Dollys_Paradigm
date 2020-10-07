using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sc_Enemy_Melee : Sc_Enemy
{
    [Header("Melee")]
    [SerializeField] int damage = 5;

    public override void Awake()
    {
        surface = GameObject.Find("NavMeshSurface_Melee").GetComponent<NavMeshSurface>();
        base.Awake();
    }

    public override void Detect()
    {
        base.Detect();

        if (player != null)
        {
            Vector3 playerPos = player.transform.position;
            distanceToPlayer = Vector3.Distance(transform.position, playerPos);

            if ((player.Health.isDead || agent.pathStatus == NavMeshPathStatus.PathPartial) && !Health.isDead)
            {
                agent.SetDestination(spawnPos);
                agent.isStopped = false;
            }
            else
            {
                agent.isStopped = isClose;

                if (!isClose && !Health.isDead)
                {
                    agent.SetDestination(playerPos);
                }
            }
        }
    }

    public override void Fight()
    {
        base.Fight();
        timer += Time.deltaTime;

        if (isClose)
        {

            if (player != null && timer > attackDelay)
            {
                timer = 0;
                anim.SetTrigger("Attack");
            }
        }
    }

    public override void LaunchAttack()
    {
        base.LaunchAttack();
        player.Hurt(damage);
    }

    public override void Update()
    {
        Vector2 vel = agent.velocity.normalized;
        anim.SetFloat("Velocity", vel.magnitude);
        base.Update();       
    }
}
