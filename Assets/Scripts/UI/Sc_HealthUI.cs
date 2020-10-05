using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sc_HealthUI : MonoBehaviour
{
    Sc_PlayerController player => FindObjectOfType<Sc_PlayerController>();
    [SerializeField] Text healthText;
    Material glowText;

    [SerializeField] Gradient gr;
    [SerializeField] CanvasGroup overlays;

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
    }
}
