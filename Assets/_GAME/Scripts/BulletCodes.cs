using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCodes : MonoBehaviour
{
    public Vector3 moveVector;
    public float speed;
    public float endTime;
    void Start()
    {
        Invoke("DestroySelf", endTime);
    }

    void Update()
    {
        transform.position += moveVector * Time.deltaTime * speed;
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
