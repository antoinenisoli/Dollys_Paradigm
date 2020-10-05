using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sc_GameOverScreen : MonoBehaviour
{
    [SerializeField] GameObject gmScreen;

    private void Start()
    {
        Sc_EventManager.current.onGameOver += SwitchScreen;
        gmScreen.SetActive(false);
    }

    public void SwitchScreen()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gmScreen.SetActive(true);
    }

    public void Restart()
    {
        Sc_EventManager.current.AllRespawn();
        gmScreen.SetActive(false);
    }

    public void Leave()
    {
        SceneManager.LoadScene(0);
    }
}
