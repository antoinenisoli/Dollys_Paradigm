using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sc_OverlayUI : MonoBehaviour
{
    Sc_PlayerController player => FindObjectOfType<Sc_PlayerController>();
    Sc_GameManager gm => FindObjectOfType<Sc_GameManager>();

    [SerializeField] Text healthText;
    Material glowText;
    [SerializeField] Gradient gr;
    [SerializeField] CanvasGroup overlays;

    [SerializeField] Text roomIndicator;

    private void Start()
    {
        glowText = healthText.material;
    }

    private void Update()
    {
        healthText.text = player.Health.CurrentHealth + " / " + player.Health.MaxHealth;
        float healthValue = (float)player.Health.CurrentHealth/player.Health.MaxHealth;
        Color col = gr.Evaluate(healthValue);
        glowText.SetColor("_EmissionColor", col);
        overlays.alpha = 1 - healthValue;
        overlays.alpha = Mathf.Clamp(overlays.alpha, 0, 0.75f);
        roomIndicator.text = "Room #" + gm.roomIndex;
    }
}
