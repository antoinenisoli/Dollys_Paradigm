using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_LevelSpawn : MonoBehaviour
{
    [SerializeField] List<Sc_Interactable> interactables;
    [SerializeField] List<Sc_Enemy> enemies;
    [SerializeField] LayerMask detect;
    [SerializeField] Vector3 offSet;
    Vector3 detectArea;
    [SerializeField] float detectRadius = 40;

    public void OnDrawGizmosSelected()
    {
        detectArea = transform.position + offSet;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectArea, detectRadius);
    }

    private void Awake()
    {
        detectArea = transform.position + offSet;
        Collider[] scan = Physics.OverlapSphere(detectArea, detectRadius, detect);

        foreach (Collider col in scan)
        {
            Sc_Interactable inte = col.gameObject.GetComponent<Sc_Interactable>();
            if (inte && !interactables.Contains(inte))
            {
                interactables.Add(inte);
            }

            Sc_Enemy enemy = col.gameObject.GetComponentInParent<Sc_Enemy>();
            if (enemy && !enemies.Contains(enemy))
            {
                enemies.Add(enemy);
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].Health.isDead)
            {
                enemies.Remove(enemies[i]);
                break;
            }
        }

        for (int i = 0; i < interactables.Count; i++)
        {
            if (enemies.Count == 0)
            {
                interactables[i].canActivate = true;
                break;
            }
        }
    }
}
