using UnityEngine;
using System.Collections.Generic;

public class UI : MonoBehaviour
{
    [Header("Game Stats")]
    [SerializeField] private GameStats gameStats;
    
    private void Update() {
        if(GameObject.FindGameObjectWithTag("Finish"))
        {
            gameStats.objective = "Find the Exit";
        }
        else
        {
            gameStats.objective = "Find the Gem";
        }
    }
}
