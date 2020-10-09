using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public Sc_Door mainDoor;
    public Sc_DoorSpawn roomSpawn;
    public List<Sc_Enemy> currentEnemies;
    public List<Sc_Enemy> allEnemies = new List<Sc_Enemy>();

    public void ResetList()
    {
        for (int i = 0; i < allEnemies.Count; i++)
        {
            if (!currentEnemies.Contains(allEnemies[i]))
                currentEnemies.Add(allEnemies[i]);
        }
    }
}

public class Sc_LevelManager : MonoBehaviour
{
    public LevelData data;
    [SerializeField] bool showGizmos;
    [SerializeField] float doorDelay = 2;

    [SerializeField] LayerMask detect = 1 >> 8;
    [SerializeField] Vector3 offSet;
    [SerializeField] float detectRadius = 40;
    Vector3 detectArea;

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            detectArea = transform.position + offSet;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(detectArea, detectRadius);
        }
    }

    private void Awake()
    {
        GetRoomData();
    }

    public void GetRoomData()
    {
        detectArea = transform.position + offSet;
        Collider[] scan = Physics.OverlapSphere(detectArea, detectRadius, detect);

        foreach (Collider col in scan)
        {
            Sc_Door door = col.gameObject.GetComponent<Sc_Door>();
            if (data.mainDoor == null)
            {
                data.mainDoor = door;
            }

            Sc_DoorSpawn spawn = col.gameObject.GetComponent<Sc_DoorSpawn>();
            if (data.roomSpawn == null)
            {
                data.roomSpawn = spawn;
            }

            Sc_Enemy enemy = col.gameObject.GetComponentInParent<Sc_Enemy>();
            if (enemy && !data.allEnemies.Contains(enemy))
            {
                data.allEnemies.Add(enemy);
            }
        }

        data.ResetList();
    }

    public void LevelReset()
    {
        GetRoomData();
        foreach (Sc_Enemy mob in data.allEnemies)
        {
            mob.gameObject.SetActive(true);
            mob.Respawn();
            mob.player = null;
        }

        data.mainDoor.canActivate = false;
    }

    private void Update()
    {
        for (int i = 0; i < data.currentEnemies.Count; i++)
        {
            if (data.currentEnemies[i].Health.isDead)
            {
                data.currentEnemies.Remove(data.currentEnemies[i]);
                break;
            }
        }

        if (data.currentEnemies.Count == 0 && !data.mainDoor.canActivate)
        {
            data.mainDoor.Open(doorDelay);
        }
    }
}
