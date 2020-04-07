using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrafficMesh : MonoBehaviour
{

    #region Public

    public List<WayPoint> wayPoints = new List<WayPoint>();
    public Transform wayPointRoot;

    #endregion

    #region Public Functions

    public void RemoveWayPointFromBraches(WayPoint wayPoint)
    {

        foreach(WayPoint w in wayPoints)
        {

            if(w.branches.Contains(wayPoint))
            {

                w.branches.Remove(wayPoint);

            }

        }

    }

    public void OrderWayPoints()
    {

        for(int i=0; i<wayPoints.Count; i++)
        {

            wayPoints[i] = wayPointRoot.GetChild(i).GetComponent<WayPoint>();

        }

    }

    #endregion

}
