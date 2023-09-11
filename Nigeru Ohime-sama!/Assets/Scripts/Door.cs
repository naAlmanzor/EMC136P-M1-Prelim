using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject goal;
    void Update()
    {
        GameObject[] gems = GameObject.FindGameObjectsWithTag("Gem");
        
        if (gems.Length <= 0)
        {
            // Debug.Log("Gems");
            this.gameObject.SetActive(false);
            // GameObject.FindGameObjectWithTag("Finish").SetActive(true);
            goal.tag = "Finish";
        }
    }
}
