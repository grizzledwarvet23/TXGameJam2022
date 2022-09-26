using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject credits;
   public void PlayButton ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitButton ()
    {
        Application.Quit();
    }

    public void CreditsButton()
    {
        credits.SetActive(true);
    }

    public void Back()
    {
        credits.SetActive(false);
    }
}
