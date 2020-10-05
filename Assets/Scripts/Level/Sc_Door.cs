using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Door : Sc_Interactable
{
    [SerializeField] Sc_Door destination;
    public Transform spawnPos;

    public override void Activate(Sc_Character chara)
    {
        base.Activate(chara);

        chara.transform.position = destination.spawnPos.position;
    }
}
