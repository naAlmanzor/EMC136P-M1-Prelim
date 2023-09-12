using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [Header("Game Stats")]
    [SerializeField] private GameStats gameStats;

    [SerializeField] private GameObject[] hearts;
    [SerializeField] private Image stamina;
    [SerializeField] private TextMeshProUGUI objText;
    
    private void Update()
    {
        //Objective Stuff
        if(GameObject.FindGameObjectWithTag("Finish"))
        {
            gameStats.objective = "Find the Exit";
        }
        else
        {
            gameStats.objective = "Find the Gem";
        }

        objText.text = gameStats.objective;

        //Health Stuff
        switch(gameStats.playerHealth)
        {
            case 3:
                hearts[2].SetActive(true);
                hearts[1].SetActive(true);
                hearts[0].SetActive(true);
                break;
            case 2:
                hearts[2].SetActive(false);
                hearts[1].SetActive(true);
                hearts[0].SetActive(true);
                break;
            case 1:
                hearts[2].SetActive(false);
                hearts[1].SetActive(false);
                hearts[0].SetActive(true);
                break;
            case 0:
                hearts[2].SetActive(false);
                hearts[1].SetActive(false);
                hearts[0].SetActive(false);
                gameStats.state = "lose";
                SceneManager.LoadScene(1);
                break;
        }

        //Stamina Stuff
        stamina.fillAmount = gameStats.playerStamina/100;
    }
}
