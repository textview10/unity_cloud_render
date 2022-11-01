#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorGizmos : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color (1,1,1, 0.8f);
        Gizmos.DrawSphere (transform.position, 0.3f);
    }
}
#endif
