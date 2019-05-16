using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene(2); //Load the next scene (usually Intro)
    }
    public void ExitApplication()
    {
        Application.Quit(); //Exit the application , shuts down game completely
    }
}
