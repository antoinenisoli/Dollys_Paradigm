using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_EventManager : MonoBehaviour
{
    public static Sc_EventManager current;

    private void Awake()
    {
        current = this;
    }

    public event Action onGlobalRespawn;
    public event Action onGameOver;

    public void AllRespawn()
    {
        onGlobalRespawn?.Invoke();
    }

    public void GameOverScreen()
    {
        onGameOver?.Invoke();
    }
}
