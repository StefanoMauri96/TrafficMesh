using System.Collections;
using UnityEngine;

public class CharacterNavigatorController : MonoBehaviour
{

    #region Private

    private Vector3 destination;
    [SerializeField]
    private float stopDistance;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float movementSpeed;

    private Vector3 velocity;
    private Vector3 lastPosition;
    private Animator animator;
    private bool isCorotineRunning = false;

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

    }

    private void Start()
    {

        StartCoroutine(CharacterNavigatorLoop());

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

        if(isCorotineRunning == false)
        {

            StartCoroutine(CharacterNavigatorLoop());

        }

    }

    #endregion

    #region Coroutine

    private IEnumerator CharacterNavigatorLoop()
    {

        isCorotineRunning = true;

        while (isCorotineRunning == true)
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

        isCorotineRunning = false;
        yield return null;

    }

    #endregion

}
