using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sc_Digicode_Button : Sc_Interactable
{
    protected TextMeshPro myText => GetComponentInChildren<TextMeshPro>();
    protected Sc_Digicode digicode => FindObjectOfType<Sc_Digicode>();
    protected Animator anim => GetComponent<Animator>();

    private void Start()
    {
        canActivate = true;
    }

    public override void Activate(Sc_Character chara)
    {
        anim.SetTrigger("Push");
    }

    private void Update()
    {
        canActivate = !anim.GetCurrentAnimatorStateInfo(0).IsName("DigicodeButton_Push") && digicode.maxSize;
    }
}
