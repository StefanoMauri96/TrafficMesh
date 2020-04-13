using System.Collections;
using UnityEngine;

public class CharacterNavigatorController : MonoBehaviour
{

    public float fwdDotProduct;
    public float rightDotProduct;

    #region Private

    [Header("Only Debug")]
    [SerializeField]
    private Vector3 destination;
    [SerializeField]
    private float stopDistance;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private bool startOnPlay = false;
    [SerializeField]
    private TrafficFlocking trafficFlocking;
    [SerializeField]
    private bool useFlocking = true;

    private Vector3 velocity;
    private Vector3 lastPosition;
    private Animator animator;
    private bool isCoroutineRunning = false;

    private bool _reachedDestination;
    private bool _isWait;
    private float initMovementSpeed;
    private bool a;

    #endregion

    #region Public

    public bool reachedDestination
    {

        get { return _reachedDestination; }

    }

    public bool isWait
    {

        set { _isWait = value; }
        get { return _isWait; }

    }

    #endregion


    private void Start()
    {

        if(GetComponent<Animator>() == true)
            animator = GetComponent<Animator>();

        lastPosition = transform.position;

        if (startOnPlay == true)
        {

            _isWait = false;
            StartCoroutine(CharacterNavigatorLoop());

        }
        else
        {
            _isWait = true;

        }

    }
    

    #region Public Functions

    public void SetDestination(Vector3 newDestination)
    {

        destination = newDestination;
        _reachedDestination = false;

    }

    public void StopMovement()
    {

        StopAllCoroutines();

    }

    public void StartMovement()
    {

        if(isCoroutineRunning == false)
        {

            _isWait = false;
            StartCoroutine(CharacterNavigatorLoop());

        }

    }

    public void SetStopDistance(float value)
    {

        if (value < 0)
            value = 0;

        stopDistance = value;

    }

    public void SetRotationSpeed(float value)
    {

        if (value < 0)
            value = 0;

        rotationSpeed = value;

    }

    public void SetMovementSpeed(float value)
    {

        if (value < 0)
            value = 0.1f;

        movementSpeed = value;
        initMovementSpeed = movementSpeed;

    }

    #endregion

    #region Private

    private void ApplyFlockingRules()
    {

        float distance;
        int groupSize = 0;
        Vector3 vcenter = Vector3.zero;
        Vector3 vavoid = Vector3.zero;

        foreach (GameObject go in trafficFlocking.TrafficPeople)
        {

            if (go != this.gameObject)
            {

                distance = Vector3.Distance(go.transform.position, this.transform.position);

                if (distance <= trafficFlocking.neighbourDistance)
                {

                    vcenter += go.transform.position;
                    groupSize++;

                    if (distance < 1f)
                    {

                        vavoid = vavoid + (this.transform.position - go.transform.position);


                    }

                    //CharacterNavigatorController anotherTrafficPerson = go.GetComponent<CharacterNavigatorController>();
                    //movementSpeed = movementSpeed + anotherTrafficPerson.movementSpeed;

                }

            }

        }

        if (groupSize > 0)
        {

            a = false;
            vcenter = vcenter / groupSize + (destination - this.transform.position);
            //movementSpeed += 0.05f;

            Vector3 direction = (vcenter + vavoid) - transform.position;

            if (direction != Vector3.zero)
            {

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(direction),
                    movementSpeed*2 * Time.deltaTime);

            }

        }
        else
        {

            a = true;

        }

    }

    #endregion

    #region Coroutine

    private IEnumerator CharacterNavigatorLoop()
    {

        isCoroutineRunning = true;

        while (isCoroutineRunning == true)
        {

            if (_isWait == false)
            {

                if (transform.position != destination)
                {

                    Vector3 destinationDirection = destination - transform.position;
                    destinationDirection.y = 0;

                    float destinationDistance = destinationDirection.magnitude;

                    if (destinationDistance >= stopDistance)
                    {
                        _reachedDestination = false;

                        ApplyFlockingRules();
                        if(a == true)
                        {

                            movementSpeed = initMovementSpeed;
                            Quaternion targetRoatation = Quaternion.LookRotation(destinationDirection);
                            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRoatation, rotationSpeed * Time.deltaTime);

                        }
                      
                        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);

                    }
                    else
                    {

                        _reachedDestination = true;

                    }


                    if (animator != null)
                    {

                        velocity = (transform.position - lastPosition) / Time.deltaTime;
                        velocity.y = 0;
                        var velocityMagitude = velocity.magnitude;
                        velocity = velocity.normalized;
                        fwdDotProduct = Vector3.Dot(transform.forward, velocity);
                        rightDotProduct = Vector3.Dot(transform.right, velocity);

                        animator.SetFloat("Turn", rightDotProduct, 0.1f, Time.deltaTime);
                        animator.SetFloat("Forward", movementSpeed, 0.1f, Time.deltaTime);

                        lastPosition = transform.position;

                    }


                }
                else
                {

                    _reachedDestination = false;

                }

            }

            yield return null;

        }

        isCoroutineRunning = false;
        yield return null;

    }

    #endregion

}
