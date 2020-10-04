using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Health : MonoBehaviour
{
    Sc_Character chara => GetComponent<Sc_Character>();
    public bool isDead;
    [SerializeField] int currentHealth = 1;
    [SerializeField] int maxHealth = 1;

    public int MaxHealth
    {
        get => maxHealth;
        set
        {
            if (value < 0)
            {
                value = 0;
            }

            maxHealth = value;
        }
    }

    public int CurrentHealth
    {
        get => currentHealth;
        set
        {
            if (value <= 0)
            {
                if (!isDead)
                {
                    isDead = true;
                    chara.Death();
                }
                else
                {
                    value = 0;
                }
            }

            if (value > MaxHealth)
            {
                value = MaxHealth;
            }

            currentHealth = value;
        }
    }

    public void TakeDamages(int dmg)
    {
        CurrentHealth -= dmg;
    }
}
