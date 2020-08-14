
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Triggerpos : MonoBehaviour
{
    public List<InputDevice> devices;
    private InputDevice targetDev;
    public GameObject trigger;

    void Start()
    {
        devices = new List<InputDevice>();
        InputDeviceCharacteristics RConChar = InputDeviceCharacteristics.Right;
        InputDevices.GetDevicesWithCharacteristics(RConChar, devices);
        if (devices.Count > 0) { targetDev = devices[0]; }
    }

    void Update()
    {
        targetDev.TryGetFeatureValue(CommonUsages.trigger, out float primaryButtonValue);
        trigger.transform.localPosition = new Vector3(-0.21f - (primaryButtonValue / 20), 0.048f, 0f);
    }
}
