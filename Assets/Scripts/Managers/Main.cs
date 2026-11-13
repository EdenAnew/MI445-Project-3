using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Main : MonoBehaviour
{
    static private Main S;
    [Header("References")]
    [SerializeField] private GameObject pauseUI;

    [Header("Dynamic")]
    [SerializeField]
    static private bool _Paused = false;
    static public bool Paused
    {
        get {return _Paused;}
        set
        {
            _Paused = value;
            Time.timeScale = (_Paused) ? 0 : 1;
            S.pauseUI.SetActive(_Paused);
        }
    }

    [SerializeField]
    static private bool _CursorCaptured = false;
    static public bool CursorCaptured
    {
        get {return _CursorCaptured;}
        set
        {
            _CursorCaptured = value;
            Cursor.lockState = (_CursorCaptured) ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !_CursorCaptured;

        }
    }

    private InputMap inputMap;
    private InputAction pauseInput;
    private InputAction returnInput;

    void Awake() 
    {
        S = this;
        inputMap = new InputMap();
    }

    void Start()
    {
        // capture, hide cursor when game starts
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        returnInput = inputMap.Menu.Return;
        pauseInput = inputMap.Player.Pause;
        returnInput.Enable();
        pauseInput.Enable();
    }

    private void OnDisable()
    {
        returnInput.Disable();
        pauseInput.Disable();
    }

    private void OnDestroy()
    {
        inputMap.Menu.Disable();
        inputMap.Player.Disable();
    }

    void Update()
    {
        if (Paused && returnInput.WasPressedThisFrame()) RESUME_GAME();
        else if (!Paused && pauseInput.WasPressedThisFrame()) PAUSE_GAME();
    }

    // if the application is not focused, pause the game
    void OnApplicationFocus(bool focused)
    {
        if (!focused && !Paused) PAUSE_GAME();
    }

    // on pause, show and free the cursor
    static public void PAUSE_GAME() 
    {
        CursorCaptured = false;
        Paused = true;
    }

    // on resume, hide and capture the cursor
    static public void RESUME_GAME() 
    {
        CursorCaptured = true;
        Paused = false;
    }
}