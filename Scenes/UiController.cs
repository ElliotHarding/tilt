using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiController : MonoBehaviour
{
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }

    public void Home()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Home");
    }
}
