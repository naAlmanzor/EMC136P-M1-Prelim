using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PatrolPoints : MonoBehaviour
{
    [Range (0f, 2f)]
    [SerializeField] private float waypointSize = 1f;
    [SerializeField] public GameObject[] waypointList;

    private void OnDrawGizmos(){
        
        if(transform.childCount != 0) {
            foreach(Transform t in transform)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(new Vector3(t.position.x, t.position.y, 0), waypointSize);
            }
            
            Gizmos.color = Color.red;

            for(int i = 0; i < transform.childCount - 1; i++){
                Gizmos.DrawLine(new Vector3(transform.GetChild(i).position.x, transform.GetChild(i).position.y, 0),
                                new Vector3(transform.GetChild(i+1).position.x, transform.GetChild(i+1).position.y, 0));
            }
        }
    }

    public Transform GetNextWaypoint(Transform waypoint)
    { 
        if(waypoint == null)
        {
            return transform.GetChild(0);
        }

        int nextIndex = waypoint.GetSiblingIndex() + 1;

        if(nextIndex >= transform.childCount)
        {
            return transform.GetChild(0);
        }

        return transform.GetChild(nextIndex);
    }

    public void RemoveLastWaypoint()
    {
        if(transform.childCount > 1)
        {
            Destroy(transform.GetChild(transform.childCount - 1).gameObject);
        }
        else Debug.Log("Can't destroy starting position");
        
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PatrolPoints))]
public class WaypointsListEditor : Editor
{
    int i;

    public override void OnInspectorGUI()
    {
        var thisTarget = (PatrolPoints)target;
        if(thisTarget == null) return;

        DrawDefaultInspector();

        i = 0;
        if(GUILayout.Button("Add All Nodes")){
            thisTarget.waypointList = new GameObject[thisTarget.transform.childCount];

            foreach(Transform child in thisTarget.transform)
            {
              thisTarget.waypointList[i++] = child.gameObject;
            }
            
        }
    }
}
#endif
