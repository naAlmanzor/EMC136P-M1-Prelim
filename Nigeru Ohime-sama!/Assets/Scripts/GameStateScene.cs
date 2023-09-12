using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateScene : MonoBehaviour
{
    [SerializeField] GameStats gameStats;
    [SerializeField] GameObject gameOver, gameClear;
    // Start is called before the first frame update
    void Start()
    {
        if(gameStats.state == "lose")
        {
            gameOver.SetActive(true);
            gameClear.SetActive(false);
        }

        if(gameStats.state == "win")
        {
            gameOver.SetActive(false);
            gameClear.SetActive(true);
        }
    }

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
}
