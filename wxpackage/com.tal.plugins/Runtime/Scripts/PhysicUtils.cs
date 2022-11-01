using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicUtils
{
    /**
     * 射线拾取物体，返回Transform
     */
    public static Transform PhysicsRay(Vector3 pos)
    {
        // Ray rays = Camera.main.ScreenPointToRay(pos);
        //当弃用mian摄像机 启动第三摄像机时
        Ray rays =
            Camera.main
                ? Camera.main.ScreenPointToRay(pos)
                : Camera
                    .allCameras[Camera.allCameras.Length - 1]
                    .ScreenPointToRay(pos);

        Debug.DrawRay(rays.origin, rays.direction, Color.blue, 1);
        RaycastHit hit;
        if (
            Physics.Raycast(rays, out hit) //这里Physics.Raycast方法返回的是bool类型，当光线与任何碰撞体相交时，
        )
        //返回 true，否则返回 false。
        {
            return hit.transform;
        }
        return null;
    }

    public static Transform PhyscicsRayWithLayer(Vector3 pos, string layer)
    {
        // Ray rays = Camera.main.ScreenPointToRay(pos);
        //当弃用mian摄像机 启动第三摄像机时
        Ray rays =
            Camera.main
                ? Camera.main.ScreenPointToRay(pos)
                : Camera
                    .allCameras[Camera.allCameras.Length - 1]
                    .ScreenPointToRay(pos);
        Debug.DrawRay(rays.origin, rays.direction, Color.blue, 1);
        RaycastHit hit;
        if (
            Physics
                .Raycast(rays, out hit, 100, 1 << LayerMask.NameToLayer(layer)) //这里Physics.Raycast方法返回的是bool类型，当光线与任何碰撞体相交时，
        )
        //返回 true，否则返回 false。
        {
            return hit.transform;
        }
        return null;
    }
}
