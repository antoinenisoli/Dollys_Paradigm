using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sc_MainMenu : MonoBehaviour
{
    public void NextLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void LeaveGame()
    {
        Application.Quit();
    }
}
