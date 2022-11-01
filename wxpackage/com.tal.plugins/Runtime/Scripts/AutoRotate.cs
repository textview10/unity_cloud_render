using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public float angle = 30;
    private void Update()
    {
        transform.Rotate(0, angle * Time.deltaTime, 0);
    }
}
