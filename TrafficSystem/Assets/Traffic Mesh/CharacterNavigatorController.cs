using System.Collections;
using UnityEngine;

public class CharacterNavigatorController : MonoBehaviour
{

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

    private Vector3 velocity;
    private Vector3 lastPosition;
    private Animator animator;
    private bool isCoroutineRunning = false;

    private bool _reachedDestination;
    private bool _isWait;

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


    private void Awake()
    {

        if(GetComponent<Animator>() == true)
            animator = GetComponent<Animator>();

        lastPosition = transform.position;
        destination = transform.position;

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
                        Quaternion targetRoatation = Quaternion.LookRotation(destinationDirection);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRoatation, rotationSpeed * Time.deltaTime);
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
                        var fwdDotProduct = Vector3.Dot(transform.forward, velocity);
                        var rightDotProduct = Vector3.Dot(transform.right, velocity);

                        animator.SetFloat("Horizontal", rightDotProduct);
                        animator.SetFloat("Forward", fwdDotProduct);

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
