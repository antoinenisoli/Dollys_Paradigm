using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Interactable : MonoBehaviour
{
    public MeshOutline outline => GetComponentInChildren<MeshOutline>();

    public bool canActivate;
    [SerializeField] protected bool activated;

    private void Awake()
    {
        outline.enabled = false;
    }

    public virtual void Open(float delay)
    {
        
    }

    public virtual void ResetMachine()
    {
        canActivate = false;
    }

    public virtual void Activate(Sc_Character chara)
    {
        if (activated && !canActivate)
            return;

        activated = true;
    }
}
