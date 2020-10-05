using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sc_MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject commandsDisplay;
    [SerializeField] GameObject teamDisplay;
    [SerializeField] Animator camAnim;

    private void Awake()
    {
        HideAll();
    }

    void HideAll()
    {
        commandsDisplay.SetActive(false);
        teamDisplay.SetActive(false);
    }

    public void ShowTeam()
    {
        teamDisplay.SetActive(!teamDisplay.activeSelf);
        mainMenu.SetActive(!mainMenu.activeSelf);
        camAnim.SetTrigger("SwitchTeam");
        camAnim.SetBool("ShowTeam", teamDisplay.activeSelf);
    }

    public void ShowCommands()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
        commandsDisplay.SetActive(!commandsDisplay.activeSelf);
    }

    public void Back()
    {
        mainMenu.SetActive(true);
        HideAll();
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void LeaveGame()
    {
        Application.Quit();
    }
}
