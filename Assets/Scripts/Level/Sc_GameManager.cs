using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sc_GameManager : MonoBehaviour
{
    NavMeshSurface[] surfaces => FindObjectsOfType<NavMeshSurface>();

    public List<Sc_LevelManager> levels;
    [SerializeField] PhysicMaterial physicMat;
    public int roomIndex;
    public int savedRoomIndex;

    private void Awake()
    {
        Collider[] colliders = FindObjectsOfType<Collider>();
        foreach (Collider col in colliders)
        {
            col.material = physicMat;

            if (col.GetComponent<MeshCollider>())
            {
                col.GetComponent<MeshCollider>().convex = true;
            }
        }
    }

    private void Start()
    {
        Sc_EventManager.current.onGlobalRespawn += SwitchRooms;
        if (levels.Count > 0)
        {
            for (int i = 0; i < levels.Count - 1; i++)
            {
                if (levels[i] != null && levels[i + 1] != null)
                {
                    levels[i].data.mainDoor.destination = levels[i + 1].data.roomSpawn;
                }
            }

            levels[levels.Count - 1].data.mainDoor.destination = levels[0].data.roomSpawn;
        }

        SwitchRooms();
    }

    public void ActiveSurfaces()
    {
        foreach (NavMeshSurface surface in surfaces)
        {
            surface.BuildNavMesh();
        }
    }

    public void IncreaseIndex()
    {
        roomIndex++;

        if (roomIndex > levels.Count - 1)
        {
            roomIndex = 0;
        }
    }

    public void SwitchRooms()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].gameObject.SetActive(i == roomIndex);
        }
    }
}
