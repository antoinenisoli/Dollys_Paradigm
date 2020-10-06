using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_LevelManager : MonoBehaviour
{
    public Sc_Door mainDoor;
    public Sc_DoorSpawn roomSpawn;
    [SerializeField] List<Sc_Enemy> enemies;
    [SerializeField] LayerMask detect = 1 >> 8;
    [SerializeField] Vector3 offSet;
    Vector3 detectArea;
    [SerializeField] float detectRadius = 40;

    private void OnDrawGizmos()
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
            Sc_Door door = col.gameObject.GetComponent<Sc_Door>();
            if (mainDoor == null)
            {
                mainDoor = door;
            }

            Sc_DoorSpawn spawn = col.gameObject.GetComponent<Sc_DoorSpawn>();
            if (roomSpawn == null)
            {
                roomSpawn = spawn;
            }

            Sc_Enemy enemy = col.gameObject.GetComponentInParent<Sc_Enemy>();
            if (enemy && !enemies.Contains(enemy))
            {
                enemies.Add(enemy);
            }
        }
    }

    public void LevelReset()
    {
        foreach (Sc_Enemy mob in enemies)
        {
            mob.gameObject.SetActive(true);
            mob.Respawn();
        }

        mainDoor.canActivate = false;
    }

    private void OnEnable()
    {
        LevelReset();
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

        if (enemies.Count == 0 && !mainDoor.canActivate)
        {
            mainDoor.Open();
        }
    }
}
