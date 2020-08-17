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
    
    private bool isThumbForward = false;
    [Header("Grabbing Properties")]
    private InputDevice RDev;
    private InputDevice LDev;
    private bool isNotGrabbed = true;
    public Transform RHandPos;
    public Transform LHandPos;
    public float _GrabRadius = .5f;
    public Collider[] rColliding;
    public Collider[] lColliding;
    
    public Material handmat;
    [Header("Teleport Properties")]
    public GameObject player;
    public GameObject Lazer;
    public GameObject OvSphere;
    [Range(.1f, 2f)] public float OvSpereRadius = .5f;
    public GameObject Massle;
    public Animator animBlink;
    public Material GEmmitor;
    public Material REmmitor;
    private RaycastHit hit;


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
        LDev.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 leftThumb);
        rColliding = Physics.OverlapSphere(RHandPos.position, _GrabRadius);
        foreach (Collider item in rColliding)
        {
            if (item.CompareTag("Grabbable") && (rightTrigger > .35f || rightGrip > .65f))
            {
                item.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                item.gameObject.transform.position = RHandPos.position;
                item.gameObject.transform.rotation = RHandPos.rotation;
                break;
            }
        }
        lColliding = Physics.OverlapSphere(LHandPos.position, _GrabRadius);
        foreach (Collider item in lColliding)
        {
            if (item.CompareTag("Grabbable") && (leftTrigger > .35f || leftGrip > .65f))
            {
                item.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                item.gameObject.transform.position = LHandPos.position;
                item.gameObject.transform.rotation = LHandPos.rotation;
                break;
            }
        }
        if (leftThumb.y > .5f)
        {
            
            Lazer.GetComponent<MeshRenderer>().enabled = true;
            if (Physics.Raycast(Massle.transform.position,Massle.transform.forward,out hit, 10f))
            {
                OvSphere.GetComponent<MeshRenderer>().enabled = true;
                OvSphere.transform.position = hit.point;
                if (isAbleToTeleport(hit))
                {
                    OvSphere.GetComponent<MeshRenderer>().material = GEmmitor;
                    Lazer.GetComponent<MeshRenderer>().material = GEmmitor;
                }
                else
                {
                    OvSphere.GetComponent<MeshRenderer>().material = REmmitor;
                    Lazer.GetComponent<MeshRenderer>().material = REmmitor;
                }
                isThumbForward = true;
            }
            else
            {
                OvSphere.GetComponent<MeshRenderer>().enabled = false;
                Lazer.GetComponent<MeshRenderer>().material = REmmitor;
            }


        }
        else if (isThumbForward)
        {
            if (isAbleToTeleport(hit))
                player.transform.position = new Vector3(OvSphere.transform.position.x, player.transform.position.y, OvSphere.transform.position.z);
            isThumbForward = false;
            Lazer.GetComponent<MeshRenderer>().enabled = false;
            OvSphere.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    bool isAbleToTeleport(RaycastHit hit)
    {
        //bool Is = false;
        Collider[] colliders = Physics.OverlapSphere(hit.point, OvSpereRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Teleporter")) return true;
        }
        return false;
    }

    async void SetHandMat()
    {
        while (true)
        {
            await Task.Delay(10);
            try {
                GameObject.Find("hand_left_renderPart_0").GetComponent<SkinnedMeshRenderer>().material = handmat;
                GameObject.Find("hand_right_renderPart_0").GetComponent<SkinnedMeshRenderer>().material = handmat;
                if(GameObject.Find("hand_left_renderPart_0").GetComponent<SkinnedMeshRenderer>().material == handmat
                    && GameObject.Find("hand_right_renderPart_0").GetComponent<SkinnedMeshRenderer>().material == handmat)
                {
                    return;
                }
            }
            catch {}
        }
    }

}
