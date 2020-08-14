using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;

public class RayCaster : MonoBehaviour
{
    public GameObject massle;
    public GameObject GreenLaser;
    public GameObject Player;
    public List<InputDevice> devices;
    public float PlRot = 0;
    private InputDevice targetDev;
    public Animator anim;
    [SerializeField]bool IsReadyForRot = true;
    void Start()
    {
        devices = new List<InputDevice>();
        InputDeviceCharacteristics RConChar = InputDeviceCharacteristics.Right;
        InputDevices.GetDevicesWithCharacteristics(RConChar, devices);
        if(devices.Count > 0) { targetDev = devices[0]; }
    }

    void Update()
    {
        targetDev.TryGetFeatureValue(CommonUsages.trigger, out float primaryButtonValue);
        if (primaryButtonValue > 0.2f) 
        { 
            Teleport(); 
        }


        targetDev.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 axisButtonValue);
        if (axisButtonValue.x > .5f) {
            if (IsReadyForRot) PlRot += 90f; 
            IsReadyForRot = false; 
        }
        else if (axisButtonValue.x < -.5f) {
            if (IsReadyForRot) PlRot -= 90f;
            IsReadyForRot = false; 
        }
        else { IsReadyForRot = true; }

        Player.transform.rotation = Quaternion.Euler(Player.transform.rotation.x, PlRot, Player.transform.rotation.z);

        if(Physics.Raycast(massle.transform.position, massle.transform.forward, out RaycastHit hit, 500f)&& hit.collider.CompareTag("Teleporter"))
        {
            GreenLaser.GetComponent<Renderer>().enabled = true;
        }
        else { GreenLaser.GetComponent<Renderer>().enabled = false; }
    }
    async void Teleport()
    {
        if (Physics.Raycast(massle.transform.position, massle.transform.forward, out RaycastHit hit, 500f))
        {
            if (hit.collider.CompareTag("Teleporter"))
            {
                anim.SetBool("Play", true);
                await Task.Delay(480);
                Player.transform.position = hit.collider.gameObject.transform.position;
                anim.SetBool("Play", false);
            }
        }

    }
    //async void Rotate(float rotation)
    //{
        
    //}
}
