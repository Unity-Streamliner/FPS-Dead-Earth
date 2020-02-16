using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] public float VerticalSpeed = 5.67f;
    [SerializeField] public float HorizontalSpeed = 2.32f;
    private int _horizontalHash = 0;
    private int _verticalHash = 0;
    private int _attackHash = 0;
    private Animator _animatorController = null;
    // Start is called before the first frame update
    void Start()
    {
        _animatorController = GetComponent<Animator>();
        _horizontalHash = Animator.StringToHash("Horizontal");
        _verticalHash = Animator.StringToHash("Vertical");
        _attackHash = Animator.StringToHash("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0)) _animatorController.SetTrigger(_attackHash);
        _animatorController.SetFloat(_verticalHash, verticalInput * VerticalSpeed, 1.0f, Time.deltaTime);
        _animatorController.SetFloat(_horizontalHash, horizontalInput * HorizontalSpeed, 0.1f, Time.deltaTime);
    }
}
