using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Sc_Enemy : Sc_Character
{
    protected Animator anim => GetComponentInChildren<Animator>();
    protected SpriteRenderer spr => GetComponentInChildren<SpriteRenderer>();
    protected NavMeshAgent agent => GetComponent<NavMeshAgent>();

    public NavMeshSurface surface;
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
    [SerializeField] AudioSource attackSound;

    public virtual void Awake()
    {
        surface.BuildNavMesh();
        mat = spr.material;
        mat.DisableKeyword("_EMISSION");
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

        if (Health.isDead)
        {
            agent.isStopped = true;
            return;
        }
    }

    private void FixedUpdate()
    {
        enemies = Physics.OverlapSphere(transform.position, aggroRadius, playerLayer);               
    }

    public override void Death()
    {
        anim.SetTrigger("Death");
    }

    public virtual void Fight()
    {
        if (Health.isDead)
            return;
    }

    public virtual void LaunchAttack()
    {
        attackSound.Play();
        timer = 0;
    }

    public virtual void Update()
    {
        anim.SetBool("isDead", Health.isDead);
        isClose = distanceToPlayer < closeDistance;
        Detect();

        if (player != null && player.Health.isDead)
            return;

        Fight();
    }
}
