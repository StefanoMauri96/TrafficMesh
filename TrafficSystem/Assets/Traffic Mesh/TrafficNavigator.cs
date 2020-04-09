using System.Collections;
using UnityEngine;

public class TrafficNavigator : MonoBehaviour
{

    #region Private

    [SerializeField]
    private WayPoint currentWayPoint;

    [SerializeField]
    private typeDirection direction;
    [SerializeField]
    private bool isRandomDirection = true;

    CharacterNavigatorController controller;
    private bool isStop = false;
    private bool _isArrived = false;

    #endregion

    #region Public

    public bool isArrived
    {

        get { return _isArrived; }

    }

    public enum typeDirection { clockwise, counterclockwise }

    #endregion


    void Awake()
    {

        controller = GetComponent<CharacterNavigatorController>();

        if (isRandomDirection == true)
            direction = RandomDirection();

        if (currentWayPoint != null)
        {          
            controller.SetDestination(currentWayPoint.GetPosition());
        }

        StartCoroutine(TrafficNavigatorLoop());

    }


    #region Public Functions

    public void Stop()
    {

        isStop = true;

    }

    public void SetFirstWayPoint(WayPoint value)
    {

        currentWayPoint = value;
        controller.SetDestination(currentWayPoint.GetPosition());

    }

    public void SetDirection(typeDirection nDirection)
    {

        direction = nDirection;

    }

    public typeDirection RandomDirection()
    {

        int randomValue = Random.Range(0, 2);

        if(randomValue == 0)
        {

            return typeDirection.clockwise;

        }
        else
        {

            return typeDirection.counterclockwise;

        }

    }

    #endregion

    #region Coroutine

    private IEnumerator TrafficNavigatorLoop()
    {

        while(isStop == false)
        {

            if (currentWayPoint == null)
                yield return null;

            if (controller.reachedDestination == true && currentWayPoint.isClose == false)
            {

                bool shouldBranch = false;

                if (currentWayPoint.branches != null && currentWayPoint.branches.Count > 0)
                {

                    shouldBranch = Random.Range(0f, 1f) <= currentWayPoint.branchRatio ? true : false;

                }

                if (shouldBranch == true)
                {

                    currentWayPoint = currentWayPoint.branches[Random.Range(0, currentWayPoint.branches.Count - 1)];

                }
                else
                {

                    if (direction == typeDirection.clockwise)
                    {

                        if (currentWayPoint.nextWayPoint != null)
                        {

                            currentWayPoint = currentWayPoint.nextWayPoint;

                        }
                        else
                        {

                            currentWayPoint = currentWayPoint.previousWayPoint;
                            direction = typeDirection.counterclockwise;

                        }

                    }
                    else if (direction == typeDirection.counterclockwise)
                    {

                        if (currentWayPoint.previousWayPoint != null)
                        {

                            currentWayPoint = currentWayPoint.previousWayPoint;

                        }
                        else
                        {

                            currentWayPoint = currentWayPoint.nextWayPoint;
                            direction = typeDirection.clockwise;

                        }

                    }

                }

                controller.SetDestination(currentWayPoint.GetPosition());

            }
            else if(currentWayPoint.isClose == true)
            {

                _isArrived = true;

            }


            //yield return new WaitForSeconds(0.05f);
            yield return null;

        }

        yield return null;

    }

    #endregion

}
