using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Sc_Enemy : Sc_Character
{
    public NavMeshSurface surface;

    protected Animator anim => GetComponentInChildren<Animator>();
    protected SpriteRenderer spr => GetComponentInChildren<SpriteRenderer>();
    protected NavMeshAgent agent => GetComponent<NavMeshAgent>();
    protected Material mat;

    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected float aggroRadius = 5;
    protected Collider[] enemies;
    protected Sc_PlayerController player;

    [SerializeField] protected float attackDelay = 1.5f;
    [SerializeField] protected float distanceToPlayer;
    [SerializeField] protected float closeDistance;
    [SerializeField] protected float timer;
    [SerializeField] protected bool isClose;

    public virtual void Awake()
    {
        surface.BuildNavMesh();
        mat = spr.material;
    }

    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
    }

    public override IEnumerator ChangeLifeColor(Color color)
    {
        mat.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.15f);
        mat.DisableKeyword("_EMISSION");
    }

    public virtual void Detect()
    {
        if (player != null && (enemies.Length <= 0 || player.Health.isDead))
            player = null;

        foreach (Collider col in enemies)
        {
            Sc_PlayerController _player = col.GetComponentInParent<Sc_PlayerController>();
            if (_player)
            {
                player = _player;
            }
        }
    }

    private void FixedUpdate()
    {
        enemies = Physics.OverlapSphere(transform.position, aggroRadius, playerLayer);               
    }

    public override void Death()
    {
        base.Death();
        anim.SetTrigger("Death");
    }

    public virtual void Fight()
    {
        
    }

    public virtual void LaunchAttack()
    {
        timer = 0;
    }

    private void Update()
    {
        anim.SetBool("isDead", Health.isDead);
        isClose = distanceToPlayer < closeDistance;
        Detect();

        if (player != null && player.Health.isDead)
            return;

        Fight();
    }
}
