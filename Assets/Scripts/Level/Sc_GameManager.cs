using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_GameManager : MonoBehaviour
{
    Sc_PlayerController player => FindObjectOfType<Sc_PlayerController>();
    [SerializeField] Sc_LevelManager[] levels;
    public int roomIndex;
    public int savedRoomIndex;

    public void IncreaseIndex()
    {
        roomIndex++;

        if (roomIndex > levels.Length - 1)
        {
            roomIndex = 0;
        }
    }

    private void Start()
    {
        for (int i = 0; i < levels.Length - 1; i++)
        {
            if (levels[i] != null && levels[i + 1] != null)
            {
                levels[i].mainDoor.destination = levels[i + 1].roomSpawn;
            }
        }

        levels[levels.Length - 1].mainDoor.destination = levels[0].roomSpawn;
    }

    private void Update()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].gameObject.SetActive(i == roomIndex);
        }
    }
}
