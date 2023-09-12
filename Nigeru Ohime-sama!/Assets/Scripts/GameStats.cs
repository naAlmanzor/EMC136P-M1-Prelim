using UnityEngine;

[CreateAssetMenu(fileName = "GameStats", menuName = "ScriptableObjects/GameStats", order = 1)]
public class GameStats : ScriptableObject
{
    public int playerHealth = 3;
    public string objective = "Find the Gem";
    public float playerStamina = 100;

    // You can add other game-related statistics here
}
