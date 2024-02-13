using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{

    [SerializeField] const float wayPointGizmos = 0.3f;
    private void OnDrawGizmos()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            int j = GetNextPoint(i);
            Gizmos.DrawSphere(GetwayPoint(i), wayPointGizmos);
            Gizmos.DrawLine(GetwayPoint(i), GetwayPoint(j));
        }
    }

        public int GetNextPoint(int i)
    {
        
        return i + 1>=transform.childCount?0:i+1;
    }


    public int GetWaypointCount()
    {
        return transform.childCount;
    }

    public Vector3 GetwayPoint(int i)
    {
        return transform.GetChild(i).position;
    }
}
