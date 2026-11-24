using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The number of seconds that the enemy knows where the player is post-losing line of sight")]
    private float continueTrackingTime = 0.5f;

    [SerializeField]
    private float wanderRange = 10f;

    bool knowsWhereThePlayerIsForever = false;

    [SerializeField]
    private float knockoutTime;
    private float updatesInSamePos = 0f, updatesAllowedToBeTheSamePos = 25f;
    private bool continueTracking, trackingStarted = false;
    private bool movementAllowed = true;

    private Vector3 positionLastFrame;
    protected Vector3 lastKnownPosition;
    private Vector3 wanderPosition;
    private Rigidbody rb;
    private NavMeshAgent navAgent;
    protected GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    void FixedUpdate()
    {
        if (movementAllowed)
            HandleMovement();
    }

    public virtual void Setup()
    {
        rb = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();

        // Might be best to change later
        if (GameObject.Find("Player"))
            target = GameObject.Find("Player");
        else
            Debug.LogError("THE PLAYER COULD NOT BE FOUND!");

        wanderPosition = Vector3.zero;
        lastKnownPosition = Vector3.zero;
    }

    /// <summary>
    /// Handles the movement of enemy AI.
    /// If it sees the player, or it knows where the player is,
    /// it follows the player.
    /// 
    /// Otherwise, it travels to the last known position of the player.
    /// 
    /// Once it reaches the last known position of the player, it wanders.
    /// 
    /// </summary>
    void HandleMovement()
    {
        PreventBeingStuck();
        
        if (knowsWhereThePlayerIsForever || continueTracking || LineOfSight())
            FollowTarget();
        else if (lastKnownPosition != Vector3.zero && !LastKnownLocationReached() && updatesInSamePos < updatesAllowedToBeTheSamePos)
            NavigateToNonTarget(lastKnownPosition);
        else
        {
            lastKnownPosition = Vector3.zero;
            // Using Vector3.zero as a null value should be unnoticeable during actual gameplay
            if (wanderPosition == Vector3.zero || EqualIgnoringHeight(wanderPosition, transform.position))
                wanderPosition = GetWanderPosition();
            NavigateToNonTarget(wanderPosition);
        }
    }

    /// <summary>
    /// Line of sight functionality on the player (or any held object of the player)
    /// Uses an infinite raycast.
    /// </summary>
    /// <returns> Whether or not it can see the player</returns>
    bool LineOfSight()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hit, Mathf.Infinity))
        {
            Throwable heldObject = hit.transform.GetComponent<Throwable>();
            // If we hit the player or their held object
            if (hit.transform.position == target.transform.position || (heldObject != null && heldObject.rb.isKinematic == true))
            {
                wanderPosition = Vector3.zero;
                continueTracking = true;
                return true;
            }
        }

        if (!trackingStarted && continueTracking)
            StartCoroutine(nameof(ContinueTrackingPlayer));

        return false;
    }

    /// <summary>
    /// Follows the player
    /// </summary>
    void FollowTarget()
    {
        lastKnownPosition = Vector3.zero;
        navAgent.SetDestination(target.transform.position);
    }

    /// <summary>
    /// Moves towards a set destination
    /// </summary>
    /// <param name="position">The destination the enemy is moving towards</param>
    void NavigateToNonTarget(Vector3 position)
    {
        navAgent.SetDestination(position);
    }

    /// <summary>
    /// Gives a position for the AI to wander to within a radius
    /// This section of code is a modified version of a function created by Selzier
    /// https://discussions.unity.com/t/how-to-get-a-random-point-on-navmesh/73440/2
    /// </summary>
    /// <returns>A valid position for the AI to wander towards</returns>
    Vector3 GetWanderPosition()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomDirection;
        NavMeshHit hit;
        do
        {
            randomDirection = (Random.insideUnitSphere * wanderRange) + transform.position;
        }
        while (!NavMesh.SamplePosition(randomDirection, out hit, wanderRange, 1) && randomDirection != Vector3.zero);

        finalPosition = hit.position;
        return finalPosition;
    }

    /// <summary>
    /// Checking if the lastKnownPosition has been reached by the AI
    /// </summary>
    /// <returns>Whether lastKnownPosition was reached</returns>
    bool LastKnownLocationReached()
    {
        if (EqualIgnoringHeight(transform.position, lastKnownPosition))
        {
            lastKnownPosition = Vector3.zero;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if two positions are equal, minus the y position
    /// </summary>
    /// <param name="v1">Position 1</param>
    /// <param name="v2">Position 2</param>
    /// <returns>Whether both x positions and z positions are the same</returns>
    bool EqualIgnoringHeight(Vector3 v1, Vector3 v2)
    {
        if (Mathf.Approximately(v1.x, v2.x) && Mathf.Approximately(v1.z, v2.z))
            return true;
        return false;
    }

    /// <summary>
    /// Logic for preventing the AI from getting stuck. Sloppy.
    /// </summary>
    void PreventBeingStuck()
    {
        if (positionLastFrame == transform.position)
            updatesInSamePos++;
        else
            updatesInSamePos = 0;

        positionLastFrame = transform.position;
    }

    /// <summary>
    /// Explode
    /// </summary>
    void Explode()
    {
        // Do particles
        Destroy(this.gameObject, 0.5f);
    }

    /// <summary>
    /// An enumerator handling the logic of toggling player tracking after not seeing them
    /// </summary>
    /// <returns></returns>
    IEnumerator ContinueTrackingPlayer()
    {
        trackingStarted = true;
        yield return new WaitForSeconds(continueTrackingTime);
        continueTracking = false;
        trackingStarted = false;
        lastKnownPosition = target.transform.position;
    }


    private void OnTriggerEnter(Collider other)
    {
        // Add player killer
    }

    /// <summary>
    /// Gives the enemy permanent player awareness
    /// </summary>
    public void GainPlayerAwareness()
    {
        knowsWhereThePlayerIsForever = true;
    }

    private void OnEnable()
    {
        EnemyManager.OnEnemyAwarenessTimer += GainPlayerAwareness;
    }

    private void OnDisable()
    {
        EnemyManager.OnEnemyAwarenessTimer -= GainPlayerAwareness;
    }
}