using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;
using UnityEngineInternal;

public class UserController : MonoBehaviour
{
    public List<InputDevice> devicesR;
    public List<InputDevice> devicesL;
    private InputDevice RDev;
    private InputDevice LDev;
    private bool isNotGrabbed = true;
    public Transform RHandPos;
    public Transform LHandPos;
    public float _GrabRadius = .5f;
    public Collider[] _rColliding;
    public Collider[] _lColliding;
    public Material _handmat;


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
        SetHandMat();
    }

    void Update()
    {
        RDev.TryGetFeatureValue(CommonUsages.trigger, out float rightTrigger);
        RDev.TryGetFeatureValue(CommonUsages.grip, out float rightGrip);
        LDev.TryGetFeatureValue(CommonUsages.trigger, out float leftTrigger);
        LDev.TryGetFeatureValue(CommonUsages.grip, out float leftGrip);
        _rColliding = Physics.OverlapSphere(RHandPos.position, _GrabRadius);
        foreach (Collider item in _rColliding)
        {
            if (item.CompareTag("Grabbable") && (rightTrigger > .35f || rightGrip > .65f))
            {
                item.gameObject.transform.position = RHandPos.position;
                item.gameObject.transform.rotation = RHandPos.rotation;
            }
        }
        _lColliding = Physics.OverlapSphere(LHandPos.position, _GrabRadius);
        foreach (Collider item in _lColliding)
        {
            if (item.CompareTag("Grabbable") && (leftTrigger > .35f || leftGrip > .65f))
            {
                item.gameObject.transform.position = LHandPos.position;
                item.gameObject.transform.rotation = LHandPos.rotation;
            }
        }
    }
    async void SetHandMat()
    {
        for (int i = 0;i<50;i++)
        {
            await Task.Delay(100);
            try {
                GameObject.Find("hand_left_renderPart_0").GetComponent<SkinnedMeshRenderer>().material = _handmat;
                GameObject.Find("hand_right_renderPart_0").GetComponent<SkinnedMeshRenderer>().material = _handmat;
            }
            catch { }
        }
    }

}
