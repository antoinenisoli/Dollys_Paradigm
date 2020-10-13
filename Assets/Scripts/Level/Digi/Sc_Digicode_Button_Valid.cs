using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Digicode_Button_Valid : Sc_Digicode_Button
{
    public override void Activate(Sc_Character chara)
    {
        base.Activate(chara);
        digicode.CheckCode();
    }
}
