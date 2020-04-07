using UnityEngine;
using UnityEditor;

[InitializeOnLoad()]
public class WayPointEditor
{

    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmo(WayPoint wayPoint, GizmoType gizmoType)
    {

        TrafficStyle trafficStyle = wayPoint.transform.parent.GetComponent<TrafficStyle>();

        if (trafficStyle.haveWayPointLabels == true)
        {
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.normal.textColor = trafficStyle.labelWayPointColor;
            Handles.Label(wayPoint.transform.position + new Vector3(0, 1, 0), wayPoint.transform.name,labelStyle);
        }

        if (wayPoint.nextWayPoint != null)
        {

            Mesh mesh = new Mesh();

            Vector3 posWayPoint = new Vector3(wayPoint.transform.position.x, wayPoint.transform.position.y + 0.02f, wayPoint.transform.position.z);
            Vector3 posNextWayPoint = new Vector3(wayPoint.nextWayPoint.transform.position.x, wayPoint.nextWayPoint.transform.position.y + 0.02f, wayPoint.nextWayPoint.transform.position.z);

            Vector3[] vertices = new Vector3[4];
            vertices[0] = posWayPoint + wayPoint.transform.right * wayPoint.width / 2f;
            vertices[1] = posWayPoint - wayPoint.transform.right * wayPoint.width / 2f;
            vertices[2] = posNextWayPoint - wayPoint.nextWayPoint.transform.right * wayPoint.nextWayPoint.width / 2f;
            vertices[3] = posNextWayPoint + wayPoint.nextWayPoint.transform.right * wayPoint.nextWayPoint.width / 2f;
            mesh.vertices = vertices;
            int[] triangles = new int[]
            {

                0,
                1,
                2,
                2,
                3,
                0
            };
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            Gizmos.color = trafficStyle.meshColor;
            Gizmos.DrawMesh(mesh);
        }


        Gizmos.color = trafficStyle.rotationWayPointColor;
        Gizmos.DrawLine(wayPoint.transform.position + (wayPoint.transform.right * wayPoint.width / 2f), wayPoint.transform.position - (wayPoint.transform.right * wayPoint.width / 2f));
        Gizmos.DrawSphere(wayPoint.transform.position - (wayPoint.transform.right * wayPoint.width / 2f), 0.03f);
        Gizmos.DrawSphere(wayPoint.transform.position + (wayPoint.transform.right * wayPoint.width / 2f), 0.03f);

        if ((gizmoType & GizmoType.Selected) != 0)
        {

            Gizmos.color = trafficStyle.wayPointColor * 0.5f;
           

        }
        else
        {

            Gizmos.color = trafficStyle.wayPointColor;

        }

        Gizmos.DrawSphere(wayPoint.transform.position, 0.1f);

        

        /*if(wayPoint.previousWayPoint != null)
        {

            Gizmos.color = Color.red;
            Vector3 offset = wayPoint.transform.right * wayPoint.width / 2f;
            Vector3 offsetTo = wayPoint.previousWayPoint.transform.right * wayPoint.previousWayPoint.width / 2f;

            Gizmos.DrawLine(wayPoint.transform.position + offset, wayPoint.previousWayPoint.transform.position + offsetTo);

        }
        if(wayPoint.nextWayPoint != null)
        {

            Gizmos.color = Color.green;
            Vector3 offset = wayPoint.transform.right * wayPoint.width / 2f;
            Vector3 offsetTo = wayPoint.nextWayPoint.transform.right * wayPoint.nextWayPoint.width / 2f;

            Gizmos.DrawLine(wayPoint.transform.position - offset, wayPoint.nextWayPoint.transform.position - offsetTo);

        }*/
  

        if (wayPoint.branches != null)
        {

            foreach (WayPoint branch in wayPoint.branches)
            {

                if (trafficStyle.haveBranchProbabilityLabel == true)
                {
                    GUIStyle labelStyle = new GUIStyle();
                    labelStyle.fontSize = trafficStyle.labelBanchFontSize;
                    labelStyle.normal.textColor = trafficStyle.labelBranchProbabilityColor;
                    Handles.Label(wayPoint.transform.position - (wayPoint.transform.position - branch.transform.position) /3 + new Vector3(0, 1, 0), (wayPoint.branchRatio*100/wayPoint.branches.Count).ToString()+"%", labelStyle);

                }

                //DrawArrow.ForGizmo(wayPoint.transform.position - (wayPoint.transform.position - branch.transform.position) / 3, );
                //Handles.ArrowCap(0, wayPoint.transform.position, Quaternion.FromToRotation(Vector3.Normalize(branch.transform.position),Vector3.Normalize(branch.transform.position)), 2);

                /*if (branch.transform.position.z > wayPoint.transform.position.z)
                {
                    Gizmos.color = trafficStyle.branchColor;
                    Gizmos.DrawLine(wayPoint.transform.position - (wayPoint.transform.position - branch.transform.position) / 3, wayPoint.transform.position - (wayPoint.transform.position - branch.transform.position) / 3 - new Vector3(0.5f, 0, 0.5f) + direction);
                    Gizmos.DrawLine(wayPoint.transform.position - (wayPoint.transform.position - branch.transform.position) / 3, wayPoint.transform.position - (wayPoint.transform.position - branch.transform.position) / 3 - new Vector3(-0.5f, 0, 0.5f) + direction);
                }
                else
                {

                    Gizmos.color = trafficStyle.branchColor;
                    Gizmos.DrawLine(wayPoint.transform.position - (wayPoint.transform.position - branch.transform.position) / 3, wayPoint.transform.position - (wayPoint.transform.position - branch.transform.position) / 3 - new Vector3(0.5f, 0, -0.5f) + direction);
                    Gizmos.DrawLine(wayPoint.transform.position - (wayPoint.transform.position - branch.transform.position) / 3, wayPoint.transform.position - (wayPoint.transform.position - branch.transform.position) / 3 - new Vector3(-0.5f, 0, -0.5f) + direction);

                }*/
                
                Gizmos.color = trafficStyle.branchColor;
                Gizmos.DrawLine(wayPoint.transform.position, branch.transform.position);

            }

        }
    }


}
