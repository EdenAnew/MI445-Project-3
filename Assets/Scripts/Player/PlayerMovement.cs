using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References - General")]
    [SerializeField] private PlayerView playerView;
    [SerializeField] private GroundSensor groundSensor;
    [Header("References - Audio")]
    [SerializeField] private AudioClip[] soundFootsteps;
    [SerializeField] private AudioClip soundJump;
    [SerializeField] private AudioClip soundLand;

    [Header("Parameters - Movement")]
    [Tooltip("The base walking velocity of the player")]
    [SerializeField] private float walkSpeed = 5.0f;
    [Tooltip("The amount that walkSpeed is multiplied by when sprinting")]
    [SerializeField] private float sprintMult = 1.75f;
    [Tooltip("The velocity that the player leaves the ground when jumping")]
    [SerializeField] private float jumpImpulse = 5.0f;
    [Tooltip("Impacts the ramp-up to reach full speed, as well as stopping and turning responsiveness; values above .4 are severely sluggish")]
    [SerializeField] private float momentum = 0.2f;
    [Header("Parameters - Audio")]
    [Tooltip("Time in seconds between footsteps being played; this is divided by sprintMult when sprinting")]
    [SerializeField] private float timeBetweenFootsteps = 0.6f;
    [Tooltip("Time in seconds after landing until footsteps can begin playing")]
    [SerializeField] private float delayFootstepsStart = 0.4f;
    [Tooltip("Time in seconds of airtime required to play landing sound once ground is hit")]
    [SerializeField] private float timeMinForLandingSound = 0.1f;
    
    private bool sprinting = false;
    private bool jumping = false;
    private bool wasGrounded = true;
    private int indexFootstep = 0;
    private float timeLastFootstep = 0f;
    private float timeLastGrounded = 0f;
    private float tiltDirection = 0f;
    private float fovAdded = 0f;
    private Vector2 movementInputVector = Vector2.zero;
    private Vector3 calculatedVelocity = Vector3.zero;

    private InputMap inputMap;
    private InputAction movementInput;
    private InputAction sprintInput;
    private InputAction jumpInput;

    private Rigidbody rb;
    private AudioSource aSource;

    private void Awake()
    {
        inputMap = new InputMap();
    }
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        aSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        movementInput = inputMap.Player.Movement;
        sprintInput = inputMap.Player.Sprint;
        jumpInput = inputMap.Player.Jump;
        movementInput.Enable();
        sprintInput.Enable();
        jumpInput.Enable();
    }

    private void OnDisable()
    {
        movementInput.Disable();
        sprintInput.Disable();
        jumpInput.Disable();
    }

    private void OnDestroy()
    {
        inputMap.Player.Disable();
    }

    private void Update()
    {
        UpdateMovementInputs();
        UpdateFootsteps();
    }

    private void FixedUpdate()
    {
        UpdateMovementPhysics();
    }

    private void UpdateMovementInputs()
    {
        // check inputs
        sprinting = sprintInput.IsPressed();
        movementInputVector = movementInput.ReadValue<Vector2>();
        if (jumpInput.WasPressedThisFrame() && groundSensor.isGrounded)
        {
            jumping = true;
            aSource.PlayOneShot(soundJump);
        }
        // calculate movement direction, normalize, then multiply by walk/sprint speed
        calculatedVelocity = Vector3.zero;
        calculatedVelocity += transform.right * movementInputVector.x;
        calculatedVelocity += transform.forward * movementInputVector.y;
        calculatedVelocity.Normalize();
        calculatedVelocity *= sprinting ? walkSpeed * sprintMult : walkSpeed;
        // set look tilt and dynamic fov
        tiltDirection = 0;
        fovAdded = 0;
        if (movementInputVector.x != 0)
        {
            tiltDirection = (movementInputVector.x > 0) ? -1 : 1;
            tiltDirection *= (sprinting) ? 1 : 0.5f;
            tiltDirection *= (movementInputVector.y != 0) ? 0.6f : 1;
        }
        if (movementInputVector.y > 0 && sprinting)
        {
            fovAdded = (movementInputVector.x != 0) ? 0.6f : 1;
        }
        playerView.SetLookTiltGoal(tiltDirection);
        playerView.SetDynamicFOVGoal(fovAdded);
    }

    private void UpdateFootsteps()
    {
        if (!groundSensor.isGrounded || movementInputVector == Vector2.zero) // if player is not moving on ground
        {
            timeLastFootstep = Time.time + (delayFootstepsStart - timeBetweenFootsteps); // have a small buffer before footsteps start
        }
        else if (timeLastFootstep + (timeBetweenFootsteps * (sprinting ? 1 / sprintMult : 1)) <= Time.time)
        {
            timeLastFootstep = Time.time;
            aSource.PlayOneShot(soundFootsteps[indexFootstep]);
            indexFootstep = (indexFootstep + 1 >= soundFootsteps.Length) ? 0 : indexFootstep + 1;
        }
    }

    private void UpdateMovementPhysics()
    {
        // check if we are landing
        if (wasGrounded && !groundSensor.isGrounded) // leaving ground
        {
            wasGrounded = false;
            timeLastGrounded = Time.fixedTime;
        }
        else if (!wasGrounded && groundSensor.isGrounded) // hitting ground
        {
            if (timeLastGrounded + timeMinForLandingSound <= Time.fixedTime) aSource.PlayOneShot(soundLand);
            wasGrounded = true;
        }
        // lerp horizontal velocity between current and new
        calculatedVelocity = Vector3.Lerp(rb.velocity, calculatedVelocity, (1 / momentum) * Time.fixedDeltaTime);
        // add existing y velocity, or jump impulse if jumping
        calculatedVelocity.y = jumping ? jumpImpulse : rb.velocity.y;
        if (jumping) jumping = false; // clear jumping variable
        rb.velocity = calculatedVelocity;
    }
}