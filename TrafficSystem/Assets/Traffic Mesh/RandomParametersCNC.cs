using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterNavigatorController))]
public class RandomParametersCNC : MonoBehaviour
{

    #region Private

    [SerializeField]
    private Vector2 minMaxStopDestance = new Vector2(0.1f,0.5f);
    [SerializeField]
    private Vector2 minMaxRotationSpeed = new Vector2(10f, 120f);
    [SerializeField]
    private Vector2 minMaxMovementSpeed = new Vector2(0.2f,0.5f);

    CharacterNavigatorController characterNavigator;

    #endregion

    private void Awake()
    {

        characterNavigator = GetComponent<CharacterNavigatorController>();

    }

    void Start()
    {

        characterNavigator.SetMovementSpeed(Random.Range(minMaxMovementSpeed.x, minMaxMovementSpeed.y));
        characterNavigator.SetRotationSpeed(Random.Range(minMaxRotationSpeed.x, minMaxRotationSpeed.y));
        characterNavigator.SetStopDistance(Random.Range(minMaxStopDestance.x, minMaxStopDestance.y));

    }

}
