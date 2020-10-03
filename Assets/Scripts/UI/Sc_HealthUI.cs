using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sc_HealthUI : MonoBehaviour
{
    Sc_PlayerController player => FindObjectOfType<Sc_PlayerController>();
    Text myText => GetComponent<Text>();

    private void Update()
    {
        myText.text = player.Health.CurrentHealth + "";
    }
}
