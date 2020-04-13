using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WayPointManagerWindow : EditorWindow
{

    #region Private

    private int tab = 0;
    private GUIContent[] buttons = new GUIContent[]
    {

        new GUIContent("Actions"),
        new GUIContent("List"),
        new GUIContent("Settings")
    };
    private Vector2 scrollPosition = Vector2.zero;
    private GameObject lastWayPointSelected;
    private int indexSearchWayPoint;
    private Color foundWayPointColor = Color.red;
    private TrafficStyle trafficStyle;
    private TrafficMesh trafficMesh;
    string fileName = "Traffic Mesh";

    GUIStyle h1;
    GUIStyle h2;

    #endregion


    [MenuItem("Tools/Traffic system")]
    public static void Open()
    {

        GetWindow<WayPointManagerWindow>("Traffic system");

    }

    private void OnEnable()
    {

        h1 = new GUIStyle();
        h1.fontSize = 18;
        h1.fontStyle = FontStyle.Bold;
        h1.normal.textColor = Color.white;
        h1.alignment = TextAnchor.MiddleCenter;

        h2 = new GUIStyle();
        h2.fontSize = 13;
        h2.fontStyle = FontStyle.Bold;
        h2.normal.textColor = Color.white;
        h2.alignment = TextAnchor.MiddleLeft;
        h2.padding = new RectOffset(10, 0, 0, 0);

        //InitFolders();

    }

    private void OnGUI()
    {

        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("Traffic system",h1);
        EditorGUILayout.Space(30);
        trafficMesh = (TrafficMesh)EditorGUILayout.ObjectField("Traffic Mesh", trafficMesh, typeof(TrafficMesh), true);
        EditorGUILayout.Space(20);

        if (trafficMesh == null)
        {

            EditorGUILayout.HelpBox("First of all assign a Traffic Mesh or creat new Traffic Mesh", MessageType.Warning);

            DrawInitMenu();

        }
        else
        {

            tab = GUILayout.Toolbar(tab, buttons);
            EditorGUILayout.Separator();

            switch (tab)
            {

                case 0:
                    EditorGUILayout.BeginVertical("box");
                    DrawActions();
                    EditorGUILayout.EndVertical();
                    break;
                case 1:
                    DrawList();
                    break;
                case 2:
                    trafficStyle = trafficMesh.wayPointRoot.GetComponent<TrafficStyle>();
                    DrawSettings();
                    break;


            }

        }

    }


    #region Menu

    private void DrawInitMenu()
    {

        
        fileName = GUILayout.TextField(fileName);

        if (GUILayout.Button("Creat new Traffic Mesh", GUILayout.Height(30)))
        {

            CreateNewTrafficMeshOnScene(fileName);

        }


    }

    private void DrawActions()
    {

        if(GUILayout.Button("Creat way point",GUILayout.Height(30)))
        {

            CreateWayPoint();           

        }
        if(Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<WayPoint>() == true)
        {
           
            GUILayout.Space(10);
            if (GUILayout.Button("Creat way point before",GUILayout.Height(30)))
            {

                CreateWayPointBefore();
                UpdateWayPointsName();

            }
            GUILayout.Space(10);
            if (GUILayout.Button("Creat way point after",GUILayout.Height(30)))
            {

                CreateWayPointAfter();
                UpdateWayPointsName();

            }
            GUILayout.Space(10);
            if (GUILayout.Button("Creat branch with return", GUILayout.Height(30)))
            {

                CreateBranch(true);
                UpdateWayPointsName();

            }
            GUILayout.Space(10);
            if (GUILayout.Button("Creat branch", GUILayout.Height(30)))
            {

                CreateBranch(false);
                UpdateWayPointsName();

            }
            GUILayout.Space(10);
            if (GUILayout.Button("Remove way point", GUILayout.Height(30)))
            {

                RemoveWayPoint();
                UpdateWayPointsName();

            }
            GUILayout.Space(10);
            if (GUILayout.Button("Close loop", GUILayout.Height(30)))
            {

                CloseLoop();

            }
        }

    }

    private void DrawList()
    {

        EditorGUILayout.Space(10);
        indexSearchWayPoint = EditorGUILayout.IntField("Search index", indexSearchWayPoint);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        for (int i=0; i< trafficMesh.wayPoints.Count; i++)
        {

            if (indexSearchWayPoint == 0)
            {

                if (Selection.activeGameObject == trafficMesh.wayPointRoot.GetChild(i).gameObject)
                {

                    h2.normal.textColor = foundWayPointColor;

                    if (lastWayPointSelected != Selection.activeGameObject)
                    {

                        lastWayPointSelected = Selection.activeGameObject;
                        scrollPosition = new Vector2(0, 160 * i + 1);

                    }

                }
                else
                {

                    h2.normal.textColor = Color.white;

                }

                
                EditorGUILayout.BeginVertical("GroupBox");
                EditorGUILayout.Space(20);
                EditorGUILayout.LabelField(trafficMesh.wayPointRoot.GetChild(i).name, h2);
                EditorGUILayout.Space(5);
                SerializedObject obj = new SerializedObject(trafficMesh.wayPoints[i]);
                EditorGUILayout.PropertyField(obj.FindProperty("nextWayPoint"));
                EditorGUILayout.PropertyField(obj.FindProperty("previousWayPoint"));
                EditorGUILayout.PropertyField(obj.FindProperty("width"));
                EditorGUILayout.PropertyField(obj.FindProperty("branchRatio"));
                EditorGUILayout.PropertyField(obj.FindProperty("isClose"));
                EditorGUILayout.Space(20);
                EditorGUILayout.EndVertical();
                obj.ApplyModifiedProperties();

            }
            else
            {

                if (trafficMesh.wayPointRoot.GetChild(i).name == "WayPoint "+indexSearchWayPoint.ToString())
                {

                    h2.normal.textColor = Color.red;
                    EditorGUILayout.Space(20);
                    EditorGUILayout.LabelField(trafficMesh.wayPointRoot.GetChild(i).name, h2);
                    EditorGUILayout.Space(5);
                    SerializedObject obj = new SerializedObject(trafficMesh.wayPoints[i]);
                    EditorGUILayout.PropertyField(obj.FindProperty("nextWayPoint"));
                    EditorGUILayout.PropertyField(obj.FindProperty("previousWayPoint"));
                    EditorGUILayout.PropertyField(obj.FindProperty("width"));
                    EditorGUILayout.PropertyField(obj.FindProperty("branchRatio"));
                    EditorGUILayout.Space(20);
                    obj.ApplyModifiedProperties();
                }


            }
           

        }

        EditorGUILayout.EndScrollView();

    }

    private void DrawSettings()
    {

        h2.normal.textColor = Color.white;

        EditorGUILayout.LabelField("Scene settings", h2);
        EditorGUILayout.Space(10);
        trafficStyle.meshColor = EditorGUILayout.ColorField("Mesh color", trafficStyle.meshColor);
        trafficStyle.wayPointColor = EditorGUILayout.ColorField("Way point color", trafficStyle.wayPointColor);
        trafficStyle.branchColor = EditorGUILayout.ColorField("Branch color", trafficStyle.branchColor);
        trafficStyle.haveWayPointLabels = EditorGUILayout.Toggle("Show way point label", trafficStyle.haveWayPointLabels);

        if(trafficStyle.haveWayPointLabels == true)
            trafficStyle.labelWayPointColor = EditorGUILayout.ColorField("Label way point color", trafficStyle.labelWayPointColor);

        trafficStyle.haveBranchProbabilityLabel = EditorGUILayout.Toggle("Show branch label", trafficStyle.haveBranchProbabilityLabel);

        if (trafficStyle.haveBranchProbabilityLabel == true)
        {
            trafficStyle.labelBranchProbabilityColor = EditorGUILayout.ColorField("Label branch color", trafficStyle.labelBranchProbabilityColor);
            trafficStyle.labelBanchFontSize = EditorGUILayout.IntSlider("Label branch font size",trafficStyle.labelBanchFontSize, 10,50);
        }

        trafficStyle.rotationWayPointColor = EditorGUILayout.ColorField("Rotation way point color", trafficStyle.rotationWayPointColor);

        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Editor settings", h2);
        EditorGUILayout.Space(10);
        foundWayPointColor = EditorGUILayout.ColorField("Found way point color", foundWayPointColor);

    }

    #endregion

    #region Funtions

    private void InitFolders()
    {

        if(AssetDatabase.IsValidFolder("Assets/Traffic System") == false)
        {

            AssetDatabase.CreateFolder("Assets", "Traffic System");
            AssetDatabase.CreateFolder("Assets/Traffic System", "TrafficMesh");
            AssetDatabase.SaveAssets();

        }
       
    }

    private void CreateNewTrafficMeshOnScene(string name)
    {

        GameObject root = new GameObject(fileName);
        trafficMesh = root.AddComponent<TrafficMesh>();
        trafficStyle = root.AddComponent<TrafficStyle>();
        root.name = name;

        trafficMesh.wayPointRoot = root.transform;

    }

    private void CreateTrafficMeshAsScriptableObject(string fileName)
    {

        /*trafficMesh = ScriptableObject.CreateInstance<TrafficMesh>();
        AssetDatabase.CreateAsset(trafficMesh, "Assets/Traffic System/TrafficMesh/" + fileName + ".asset");
        AssetDatabase.SaveAssets();

        GameObject root = new GameObject(fileName);
        root.AddComponent<TrafficStyle>();

        trafficMesh.wayPointRoot = root.transform;*/

    }

    private void CreateWayPoint()
    {

        GameObject wayPointObject = new GameObject("WayPoint "+ trafficMesh.wayPointRoot.childCount,typeof(WayPoint));
        wayPointObject.transform.SetParent(trafficMesh.wayPointRoot, false);

        WayPoint wayPoint = wayPointObject.GetComponent<WayPoint>();
        trafficMesh.wayPoints.Add(wayPoint);

        if (trafficMesh.wayPoints.Count > 1)
        {

            wayPoint.previousWayPoint = trafficMesh.wayPointRoot.GetChild(trafficMesh.wayPointRoot.childCount - 2).GetComponent<WayPoint>();
            wayPoint.previousWayPoint.nextWayPoint = wayPoint;

            //Place the way point at the last position
            wayPoint.transform.position = wayPoint.previousWayPoint.transform.position;
            wayPoint.transform.forward = wayPoint.previousWayPoint.transform.forward;

        }

        Selection.activeGameObject = wayPoint.gameObject;

    }

    private void CreateWayPointBefore()
    {

        GameObject wayPointObject = new GameObject("Temp", typeof(WayPoint));
        wayPointObject.transform.SetParent(trafficMesh.wayPointRoot, false);

        WayPoint newWayPoint = wayPointObject.GetComponent<WayPoint>();
        trafficMesh.wayPoints.Add(newWayPoint);

        WayPoint selectedWayPoint = Selection.activeGameObject.GetComponent<WayPoint>();

        wayPointObject.transform.position = selectedWayPoint.transform.position;
        wayPointObject.transform.forward = selectedWayPoint.transform.forward;

        if(selectedWayPoint.previousWayPoint != null)
        {

            newWayPoint.previousWayPoint = selectedWayPoint.previousWayPoint;
            selectedWayPoint.previousWayPoint.nextWayPoint = newWayPoint;

        }

        newWayPoint.nextWayPoint = selectedWayPoint;
        selectedWayPoint.previousWayPoint = newWayPoint;

        newWayPoint.transform.SetSiblingIndex(selectedWayPoint.transform.GetSiblingIndex());
        trafficMesh.OrderWayPoints();

        Selection.activeGameObject = newWayPoint.gameObject;

    }

    private void CreateWayPointAfter()
    {

        GameObject wayPointObject = new GameObject("Temp", typeof(WayPoint));
        wayPointObject.transform.SetParent(trafficMesh.wayPointRoot, false);

        WayPoint newWayPoint = wayPointObject.GetComponent<WayPoint>();
        trafficMesh.wayPoints.Add(newWayPoint);

        WayPoint selectedWayPoint = Selection.activeGameObject.GetComponent<WayPoint>();

        wayPointObject.transform.position = selectedWayPoint.transform.position;
        wayPointObject.transform.forward = selectedWayPoint.transform.forward;

        newWayPoint.previousWayPoint = selectedWayPoint;
        
        if(selectedWayPoint.nextWayPoint != null)
        {

            selectedWayPoint.nextWayPoint.previousWayPoint = newWayPoint;
            newWayPoint.nextWayPoint = selectedWayPoint.nextWayPoint;

        }

        selectedWayPoint.nextWayPoint = newWayPoint;

        newWayPoint.transform.SetSiblingIndex(selectedWayPoint.transform.GetSiblingIndex()+1);
        trafficMesh.OrderWayPoints();

        Selection.activeGameObject = newWayPoint.gameObject;

    }

    private void RemoveWayPoint()
    {

        WayPoint selectedWayPoint = Selection.activeGameObject.GetComponent<WayPoint>();

        if(selectedWayPoint.nextWayPoint != null)
        {

            selectedWayPoint.nextWayPoint.previousWayPoint = selectedWayPoint.previousWayPoint;

        }
        if(selectedWayPoint.previousWayPoint != null)
        {

            selectedWayPoint.previousWayPoint.nextWayPoint = selectedWayPoint.nextWayPoint;
            Selection.activeGameObject = selectedWayPoint.previousWayPoint.gameObject;

        }

        trafficMesh.RemoveWayPointFromBraches(selectedWayPoint);
        trafficMesh.wayPoints.Remove(selectedWayPoint);
        DestroyImmediate(selectedWayPoint.gameObject);

    }

    private void CreateBranch(bool haveReturn)
    {

        WayPoint branchedFrom = Selection.activeGameObject.GetComponent<WayPoint>();

        GameObject wayPointObject = new GameObject("WayPoint " + trafficMesh.wayPointRoot.childCount, typeof(WayPoint));
        wayPointObject.transform.SetParent(trafficMesh.wayPointRoot, false);

        WayPoint wayPoint = wayPointObject.GetComponent<WayPoint>();
        trafficMesh.wayPoints.Add(wayPoint);
       
        if(wayPoint.branches == null)
        {

            wayPoint.branches = new List<WayPoint>();

        }

        if (branchedFrom.branches == null)
        {

            branchedFrom.branches = new List<WayPoint>();

        }

        branchedFrom.branches.Add(wayPoint);

        if(haveReturn == true)
            wayPoint.branches.Add(branchedFrom);

        wayPoint.transform.position = branchedFrom.transform.position;
        wayPoint.transform.forward = branchedFrom.transform.forward;

        Selection.activeGameObject = wayPoint.gameObject;

    }

    private void CloseLoop()
    {

        WayPoint firstWayPoint = trafficMesh.wayPoints[0];
        WayPoint lastWayPoint = trafficMesh.wayPoints[trafficMesh.wayPoints.Count-1];
        firstWayPoint.previousWayPoint = lastWayPoint;
        lastWayPoint.nextWayPoint = firstWayPoint;

    }

    private void UpdateWayPointsName()
    {

        for(int i=0; i<trafficMesh.wayPoints.Count; i++)
        {

            trafficMesh.wayPointRoot.transform.GetChild(i).name = "WayPoint " + i;

        }

    }

    #endregion

    #region Utility

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    #endregion

}
