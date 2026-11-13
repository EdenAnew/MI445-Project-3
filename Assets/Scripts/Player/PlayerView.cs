using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject head;
    [SerializeField] private Camera playerCamera;

    [Header("Parameters")]
    [SerializeField] private bool invertY = false;
    [SerializeField] private bool useLookTilt = true;
    [SerializeField] private bool useDynamicFOV = true;
    [Tooltip("Multiplier for mouse input; ideal range is between 1 and 10")]
    [SerializeField] private float lookSensitivity = 5.0f;
    [Tooltip("Degrees above the horizon that the player can look")]
    [SerializeField] private float topViewBoundary = 85.0f;
    [Tooltip("Degrees below the horizon that the player can look")]
    [SerializeField] private float bottomViewBoundary = 75.0f;
    [Tooltip("Degrees that the camera will roll during horizontal movement")]
    [SerializeField] private float lookTiltMagnitude = 15.0f;
    [Tooltip("The speed of the look tilt change")]
    [SerializeField] private float lookTiltSpeed = 2.0f;
    [Tooltip("The amount of FOV added when dynamic FOV is triggered")]
    [SerializeField] private float dynamicFOVMagnitude = 10.0f;
    [Tooltip("The speed of the FOV change")]
    [SerializeField] private float dashFOVSpeed = 2.0f;

    private Vector2 lookInputVector = Vector2.zero;
    private Vector3 headAngle = Vector3.zero;
    private float pitch = 0f;
    private float yaw = 0f;
    private float lookTilt = 0f;
    private float lookTiltGoal = 0f;
    private float baseFOV;
    private float dynamicFOVGoal;

    private InputMap inputMap;
    private InputAction cameraInput;

    private void Awake()
    {
        inputMap = new InputMap();
    }

    private void Start()
    {
        baseFOV = playerCamera.fieldOfView;
        dynamicFOVGoal = baseFOV;
    }

    private void OnEnable()
    {
        cameraInput = inputMap.Player.Camera;
        cameraInput.Enable();
    }

    private void OnDisable()
    {
        cameraInput.Disable();
    }

    private void OnDestroy()
    {
        inputMap.Player.Disable();
    }

    private void Update()
    {
        lookInputVector = cameraInput.ReadValue<Vector2>();
        UpdateLookTilt();
        UpdateDynamicFOV();
        UpdateLookAngle();
    }

    /*
    update functions dedicated to specific features
    regarding the camera/head
    */
    private void UpdateLookTilt()
    {
        if (!useLookTilt)
        {
            lookTilt = 0f;
            return;
        }
        lookTilt = Mathf.Lerp(lookTilt, lookTiltGoal, lookTiltSpeed * Time.deltaTime);
    }

    private void UpdateDynamicFOV()
    {
        if (!useDynamicFOV)
        {
            playerCamera.fieldOfView = baseFOV;
            return;
        }
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, dynamicFOVGoal, dashFOVSpeed * Time.deltaTime);
    }

    private void UpdateLookAngle()
    {
        pitch = lookInputVector.y * lookSensitivity * Time.deltaTime;
        yaw = lookInputVector.x * lookSensitivity * Time.deltaTime;

        pitch = invertY ? pitch : -pitch; // invert pitch if applicable

        // rotate avatar based on yaw
        this.transform.localEulerAngles += new Vector3(0f, yaw, 0f);
        
        // calculate head rotation based on pitch
        headAngle = head.transform.localEulerAngles + new Vector3(pitch, 0f, 0f);
        
        // check angle boundaries
        if (AngleWithin180(headAngle.x) < -topViewBoundary)
        {
            headAngle.x = 360 - topViewBoundary;
        }
        else if (AngleWithin180(headAngle.x) > bottomViewBoundary)
        {
            headAngle.x = bottomViewBoundary;
        }

        // add look tilt
        headAngle.z = lookTilt;

        // apply head rotation
        head.transform.localEulerAngles = headAngle;
    }

    /*
    public functions to set the goal values of
    look tilt and dynamic FOV, and toggles to 
    enable/disable these features
    */
    public void SetLookTiltGoal(float tilt)
    {
        lookTiltGoal = tilt * lookTiltMagnitude;
    }

    public void SetDynamicFOVGoal(float fov)
    {
        dynamicFOVGoal = (fov * dynamicFOVMagnitude) + baseFOV;
    }

    public void ToggleCameraTilt()
    {
        useLookTilt = useLookTilt ? false : true;
    }

    public void ToggleDynamicFOV()
    {
        useDynamicFOV = useDynamicFOV ? false : true;
    }

    /* 
    returns degrees above/below the "horizontal" axis
    value is negative if above horizon (180 or more)
    value is positive if below horizon (less than 180)
    keep in mind that once we go above horizon line, actual angle starts at 360 and decreases
    */
    private static float AngleWithin180 (float angle)
    {
        return (angle <= 180) ? angle : -(360 - angle);
    }
}