using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Door : Sc_Interactable
{
    Sc_GameManager manager => FindObjectOfType<Sc_GameManager>();
    Animator anim => GetComponent<Animator>();
    public Sc_DoorSpawn destination;

    public override void Activate(Sc_Character chara)
    {
        base.Activate(chara);
        manager.IncreaseIndex();
        chara.transform.position = destination.spawnPos.position;
        chara.transform.rotation = Quaternion.Euler(new Vector3(0, destination.spawnPos.rotation.eulerAngles.y, 0));
    }

    public override void Open()
    {
        base.Open();
        anim.SetBool("CanActivate", canActivate);
        anim.SetTrigger("Open");
    }
}
