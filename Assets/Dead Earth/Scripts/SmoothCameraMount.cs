using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraMount : MonoBehaviour
{

    [SerializeField] public Transform Mount = null;
    [SerializeField] public float Speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, Mount.position, Time.deltaTime * Speed);
        transform.rotation = Quaternion.Slerp(transform.rotation, Mount.rotation, Time.deltaTime * Speed);
    }
}
