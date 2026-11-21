using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerKick : MonoBehaviour
{
    private InputMap inputMap;
    InputAction kickInput;
    Collider kickColl;
    Transform camTransform;
    [Tooltip("How long the kick should stay active")]
    [SerializeField] float kickDuration;
    [Tooltip("How hard the kick should knock stuff back")]
    [SerializeField] float kickForce;
    [Tooltip("Added upward force")]
    [SerializeField] float kickUpwardForce;
    [Tooltip("How long to wait between kicks. This should, at a min, be higher than duration")]
    [SerializeField] float kickCooldown;
    float nextKickTime;
    [Tooltip("This should eventually be replaced by a full animation. For now this is my dev art kick")]
    [SerializeField] GameObject tempKickGO;

    private void Awake()
    {
        inputMap = new InputMap();
        kickInput = inputMap.Player.Kick;
        kickColl = GetComponent<Collider>();
    }

    private void Start()
    {
        kickColl.enabled = false;
        camTransform = Camera.main.GetComponent<Transform>();
    }

    private void OnEnable()
    {
        kickInput.Enable();
    }

    private void OnDisable()
    {
        kickInput.Disable();
    }

    private void Update()
    {
        if (kickInput.WasPressedThisFrame() && Time.time >= nextKickTime)
        {
            StartCoroutine(Kicking());
        }
    }

    IEnumerator Kicking()
    {
        kickColl.enabled = true;
        tempKickGO.SetActive(true);
        yield return new WaitForSeconds(kickDuration);
        kickColl.enabled = false;
        tempKickGO.SetActive(false);
        nextKickTime = Time.time + kickCooldown;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Rigidbody>(out Rigidbody rb)) return;
        Vector3 dir = (camTransform.forward + kickUpwardForce * Vector3.up).normalized;
        rb.AddForce(dir * kickForce, ForceMode.Impulse);
    }
}
