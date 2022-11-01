using UnityEngine;
using System.Collections;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class CamMove : MonoBehaviour
// {
//     // private MoveController mc;      //获取人物控制组件
//     public Transform target;         //相机跟随的目标位置
//
//     public float rotationDamping = 6;         //相机跟随人物的旋转速度
//     public float zoomSpeed = 4;                //鼠标滚轮滑动速度
//
//     private float h1;                      //点击鼠标右键，存储鼠标Y方向位移
//     private float h2;                      //点击鼠标左键，存储鼠标Y方向位移
//     private float offset;                  //镜头左右旋转的差值
//     private float distance;     //相机和目标的距离
//     private bool cameraIsRotate;
//     private float oldRotationAngle;
//
//     //private float height = 1f;       //相机和目标的高度
//     //private float heightDamping = 1;
//     // Vector3 offsetPosition;
//
//     // Use this for initialization
//     void Start()
//     { 
//         offset = 0;
//         cameraIsRotate = true;
//         // target = GameObject.Find("Player_archer_arrow_master").transform;
//         oldRotationAngle = transform.eulerAngles.y;
//         // mc = target.gameObject.GetComponent<MoveController>();
//         distance = Vector3.Distance(new Vector3(0, 0, target.position.z), new Vector3(0, 0, transform.position.z));
//
//         //offsetPosition = target.position - transform.position;
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         //transform.position = target.position - offsetPosition;
//
//         flowTarget();
//         zoomView();
//         UpAndDownView();
//         LeftAndRightView();
//
//     }
//
//     /// <summary>
//     /// 相机跟随人物移动旋转
//     /// </summary>
//     void flowTarget()
//     {
//         float wantedRotationAngle = oldRotationAngle;    //要达到的旋转角度
//         //float wantedHeight = target.position.y + height;     //要达到的高度
//         float currentRotationAngle = transform.eulerAngles.y; //当前的旋转角度
//         //float currentHeight = transform.position.y;           //当前的高度
//         if (cameraIsRotate)
//         {
//             currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
//         }
//         // currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);    //由当前高度达到要达到的高度
//         Quaternion currentRotation = Quaternion.Euler(transform.eulerAngles.x, currentRotationAngle, 0);
//         // float currentRotation=1;   //防止主角回头摄像机发生旋转，  这里不用
//
//         Vector3 ca = target.position - currentRotation * Vector3.forward * distance;     //tt是相机的位置      
//        // transform.position = target.position-currentRotation * Vector3.forward * distance;
//
//         transform.position = new Vector3(ca.x, transform.position.y, ca.z);      //最后得到的相机位置
//         transform.rotation = currentRotation;                                   //最后得到相机的旋转角度
//         // transform.LookAt(target.position);
//
//     }
//
//     /// <summary>
//     /// 滚轮控制缩放
//     /// </summary>
//     void zoomView()
//     {
//         float scrollWheel = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
//         distance -= scrollWheel;
//         if (distance > 5.6f)
//         {
//             distance = 5.6f;
//         }
//         if (distance < 0.9f)
//         {
//             distance = 0.9f;
//         }
//     }
//     /// <summary>
//     /// 摄像头上下视角
//     /// </summary>
//     void UpAndDownView()
//     {
//         if (Input.GetMouseButton(0))
//         {
//             Debug.Log(oldRotationAngle);
//             h1 = Input.GetAxis("Mouse Y") * rotationDamping;
//             Vector3 originalPosition = transform.position;
//             Quaternion originalRotation = transform.rotation;
//             transform.RotateAround(target.position, -target.right, h1);     //决定因素position和rotation
//             float x = transform.eulerAngles.x;
//             if (x < -10 || x > 80)
//             {
//                 transform.position = originalPosition;
//                 transform.rotation = originalRotation;
//             }
//         }
//     }
//     /// <summary>
//     /// 摄像头左右视角
//     /// </summary>
//     void LeftAndRightView()
//     {
//         if (Input.GetMouseButton(0))
//         {
//             cameraIsRotate = false;
//             h2 = Input.GetAxis("Mouse X") * rotationDamping;
//             Vector3 originalPosition = transform.position;
//             Quaternion originalRotation = transform.rotation;
//             transform.RotateAround(target.position, target.up, h2);     //决定因素position和rotation
//             float x = transform.eulerAngles.x;
//         }
//         if (Input.GetMouseButtonUp(0))
//         {
//             //offset = (transform.eulerAngles.y + 360 - oldRotationAngle)%360;
//             //Debug.Log(offset);
//             oldRotationAngle = transform.eulerAngles.y;
//             cameraIsRotate = true;
//         }
//     }
// }

// public class CamMove : MonoBehaviour
// {
//     public GameObject Player;
//     public float Distance;
//     //public float CameraRepositionSpeed;
//     public float MouseMotionScaleLevel;
//     public bool ReverseAxisY;
//     public float PitchMaximum;
//     public float PitchMinimum;
//     private float _CurrentCameraAngleAroundX;
//     private float _CurrentCameraAngleAroundY;
//     private Vector3 _PositionTarget;
//  
//     // Use this for initialization
//     void Start()
//     {
//         Player = GameObject.Find("Boy");
//     }
//  
//     // Update is called once per frame
//     void Update()
//     {
//         _CurrentCameraAngleAroundX += Input.GetAxis("Mouse Y") * MouseMotionScaleLevel * Time.deltaTime * (ReverseAxisY ? -1 : 1);
//         _CurrentCameraAngleAroundY += Input.GetAxis("Mouse X") * MouseMotionScaleLevel * Time.deltaTime;
//         _CurrentCameraAngleAroundX = Mathf.Clamp(_CurrentCameraAngleAroundX, PitchMinimum, PitchMaximum);
//         _PositionTarget = Player.transform.position + Quaternion.Euler(_CurrentCameraAngleAroundX, _CurrentCameraAngleAroundY, 0) * (-Player.transform.forward * Distance);
//  
//         //transform.position = Vector3.Lerp(transform.position, _PositionTarget, Time.deltaTime * CameraRepositionSpeed);
//         transform.position = _PositionTarget;
//         transform.LookAt(Player.transform);
//     }
// }
using UnityEngine;
using System.Collections;

public class CamMove : MonoBehaviour
{

    int dx = 1;

    bool zoom = false;
    bool iconmove = false;

    int z_speed = 0;
    float z_time = 0;

    public float zoomdelay = 0;
    public float fov = 30;
    public float originfov = 30;
    public Transform cha1;
    public Transform target;
    public Vector3 distancetarget;
    public Vector3 chaposition;
    private float boundfactor = 1;
    private Vector3 hit_shake1 = new Vector3(0.06f, 0, 0.03f);
    private Vector3 hit_shake2 = new Vector3(0, 0.03f, 0.02f);
    private Vector3 hit_shake3 = new Vector3(0, 0.04f, 0.02f);
    public float resetcam_delay = 0;
    private bool resetstart = false;
    private bool fovchange = false;
    private Camera mycamera;


    public Vector3 limitpos;
    //Todo Lee
    //Icon_Skill script_skillicon;
    public float fovbk_delay = 0;

    internal float limit_x_l = 2.1f;
    internal float limit_x_r = 2.1f;
    internal float limit_y_b = -3.3f;
    internal float limit_y_f = 0.85f;
    bool hidestop = false;
    short topviewon = 0;
    float topviewdelay = 3;
    //float camrotX = 50;
    short movespeed = 10;

    public Transform m_blackFog;

    // the params count fix add by wp
    private struct CamParam
    {
        public Vector3 pos;
        public Quaternion rot;
        public float field;
        public short speed;
        public CamParam(Vector3 p, Quaternion r, float f, short speed)
        {
            pos = p;
            rot = r;
            field = f;
            this.speed = speed;
        }
    }

    private CamParam camParam;

    private static CamMove mInstance;
    //added by wp
    public static CamMove instance
    {
        get
        {
            if (mInstance == null)
            {
                // mInstance = GameObject.FindObjectOfType(typeof(UICamera)) as CamMove;
            }
            return mInstance;
        }
    }

    void Awake()
    {
        if (mInstance == null) mInstance = this;

        mycamera = GetComponent<Camera>();


        // record params of camera
        camParam = new CamParam(transform.localPosition, transform.localRotation, mycamera.fieldOfView, movespeed);



        cha1 = GameObject.FindWithTag("Boy").transform; //ObjectManager.myPlayer.transform;
        transform.localEulerAngles = new Vector3(50, 0, 0);

        ResetCam();

        BlackFog(false);
    }
    void Start()
    {
//		Debug.LogError("position" + transform.position.x + " " + transform.position.y + " " + transform.position.z);
		transform.position = new Vector3 (0, 1.6f,-1.04f);
 
        distancetarget = new Vector3(0, 2, -5f);
        transform.localEulerAngles = new Vector3(50, 0, 0);
        //AudioListener.volume = PlayerPrefs.GetFloat("vol_master");
    }

    public void InitPosition(Vector3 targetPos)
    {

        //reset position on this time

        transform.localEulerAngles = new Vector3(50, 0, 0);
        Vector3 distancetarget = new Vector3(0, 2, -5f);
        Vector3 chaposition = targetPos + distancetarget;
        transform.position = chaposition;

        //Debug.Log(transform.position);

        limitpos = transform.position;
    }

    public void Topview()
    {
        distancetarget = new Vector3(0, 3, -0.1f);
        topviewon = 1;
        topviewdelay = 5;
        movespeed = 5;
    }

    public void CancelTopView()
    {
        if (topviewdelay > 0)
        {
            topviewdelay = 0.001f;
        }
    }

    public void LookTarget(Transform _target, int _fov, float _delay)
    {
        target = _target;

        if (_delay > 0)
        {
            resetstart = true;
            resetcam_delay = _delay;
        }
        if (_fov != 0)
        {
            ZoomIn(-1, _fov, _delay);
        }
    }

    public void IconHideStop(bool _hidestop)
    {
        hidestop = _hidestop;
    }

    public void ResetCam()
    {
        target = cha1;
        fov = originfov;
        fovchange = true;
        resetstart = false;
        zoom = false;
        zoomdelay = 0;
        movespeed = camParam.speed;
    }

    public bool fllow_back = true;
    private float _CurrentCameraAngleAroundX;
    private float _CurrentCameraAngleAroundY;
    float PitchMinimum = 0;
    float PitchMaximum = 360;
    private bool ReverseAxisY = false;
    float MouseMotionScaleLevel = 360f;
    void Update()
    {
        if (resetstart)
        {
            if (resetcam_delay > 0)
                resetcam_delay -= Time.deltaTime;
            else
            {
                resetcam_delay = 0;
                resetstart = false;
                ResetCam();
                ///cha1.GetComponent<CharControl>().StartControl();
            }
        }

        
        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     _CurrentCameraAngleAroundX += Input.GetAxis("Mouse Y") * 360 * Time.deltaTime * (ReverseAxisY ? -1 : 1);
        //     _CurrentCameraAngleAroundY += Input.GetAxis("Mouse X") * MouseMotionScaleLevel * Time.deltaTime;
        //     _CurrentCameraAngleAroundX = Mathf.Clamp(_CurrentCameraAngleAroundX, PitchMinimum, PitchMaximum);
        //     Vector3 _PositionTarget = target.position + Quaternion.Euler(_CurrentCameraAngleAroundX, _CurrentCameraAngleAroundY, 0) * (-target.forward * 10);
        //
        //     //transform.position = Vector3.Lerp(transform.position, _PositionTarget, Time.deltaTime * CameraRepositionSpeed);
        //     transform.position = _PositionTarget;
        //     transform.LookAt(target);    
        // }
        // else if(Input.GetKeyUp(KeyCode.A))
        // {
            //target.TransformDirection(Offset) 将偏移从局部坐标变为世界坐标,达到摄像机永远在角色背后的目的
            transform.position = Vector3.Lerp(transform.position, target.position + target.TransformDirection(distancetarget), movespeed);
            //摄像机朝向角色
            transform.LookAt(target);
    
        // }
        
        
    }
    
    public void ZoomIn(int _zoomspeed, int _fov, float _delay)
    {
        zoom = true;
        z_speed = _zoomspeed;
        fov = _fov;
        z_time = _delay;
        fovchange = true;
    }

    public void Hitcam()
    {
        dx = -dx;
        transform.position += hit_shake1 * dx;
        /*if  (mycamera.fieldOfView>23)
        {
            mycamera.fieldOfView -= 1;
        }*/
        //fovchange = true;
    }

    public void Hitcam2(float _factor)
    {
        transform.position += hit_shake3 * _factor;
        //camera.fieldOfView -= 1;
    }

    /// <summary>
    /// 记录当前参数
    /// </summary>
    public void RecordCamParams(bool isEnable)
    {
        // record params of camera  TODO:
        camParam = new CamParam(transform.localPosition, transform.localRotation, mycamera.fieldOfView, movespeed);
        enabled = isEnable;
    }

    /// <summary>
    /// 恢复参数，启用脚本
    /// </summary>
    public void RecoverCamParams()
    {
        mycamera.transform.position = limitpos;
        mycamera.transform.localEulerAngles = new Vector3(50, 0, 0);
        mycamera.fieldOfView = camParam.field;
        enabled = true;
    }

    void KMDebug()
    {

    }

    public void BlackFog(bool b)
    {
        if (m_blackFog)
            if (b)
                m_blackFog.gameObject.SetActive(true);
            else
                m_blackFog.gameObject.SetActive(false);
    }

    public void SetSpeed(short speed)
    {
        movespeed = speed;
    }
}
