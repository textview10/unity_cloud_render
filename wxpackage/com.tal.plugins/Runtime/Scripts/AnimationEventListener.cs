using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[RequireComponent(typeof(Animator))]
public class AnimationEventListener : MonoBehaviour
{
    public GameObject gameObj;
    private Animator _animator;
    private Dictionary<string, MyAnimationEvent> _eventsMap;

    public delegate void Callback();
    
    // 绑定脚本到gameObject
    public static AnimationEventListener BindListener(GameObject go)
    {
        if (go == null)
        {
            return null;
        }
        AnimationEventListener eventListener = go.GetComponent<AnimationEventListener>();
        if (eventListener == null)
        {
            eventListener = go.AddComponent<AnimationEventListener>();
        }
        eventListener.gameObj = go;
        return eventListener;
    }
    
    // 自定义动画事件
    private class MyAnimationEvent
    {
        private event Callback Callbacks;
        
        public MyAnimationEvent(Callback callback)
        {
            Callbacks += callback;
        }
        
        public void RemoveCallback(Callback callback)
        {
            Callbacks -= callback;
        }

        public void Callback()
        {
            Callbacks?.Invoke();
        }
    }

    // 注册动画监听
    public bool AddAnimationEvent(string animationName, float time, Callback callback)
    {
        if (_animator == null)
        {
            _animator = gameObj.GetComponent<Animator>();
        }

        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
        AnimationClip target = null;
        
        foreach (AnimationClip clip in clips)
        {
            if (animationName == clip.name)
            {
                target = clip;
                if (time == 0)
                {
                    time = clip.length;
                }
                break;
            }
        }

        if (target == null)
        {
            return false;
        }
        
        if (_eventsMap == null)
        {
            _eventsMap = new Dictionary<string, MyAnimationEvent>();
        }

        MyAnimationEvent myEvent = null;
        if (!_eventsMap.TryGetValue(animationName, out myEvent))
        {
            AnimationEvent animEvent = new AnimationEvent
            {
                time = time,
                functionName = "AnimationEventCallback",
                stringParameter = animationName
            };
            myEvent = new MyAnimationEvent(callback);
            _eventsMap[animationName] = myEvent;
            target.AddEvent(animEvent);
        }
        return true;
    }

    //动画回调
    private void AnimationEventCallback(String animationName)
    {
        if (animationName == null)
        {
            return;
        }

        if (_eventsMap.TryGetValue(animationName, out MyAnimationEvent myEvent))
        {
            myEvent?.Callback();
        }
    }
}