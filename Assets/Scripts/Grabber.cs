﻿using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngineInternal;

public class Grabber : MonoBehaviour
{
    public List<InputDevice> devicesR;
    public List<InputDevice> devicesL;
    private InputDevice RDev;
    private InputDevice LDev;
    private bool isNotGrabbed = true;
    public Transform RHandPos;
    public Transform LHandPos;
    public float _GrabRadius;
    public Collider[] _rColliding;
    public Collider[] _lColliding;

    // Start is called before the first frame update
    void Start()
    {
        devicesR = new List<InputDevice>();
        devicesL = new List<InputDevice>();
        InputDeviceCharacteristics RConChar = InputDeviceCharacteristics.Right;
        InputDeviceCharacteristics LConChar = InputDeviceCharacteristics.Left;
        InputDevices.GetDevicesWithCharacteristics(RConChar, devicesR);
        InputDevices.GetDevicesWithCharacteristics(LConChar, devicesL);
        RDev = devicesR[0];
        LDev = devicesL[0]; 
    }

    // Update is called once per frame
    void Update()
    {
        RDev.TryGetFeatureValue(CommonUsages.trigger, out float rightTrigger);
        RDev.TryGetFeatureValue(CommonUsages.grip, out float rightGrip);
        LDev.TryGetFeatureValue(CommonUsages.trigger, out float leftTrigger);
        LDev.TryGetFeatureValue(CommonUsages.grip, out float leftGrip);
        _rColliding = Physics.OverlapSphere(RHandPos.position, _GrabRadius);
        foreach (Collider item in _rColliding)
        {
            if (item.CompareTag("Grabbable"))
            {
                item.gameObject.transform.position = RHandPos.position;
            }
        }
        _lColliding = Physics.OverlapSphere(LHandPos.position, _GrabRadius);
        foreach (Collider item in _lColliding)
        {
            if (item.CompareTag("Grabbable")&&(leftTrigger > .2f||leftGrip > .2f))
            {
                item.gameObject.transform.position = LHandPos.position;
            }
        }
    }

}
