using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private enum JumpType
    {
        Charge,
        Hold,
        Flat
    };

    [SerializeField] private float fallMultipler = 2.5f;
    [SerializeField] private bool fastFalling = true;
    [SerializeField] private JumpType jumpType = JumpType.Charge;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private GameObject ceilingCheck;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask whatIsGround;
    public float jumpTime;

    private Rigidbody2D _rb;
    private bool _grounded = false;
    private bool _jump = false;
    private float _jumpVelocity = 0f;
    private Timer _jumpTimer;
    private float jumpTimeCounter;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _jumpTimer = new Timer(50);
        _jumpTimer.AutoReset = true;
        _jumpTimer.Elapsed += _OnJumpTimerElapsed;
    }

    private void _OnJumpTimerElapsed(object sender, ElapsedEventArgs e)
    {
        _jumpVelocity += 0.5f;
        if (_jumpVelocity >= 10f)
        {
            _jumpTimer.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpType == JumpType.Charge)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _jumpTimer.Start();
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (GroundChecker.Check(groundCheck, whatIsGround))
                {
                    _jumpTimer.Stop();
                    _jump = true;
                }
                else
                {
                    _jumpTimer.Stop();
                    _jumpVelocity = 0;
                }
            }
        }

        if (jumpType == JumpType.Flat)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _jump = true;
            }
        }

        if (jumpType == JumpType.Hold)
        {
            Debug.Log(GroundChecker.Check(groundCheck, whatIsGround));
            if (GroundChecker.Check(groundCheck, whatIsGround) && Input.GetKeyDown(KeyCode.Space))
            {
                _jump = true;
                jumpTimeCounter = jumpTime;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                _jump = false;
            }
        }

        if (fastFalling && (_rb.velocity.y < 0))
        {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultipler - 1) * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (_jump)
        {
            Debug.Log(_jump);
            ExecuteJump();
        }
    }

    private void ExecuteJump()
    {
        switch (jumpType)
        {
            case JumpType.Charge:
                Charge_Jump();
                break;
            case JumpType.Hold:
                Hold_Jump();
                break;
            case JumpType.Flat:
                Flat_Jump();
                break;
            default:
                break;
        }
    }

    private void Flat_Jump()
    {
        _jump = false;
        _rb.velocity = new Vector2(_rb.velocity.x, jumpForce * 2);
    }

    private void Hold_Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
        if (Input.GetKey(KeyCode.Space) && _jump)
        {
            if (jumpTimeCounter > 0)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                _jump = false;
            }
        }
        if (BumpHead())
        {
            _jump = false;
        }
    }

    private bool BumpHead()
    {
        var dist = 0.5f;
        var pos = ceilingCheck.transform.position;
        pos = new Vector3(pos.x, pos.y + 0.1f, pos.z);

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.up, dist, whatIsGround);
        if (hit.collider)
        {
            return true;
        }

        return false;
    }

    private void Charge_Jump()
    {
        _jump = false;
        _rb.velocity = new Vector2(_rb.velocity.x, jumpForce + _jumpVelocity);
        _jumpVelocity = 0;
    }
}
