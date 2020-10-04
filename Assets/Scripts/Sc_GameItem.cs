using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_GameItem : MonoBehaviour
{
    [SerializeField] protected int value = 5;

    public void OnTriggerEnter(Collider other)
    {
        Sc_PlayerController player = other.GetComponentInParent<Sc_PlayerController>();
        if (player)
        {
            Effect(player);
        }

        if (other.CompareTag("Solid"))
        {
            Destroy(gameObject);
        }
    }

    public virtual void Effect(Sc_Character chara)
    {
        chara.Hurt(value);
    }
}
