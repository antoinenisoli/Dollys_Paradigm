using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_GameManager : MonoBehaviour
{
    [SerializeField] List<Sc_LevelManager> levels;
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
        for (int i = 0; i < levels.Count - 1; i++)
        {
            if (levels[i] != null && levels[i + 1] != null)
            {
                levels[i].data.mainDoor.destination = levels[i + 1].data.roomSpawn;
            }
        }

        levels[levels.Count - 1].data.mainDoor.destination = levels[0].data.roomSpawn;
    }

    public void IncreaseIndex()
    {
        roomIndex++;

        if (roomIndex > levels.Count - 1)
        {
            roomIndex = 0;
        }
    }

    private void Update()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].gameObject.SetActive(i == roomIndex);           
        }
    }
}
