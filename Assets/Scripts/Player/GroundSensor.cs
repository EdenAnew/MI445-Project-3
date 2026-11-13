using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    private int objCount;

    public bool isGrounded {get {return objCount > 0;}}

    void OnTriggerEnter(Collider other) {if (!other.isTrigger) objCount++;}
    void OnTriggerExit(Collider other) {if (!other.isTrigger) objCount--;}
}
