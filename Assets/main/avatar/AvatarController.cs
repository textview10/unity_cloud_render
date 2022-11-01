using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AvatarController : MonoBehaviour
{
    private Transform _joydir;
    private ETCJoystick _etcJoystick;
    private AvatarCtrl _avatarCtrl;
    private ETCTouchPad _etcTouchPad1;

    private void Awake()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject joyPanelObj = GameObject.Find("EasyTouchControlsCanvas/DefaultJoyPanel");
        GameObject joyStickObj = joyPanelObj.transform.Find("Joystick").gameObject;

        _etcJoystick = joyStickObj.GetComponent<ETCJoystick>();

        _etcJoystick.tmSpeed = 4;
        _etcJoystick.onMove.AddListener(_onMoveInternal);
        _etcJoystick.onMoveEnd.AddListener(_onMoveEndInternal);

        _joydir = _etcJoystick.transform.Find("JoyDir");


        _etcTouchPad1 = joyPanelObj.transform.Find("TouchPadController").GetComponent<ETCTouchPad>();
        _etcTouchPad1.onMove.AddListener(_onTouchPadMoveInternal);
        
        ETCButton jumpETCButton = joyPanelObj.transform.Find("Jump").GetComponent<ETCButton>();
        jumpETCButton.onDown.AddListener(_onJump);

        GameObject avatarGo = GameObject.Find("avatar");
        _avatarCtrl = new AvatarCtrl(avatarGo);

        _etcJoystick.cameraLookAt = _avatarCtrl.GetLookRotation();
    }

    // Update is called once per frame
    void Update()
    {
        _avatarCtrl?.OnUpdate();
    }

    private void _onJump()
    {
        _avatarCtrl?.OnJump();
    }
    private void _onMoveInternal(Vector2 v)
    {
        _avatarCtrl?.OnMove(v);
        if (_joydir == null) return;
        _joydir.gameObject.SetActive(true);
        // _joydir.rotation = Quaternion.AngleAxis(30, Vector3.forward);
        if (v.x > 0)
        {
            if (v.y > 0)
            {
                _joydir.rotation = Quaternion.AngleAxis(-45, Vector3.forward);
            }
            else if (v.y < 0)
            {
                _joydir.rotation = Quaternion.AngleAxis(-135, Vector3.forward);
            }
            else
            {
                _joydir.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
            }
        }
        else if (v.x < 0)
        {
            if (v.y > 0)
            {
                _joydir.rotation = Quaternion.AngleAxis(45, Vector3.forward);
            }
            else if (v.y < 0)
            {
                _joydir.rotation = Quaternion.AngleAxis(135, Vector3.forward);
            }
            else
            {
                _joydir.rotation = Quaternion.AngleAxis(90, Vector3.forward);
            }
        }
        else if (v.x == 0)
        {
            if (v.y > 0)
                _joydir.rotation = Quaternion.AngleAxis(0, Vector3.forward);
            else if (v.y < 0)
                _joydir.rotation = Quaternion.AngleAxis(180, Vector3.forward);
        }
    }

    private void _onTouchPadMoveInternal(Vector2 v)
    {
        // Debug.Log("Test" + " test = " + v.y);
        _avatarCtrl?.OnTouchPadMove(v);
        _setCameraHeight(-v.y * 0.005f);
    }

    private float threeBezier(float t, float cx1, float cy1, float cx2, float cy2)
    {
        // --x1,y1 固定点位置
        float x1 = 0;
        float y1 = 0;
        float x2 = 1;
        float y2 = 1;
        float x = x1 * (1 - t) * (1 - t) * (1 - t) +
                  3 * cx1 * t * (1 - t) * (1 - t) +
                  3 * cx2 * t * t * (1 - t) +
                  x2 * t * t * t;
        float y = y1 * (1 - t) * (1 - t) * (1 - t) +
                  3 * cy1 * t * (1 - t) * (1 - t) +
                  3 * cy2 * t * t * (1 - t) +
                  y2 * t * t * t;
        return y;
    }

    private float GetFollowDistance(float height, float size, float min)
    {
        // --高度百分比
        float proportion = (height - min) / size;
        // -- 曲线百分比data: Y高度 X宽度
        // --local  y = threeBezier(x,0,1.45,.08,.93)
        float y = 0;
        if (proportion > 0.5)
        {
            y = threeBezier(1 - proportion, 0, 1.45f, 0.08f, 0.93f);
        }
        else
        {
            y = threeBezier(proportion, 0, 1.45f, 0.08f, 0.93f);
        }

        return y;
    }

    private void _setCameraHeight(float offset)
    {
        // if not self.joyPanel or not self.joyPanel.joystick then
        // return
        //     end
        _etcJoystick.followHeight = Mathf.Clamp(_etcJoystick.followHeight + offset, -0.7f, 2f);

        float curveData = GetFollowDistance(_etcJoystick.followHeight, 2.7f, -0.7f);
        // -- 距离
        // --最小0。5
        float size = 1f + 1.7f * curveData;
        // --joystick.followDistance = Mathf.Clamp(joystick.followDistance+offset*0.05, 1,2.5)
        _etcJoystick.followDistance = Mathf.Clamp(size, 1, 3);
        // --joystick.followDistance =0.5
        // --print("TestCarme----lb2------"..curveData.."size--->"..size.." joystick.followHeight-->".. joystick.followHeight)
        // Debug.Log("Test 00000 = " + offset);
        // Debug.Log("Test 1111 = " + _etcJoystick.followHeight);
        // Debug.Log("Test 22222 = " + _etcJoystick.followDistance);
    }

    private void _onMoveEndInternal()
    {
        _avatarCtrl?.OnMoveEnd();
        if (_joydir != null)
        {
            _joydir.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (_etcJoystick == null) return;
        _etcJoystick.onMove.RemoveAllListeners();
        _etcJoystick.onMoveEnd.RemoveAllListeners();
        _etcTouchPad1.onMove.RemoveAllListeners();
    }
}