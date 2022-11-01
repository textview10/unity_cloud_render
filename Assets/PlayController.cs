using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("wangxuTest start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Look(InputAction.CallbackContext value)
    {
        // Vector2 input = value.ReadValue<Vector2>();
        Debug.Log("wangxuTest look ...");
        // transform.eulerAngles += new Vector3(-input.y, input.x, 0);
    }
}
