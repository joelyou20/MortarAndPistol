using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float sprintSpeed = 15f;
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;  // How much to smooth out the movement
    public GameObject groundCheck;
    public LayerMask whatIsGround;
    public bool enableSprint = true;

    private float _movement;
    private Rigidbody2D _rb;
    private Vector3 _velocity = Vector3.zero;
    private float _speed;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _movement = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if(GroundChecker.Check(groundCheck, whatIsGround))
            _speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        Vector3 targetVelocity = 
            new Vector2((_movement * _speed) * 10f * Time.fixedDeltaTime, _rb.velocity.y);

        _rb.velocity = Vector3.SmoothDamp(
            _rb.velocity,
            targetVelocity,
            ref _velocity,
            movementSmoothing);
    }
}
