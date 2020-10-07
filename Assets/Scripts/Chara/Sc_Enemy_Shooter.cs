using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sc_Enemy_Shooter : Sc_Enemy
{
    [Header("Shooter enemy")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] float bulletSpeed;

    [SerializeField] protected bool detectBlock;
    public GameObject obj;
    [SerializeField] protected LayerMask blockLayer;
    [SerializeField] protected float rayThickness = 2;

    public override void Awake()
    {
        surface = GameObject.Find("NavMeshSurface_Shooter").GetComponent<NavMeshSurface>();
        base.Awake();
    }

    public override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        if (player != null)
        {
            Gizmos.color = Color.white;
            Vector3 playerPos = player.transform.position;
            Gizmos.DrawWireSphere(playerPos, rayThickness);
        }
    }

    public override void LaunchAttack()
    {
        base.LaunchAttack();
        GameObject _bullet = Instantiate(bullet, shootPos.position, Quaternion.identity);
        Rigidbody rb = _bullet.GetComponent<Rigidbody>();
        rb.velocity = (player.transform.position - shootPos.position).normalized * bulletSpeed;
    }

    public override void Fight()
    {
        base.Fight();

        if (isClose)
        {
            timer += Time.deltaTime;

            if (player != null && !detectBlock && timer > attackDelay)
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

    public override void Detect()
    {
        base.Detect();

        if (player != null)
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            Vector3 playerPos = player.transform.position;

            Ray ray = new Ray(transform.position, playerPos - transform.position);
            Debug.DrawRay(transform.position, playerPos - transform.position);
            detectBlock = Physics.SphereCast(ray, rayThickness, out RaycastHit hit, distanceToPlayer, blockLayer);

            if (player.Health.isDead || agent.pathStatus == NavMeshPathStatus.PathPartial)
            {
                agent.SetDestination(spawnPos);
                agent.isStopped = false;
            }
            else
            {
                if (isClose && !detectBlock)
                {
                    agent.isStopped = true;
                }
                else
                {
                    if (detectBlock)
                        obj = hit.collider.gameObject;

                    agent.SetDestination(player.transform.position);
                    agent.isStopped = false;
                }
            }
        }
    }
}
