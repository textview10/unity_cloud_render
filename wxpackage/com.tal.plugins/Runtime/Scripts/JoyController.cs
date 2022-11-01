using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Com.Tal.Unity.Asset;
using Com.Tal.Unity.Core;
using Com.Tal.Unity.UI;
using HedgehogTeam.EasyTouch;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public delegate void JoystickControlDele(GameObject go);
    public class JoyController : MonoBehaviour,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler,
        IPointerDownHandler
    {
        public float raduis;

        public RectTransform handle; //中心把手
        public RectTransform bgJoy; //摇杆的背景

        public JoystickControlDele joystickControl;
        
        #region -----JoyStick------
        public void OnBeginDrag(PointerEventData eventData)
        {
            //CharacterController
            //Debug.Log("OnBeginDrag");
        }

        public Action onEndDragEvent;

        Vector2 localPos;
        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log("OnDrag");
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                bgJoy,
                eventData.position,
                eventData.pressEventCamera,
                out localPos
            );
            
            Vector2 dir = localPos - Vector2.zero;
            if (dir.magnitude > raduis)
            {
                localPos = dir.normalized * raduis;
            }

            handle.localPosition = localPos;
            onAgengMove?.Invoke(true);
        }

        public bool GetLocalMagnitude()
        {
            return localPos.magnitude >= 0.1f;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //Debug.Log("OnEndDrag");
            handle.anchoredPosition = Vector2.zero;
            localPos = Vector2.zero;
            onEndDragEvent?.Invoke();
            //bgJoy.gameObject.SetActive(false);
            onAgengMove?.Invoke(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //Debug.Log("OnPointerDown");
            bgJoy.gameObject.SetActive(true);

            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localPos
            );
            //bgJoy.localPosition = localPos;
        }
        #endregion

       
        private Animator mAvatarAnimator;
        private Transform mainCamera;
        Vector3 dir = Vector3.zero;
        async void Start()
        {
            mainCamera = Camera.main.transform;
            if (mAvatar)
            {
                _agent = mAvatar.GetComponent<NavMeshAgent>();
                dir = mainCamera.transform.position - mAvatar.transform.position;
            }

           
            
        }

        /// <summary>
        /// 注册进控制主角
        /// </summary>
        /// <param name="go"></param>
        public void SetAvatar(GameObject go)
        {
            mAvatar = go;
            _agent = go.GetComponent<NavMeshAgent>();
            
            Debug.Log("注册进来了，"+go.name);
            if (mainCamera == null || mAvatar == null)
            {
                return;
            }

            // maxheight = mAvatar.transform.position.y + maxheight;
            // minheight = mAvatar.transform.position.y + minheight;
            
            //Debug.LogError("max:"+maxHeight+", mAvatar.transform.position.y:"+ mAvatar.transform.position.y);
            
            mainCamera.transform.position = mAvatar.transform.position +
                                            (Vector3.up * hightDis - mAvatar.transform.forward * backDis);
            dir = mainCamera.transform.position - mAvatar.transform.position;
        }

        public void SetAnimator(Animator animator)
        {
            mAvatarAnimator = animator;
        }

        public void SetAvatarNull()
        {
            mAvatar = null;
        }

        /// <summary>
        /// 注册进摄像机，默认使用主摄像机
        /// </summary>
        /// <param name="camGO"></param>
        public void SetCamera(GameObject camGO)
        {
            mainCamera = camGO.transform;
            if (mainCamera == null || mAvatar == null)
            {
                return;
            }
            
            dir = mainCamera.transform.position - mAvatar.transform.position;
        }

        private Vector2 moveVector = Vector2.zero;

        public void OnMoveEnd()
        {
            moveVector = Vector2.zero;
        }

        public void OnMoveSpeed(Vector2 v)
        {
            moveVector = v;
            //Debug.LogError("OnMoveSpeed:" + v);
        }
        private Vector3 _nextPosition;
        private Vector3 _nowPosition;
        private NavMeshAgent _agent;
         GameObject mAvatar;
        public float XVector = 40;
        public float YVector = 200;
        public float maxheight = 5;
        public float minheight = 2f;
        public float lookAtHight = 0.5f;
        public float backDis = 1.5f,hightDis = 2;
        public Action<bool> onAgengMove;
        public Action<Vector3> onPositionChanged;
        public Action<Vector2> onMoveDirChanged;

        public void SetAgentMove(Action<bool> callBack)
        {
            onAgengMove = callBack;
        }

        public void SetPositionChanged(Action<Vector3> callBack)
        {
            onPositionChanged = callBack;
        }

        public void SetMoveDirChanged(Action<Vector2> callBack)
        {
            onMoveDirChanged = callBack;
        }

        private void Update()
        {
            if (mAvatar == null || mainCamera == null)
            {
                return;
            }

#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.J))
            {
                mAvatarAnimator.SetTrigger("Jump");
            }
#endif

        //Debug.LogError("OnMoveSpeed:" + v);
        float x = moveVector.x*3,y = moveVector.y;
            if (x > XVector)
            {
                x = XVector;
            }else if (x < -XVector)
            {
                x = -XVector;
            }

            if (y > YVector)
            {
                y = YVector;
            }else if (y < -YVector)
            {
                y = -YVector;
            }
            onMoveDirChanged?.Invoke(localPos.normalized);
            dir = Quaternion.AngleAxis(-x * 0.03f, Vector3.up) * dir;
            
            Vector3 pos = mAvatar.transform.position + dir;
            mainCamera.transform.position = pos;
            mainCamera.transform.position += new Vector3(0, -y * 0.01f, 0);
            if (mainCamera.transform.position.y > mAvatar.transform.position.y + maxheight)
            {
                new Vector3(mainCamera.transform.position.x, mAvatar.transform.position.y + maxheight, mainCamera.transform.position.z);
            }
            else if (mainCamera.transform.position.y < mAvatar.transform.position.y + minheight)
            {
                mainCamera.transform.position =
                    new Vector3(mainCamera.transform.position.x, mAvatar.transform.position.y + minheight, mainCamera.transform.position.z);
            }
            mainCamera.transform.LookAt(mAvatar.transform.position+Vector3.up*0.5f);
            dir = mainCamera.transform.position - mAvatar.transform.position;
            
            if (localPos.magnitude>= 0.1f )
            {
                if(_agent) _agent.isStopped = false;
                Vector3 forwardV3 = new Vector3(mAvatar.transform.position.x - mainCamera.position.x,
                    0,
                    mAvatar.transform.position.z - mainCamera.position.z);
                var angle =Math.Acos( Vector2.Dot(new Vector2(0, 1), localPos.normalized))*Mathf.Rad2Deg;
                Vector3 cos = Vector3.Cross( new Vector3(0,1) , localPos);

                if (cos.z > 0)
                {
                    angle = 360 - angle;
                    mAvatar.transform.rotation =Quaternion.LookRotation( Quaternion.AngleAxis((float)angle,Vector3.up)*forwardV3);
                }
                else
                {
                    mAvatar.transform.rotation =Quaternion.LookRotation( Quaternion.AngleAxis((float)angle,Vector3.up)*forwardV3);
                }

                // mAvatar.transform.position += mAvatar.transform.forward * localPos.magnitude * Time.deltaTime * 0.05f;

                _nowPosition = mAvatar.transform.position;
                _nextPosition = _nowPosition += mAvatar.transform.forward * localPos.magnitude *Time.deltaTime * 0.6f;
                if (_agent == null)
                {
                    Debug.Log("对象上没有挂NavMeshAgent");
                    mAvatar.transform.position += mAvatar.transform.forward * localPos.magnitude * Time.deltaTime * 0.05f;
                }
                else
                {
                    _agent.SetDestination(_nextPosition);
                    
                }
                onPositionChanged?.Invoke(_nextPosition);
                if (mAvatarAnimator)
                {
                    mAvatarAnimator.SetBool("run",true);
                }
            }
            else
            {
                if(_agent != null) _agent.isStopped = true;
                if (mAvatarAnimator)
                {
                    mAvatarAnimator.SetBool("run",false);
                }
            }
        }

        private void OnDestroy()
        {
            onAgengMove = null;
            onPositionChanged = null;
            onMoveDirChanged = null;
        }
    }

