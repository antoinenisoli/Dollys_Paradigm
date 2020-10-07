using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Interactable : MonoBehaviour
{
    public bool canActivate;
    protected bool activated;

    public virtual void Open(float delay)
    {
        canActivate = true;
    }

    public virtual void Activate(Sc_Character chara)
    {
        if (activated && !canActivate)
            return;

        activated = true;
    }
}
