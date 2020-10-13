using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sc_Digicode_Button_Slot : Sc_Digicode_Button
{
    public override void Activate(Sc_Character chara)
    {
        if (digicode.canActivate)
            anim.SetTrigger("Push");

        digicode.AddCode(myText.text);
    }
}
