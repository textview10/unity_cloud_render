using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLookAt : MonoBehaviour
{
    private Camera mainCam;
    private void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (mainCam)
        {
            this.transform.LookAt(mainCam.transform);
        }
    }
}
