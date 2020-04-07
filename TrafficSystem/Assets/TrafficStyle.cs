using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficStyle : MonoBehaviour
{

    #region Public

    public Color meshColor = Color.white;
    public Color wayPointColor = Color.black;
    public Color branchColor = Color.blue;
    public Color labelWayPointColor = Color.white;
    public Color rotationWayPointColor = Color.black;
    public Color labelBranchProbabilityColor = Color.white;
    [Range(10, 50)]
    public int labelBanchFontSize = 20;
    public bool haveWayPointLabels = false;
    public bool haveBranchProbabilityLabel = false;

    #endregion

}
