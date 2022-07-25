using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(target.position + offset, Vector3.one * 0.25f);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(target.position + offset, transform.forward*3);
        }
    }
#endif

}
