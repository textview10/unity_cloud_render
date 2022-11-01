using System;
using System.Collections;
using System.Collections.Generic;
using Com.Tal.Unity.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyColliderLogic : MonoBehaviour
{
    public Image controlImage;
    
    private void Start()
    {
        var logics = FindObjectsOfType<JoyColliderLogic>();
        
        var joyController = GameObject.FindObjectOfType<JoyController>();
        joyController.onEndDragEvent += () =>
        {
            controlImage.gameObject.SetActive(false);
        };
        var trigger = this.gameObject.GetComponent<EventTriggerHandler>()??this.gameObject.AddComponent<EventTriggerHandler>();
        trigger.AddEvent(EventTriggerType.PointerEnter, (data) =>
        {
            if (joyController.GetLocalMagnitude())
            {
                foreach (var logic in logics)
                {
                    logic.Close();
                }
                controlImage.gameObject.SetActive(true);
            }
        });
    }

    public void Close()
    {
        controlImage.gameObject.SetActive(false);
    }
}
