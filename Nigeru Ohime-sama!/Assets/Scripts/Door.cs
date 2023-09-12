using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject goal;
    public bool isFinalDoor;
    void Update()
    {
        GameObject[] gems = GameObject.FindGameObjectsWithTag("Gem");
        
        if (gems.Length <= 0)
        {
            if(isFinalDoor)
            {
                // Debug.Log("Gems");
                this.gameObject.SetActive(false);
                goal.tag = "End";
            }
            else
            {
                // Debug.Log("Gems");
                this.gameObject.SetActive(false);
                goal.tag = "Finish";
            }
            
        }
    }
}
