using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateScene : MonoBehaviour
{
    [SerializeField] GameStats gameStats;
    [SerializeField] GameObject gameOver, gameClear;
    // Start is called before the first frame update
    void Start()
    {
        if(gameStats.state == "lose")
        {
            AudioManager.instance.Play("Lose");
            gameOver.SetActive(true);
            gameClear.SetActive(false);
        }

        if(gameStats.state == "win")
        {
            gameOver.SetActive(false);
            gameClear.SetActive(true);
        }
    }
}
