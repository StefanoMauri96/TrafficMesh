using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficFlocking : MonoBehaviour
{
    [Header("Rules")]
    [Range(0.1f, 10f)]
    public float neighbourDistance;

    public List<GameObject> TrafficPeople = new List<GameObject>();

}
