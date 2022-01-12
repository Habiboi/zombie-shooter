using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCodes : MonoBehaviour
{
    public Transform target;
    private Vector3 offset;
    void Start()
    {
        offset = target.position - transform.position;
    }

    void FixedUpdate()
    {
        Vector3 pos = target.position - offset;
        transform.position = Vector3.Lerp(transform.position, pos, 10f * Time.fixedDeltaTime);
    }
}
