using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Billboard : MonoBehaviour
{
    Camera mainCam => Camera.main;

    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCam.transform.rotation * Vector3.forward, mainCam.transform.rotation * Vector3.up);
    }
}
