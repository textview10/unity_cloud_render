using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLookAtForward : MonoBehaviour
{
    
    private Camera mainCam;
    public GameObject canvas;
    public Transform headTransform;
    public Vector3 offset;  

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (canvas == null)
        {
            return;
        }

        if (headTransform == null)
        {
            canvas.transform.position  = this.transform.position + offset; //指定当前Canvas位置
            canvas.transform.LookAt(mainCam.transform);
        }

        Boolean isHeadObject = false;
        for (int i = 0; i < headTransform.childCount; i++)
        {
            var child = headTransform.GetChild(i).gameObject;
            if (child && child.name.Contains("modules/dynasty")  && child.activeSelf)
            {
                isHeadObject = true;
                break;
            }
        }

        if (isHeadObject)
        {
            canvas.transform.position  = this.transform.position + offset; //指定当前Canvas位置
            Vector3 relativePos =   mainCam.transform.position - this.transform.position; // 向量相减等于camera到人的向量
            relativePos = relativePos.normalized;//取单位向量
            relativePos.y = 0;
            canvas.transform.position = canvas.transform.position + relativePos * 0.5f; //让canvas在相机方向移动一个单位向量
            canvas.transform.LookAt(mainCam.transform); //canvas朝向相机方向
        }
        else
        {
            canvas.transform.position  = this.transform.position + offset; //指定当前Canvas位置
            canvas.transform.LookAt(mainCam.transform);
            

        }

       
    }
}


