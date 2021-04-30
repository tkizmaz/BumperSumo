using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void PlayGame()
    {
        //Loading MainGame Scene.
        SceneManager.LoadScene("MainGame");
    }

    public void QuitGame()
    {
        //Quitting the application.
        Application.Quit();
    }
}
