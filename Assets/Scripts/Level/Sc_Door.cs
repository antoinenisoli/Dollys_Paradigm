using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Door : Sc_Interactable
{
    Sc_GameManager manager => FindObjectOfType<Sc_GameManager>();
    public MeshOutline outline => GetComponentInChildren<MeshOutline>();
    Animator anim => GetComponent<Animator>();

    public Sc_DoorSpawn destination;

    private void Awake()
    {
        outline.enabled = false;
    }

    public override void Activate(Sc_Character chara)
    {
        base.Activate(chara);
        manager.IncreaseIndex();
        manager.SwitchRooms();
        chara.transform.position = destination.spawnPos.position;
        chara.transform.rotation = Quaternion.Euler(new Vector3(0, destination.spawnPos.rotation.eulerAngles.y, 0));

        foreach (Sc_LevelManager level in manager.levels)
        {
            level.LevelReset();
        }
    }

    public override void Open(float delay)
    {
        base.Open(delay);
        StartCoroutine(LaunchOpen(delay));
    }

    public void SwitchDoor()
    {
        canActivate = true;
    }

    IEnumerator LaunchOpen(float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.SetBool("CanActivate", canActivate);
        anim.SetTrigger("Open");
    }
}
