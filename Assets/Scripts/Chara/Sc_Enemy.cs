using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Sc_Enemy : Sc_Character
{
    public NavMeshSurface surface;

    SpriteRenderer spr => GetComponentInChildren<SpriteRenderer>();
    NavMeshAgent agent => GetComponent<NavMeshAgent>();

    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected float aggroRadius = 5;
    protected Collider[] enemies;
    protected Sc_PlayerController player;

    [SerializeField] protected float attackDelay = 1;
    [SerializeField] protected float distanceToPlayer;
    [SerializeField] protected float closeDistance;
    protected float timer;
    protected bool isClose;

    [SerializeField] protected bool detectBlock;
    [SerializeField] protected LayerMask blockLayer;
    [SerializeField] protected float rayThickness = 2;

    private void Awake()
    {
        surface.BuildNavMesh();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);

        if (player != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(player.transform.position, rayThickness);
        }
    }

    public override IEnumerator HurtColor()
    {
        spr.color = hurtColor;
        yield return new WaitForSeconds(0.3f);
        spr.color = Color.white;
    }

    void Detect()
    {
        foreach (Collider col in enemies)
        {
            Sc_PlayerController _player = col.GetComponentInParent<Sc_PlayerController>();
            if (_player)
            {
                player = _player;
            }
        }

        if (enemies.Length <= 0)
            player = null;

        if (player != null)
        {           
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            Vector3 playerPos = player.transform.position;
            playerPos.y = transform.position.y;

            Ray ray = new Ray(transform.position, playerPos - transform.position);
            Debug.DrawRay(transform.position, playerPos - transform.position);
            detectBlock = Physics.SphereCast(ray, rayThickness, out RaycastHit hit, distanceToPlayer, blockLayer);

            if (isClose && !detectBlock)
            {
                agent.isStopped = true;
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
            }
        }
    }

    private void FixedUpdate()
    {
        enemies = Physics.OverlapSphere(transform.position, aggroRadius, playerLayer);               
    }

    public virtual void Attack()
    {
        
    }

    private void Update()
    {
        isClose = distanceToPlayer < closeDistance;
        Detect();
        Attack();
    }
}
