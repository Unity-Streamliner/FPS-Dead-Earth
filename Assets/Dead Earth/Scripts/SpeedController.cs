using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{

    [SerializeField] public float Speed = 1.0f;

    private Animator _animatorController;

    // Start is called before the first frame update
    void Start()
    {
        _animatorController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _animatorController.SetFloat("Speed", Speed);
    }
}
