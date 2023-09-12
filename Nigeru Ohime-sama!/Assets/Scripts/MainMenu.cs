using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start() {
        if(AudioManager.instance.IsPlaying("GameTheme"))
        {
            AudioManager.instance.Stop("GameTheme");
        }

        if(AudioManager.instance.IsPlaying("Lose"))
        {
            AudioManager.instance.Stop("Lose");
        }
    }
    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void QuitGame()
    {
        Application.Quit(0);
    }
}
