using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SimpleAI : MonoBehaviour
{
    public float jumpForce = 5;
    public bool isStationary = false;
    public float exclamationPoint_yOffset = 1;
    public float detectionRadius = 10f;
    public LayerMask whatIsNotTarget;
    public float groundAlert_distance = 2;
    public float wallAlert_distance = 2;
    public GameObject groundCheck;

    private enum State
    {
        Idle,
        Aggressive
    }

    private GameObject _player;
    private int _facing = 0;
    private float wanderVariance = 3f;
    private Facing _facingComponent;
    private float _speed;
    private Rigidbody2D _rb;
    private Vector3 _velocity = Vector3.zero;
    private float _movementSmoothing = 0.05f;
    private int _movement = 0;
    private bool _jump = false;

    public void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _facingComponent = GetComponent<Facing>();
        _speed = GetComponent<Enemy>().speed;
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        _facing = _facingComponent.GetFacing();

        if (!isStationary)
        {
            Monitor();
        }
    }

    public void FixedUpdate()
    {
        if(_jump)
        {
            Jump();
        }
    }

    private float CalculateDistance(Vector2 p1, Vector2 p2, bool abs)
    {
        var sign = (p2.x - p1.x) >= 0 ? 1 : -1;
        return Mathf.Sqrt(Mathf.Pow((p2.x - p1.x), 2) + Mathf.Pow((p2.y - p1.y), 2)) * (abs ? 1 : sign);
    }

    private void MoveTowardsPlayer()
    {
        // Get Player Collider radius
        var col = _player.GetComponent<Collider2D>();
        float rad = (col != null ? col.bounds.size.x : 1) * 1;

        //rotate to look at the player
        transform.LookAt(_player.transform.position);
        transform.Rotate(new Vector3(0, -90, 0), Space.Self);//correcting the original rotation

        //move towards the player
        var dist = CalculateDistance(transform.position, _player.transform.position, true);

        if (dist > rad && IsGround(true))
        {
            _movement = GetDirectionVector(transform.position, _player.transform.position).x > 0 ? 1 : -1;
            
            Vector3 targetVelocity =
               new Vector2((_movement * _speed) * 10f * Time.fixedDeltaTime, _rb.velocity.y);

            /*
            if(IsWall(true))
            {
                _jump = true;
            }
            */

            _rb.velocity = Vector3.SmoothDamp(
                _rb.velocity,
                targetVelocity,
                ref _velocity,
                _movementSmoothing);

            //transform.Translate(new Vector3(_speed * Time.deltaTime, 0, 0));
        }
        else 
        { 
            Flip();
        }
    }

    private void Monitor()
    {
        if (IsTargetWithinView(_player))
        {
            MoveTowardsPlayer();
        }
        else
        {
            Wander();
        }
    }

    private void Wander()
    {
        if(Mathf.RoundToInt(Time.time) % 3 == 0)
        {
            Debug.Log(transform.name + " Is ground ahead = " + IsGround(true));
            Debug.Log(transform.name + " Is ground behind = " + IsGround(false));
            Debug.Log(transform.name + " Is wall ahead = " + IsWall(true));
            Debug.Log(transform.name + " Is wall behind = " + IsWall(false));
            if (!IsGround(true) && !IsGround(false) ||
                IsWall(true) && IsWall(false))
            {
                Jump();
                return;
            }
            else if (
                (!IsGround(true) && IsGround(false)) ||
                IsWall(true) && !IsWall(false))
            {
                Flip();
            }
            else
            {
                var rand = new Random();
                var value = rand.Next(0, 2);
                if (value == 0) { Flip(); }
            }

            Vector3 targetVelocity =
                new Vector2(transform.right.x * _speed * 10f * Time.fixedDeltaTime, _rb.velocity.y);

            _rb.velocity = Vector3.SmoothDamp(
                _rb.velocity,
                targetVelocity,
                ref _velocity,
                _movementSmoothing);
            
        }
    }

    private void Jump()
    {
        if (GroundChecker.Check(groundCheck))
        {
            _jump = false;
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce * 2);
        }
    }

    private void Flip()
    {
        _facingComponent.SetFacing(Math.Abs(_facing - 1));
    }

    private float GetMovementDirection()
    {
        return _rb.velocity.x == 0 ? 0 : _rb.velocity.x > 0 ? 1 : -1;
    }

    private Vector2 GetDirectionVector(Vector2 origin, Vector2 target)
    {
        Vector2 fromPosition = origin;
        Vector2 toPosition = target;
        return toPosition - fromPosition;
    }

    public bool IsTargetWithinView(GameObject target)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            GetDirectionVector(
                transform.position,
                _player.transform.position),
            10f,
            whatIsNotTarget);

        //DrawLineToPlayer(hit);

        if (!hit || 
            !hit.collider || 
            (CalculateDistance(transform.position, target.transform.position, true) > detectionRadius))
        {
            return false;
        }

        return hit.collider.gameObject.name.Equals(target.name);
    }

    public void DrawLineToPlayer(RaycastHit2D hit)
    {
        if (!hit)
        {
            Debug.DrawRay(
            transform.position,
            GetDirectionVector(
                transform.position,
                _player.transform.position), Color.red);
        }
        else if (hit && !hit.collider)
        {
            Debug.DrawRay(
            transform.position,
            GetDirectionVector(
                transform.position,
                _player.transform.position), Color.yellow);
        }
        else if (hit && hit.collider && !hit.collider.gameObject.name.Equals(_player.name))
        {

            Debug.DrawRay(
            transform.position,
            GetDirectionVector(
                transform.position,
                _player.transform.position), Color.blue);
        }
        else
        {
            Debug.DrawRay(
            transform.position,
            GetDirectionVector(
                transform.position,
                _player.transform.position), Color.green);
        }
    }

    private bool IsGround(bool ahead)
    {
        if (!_facingComponent)
        {
            Debug.LogError("Object has no facing component.");
        }
        var pos = transform.position;
        var offset = _facing == 0 ? -1 : 1;
        Vector2 point;
        Vector2 downVector;
        if (ahead)
        {
            point = new Vector2(pos.x + offset - groundAlert_distance, 0);
            downVector = new Vector2(pos.x + offset - groundAlert_distance, -1 * 20);
        }
        else
        {
            point = new Vector2(pos.x + offset + groundAlert_distance, 0);
            downVector = new Vector2(pos.x + offset + groundAlert_distance, -1 * 20);
        }
        var hit = Physics2D.Raycast(point, GetDirectionVector(point, downVector));
        
        Debug.DrawRay(point, GetDirectionVector(point, downVector));
        if (!hit ||
            !hit.collider ||
            GroundChecker.GetWhatIsGround() !=
                (GroundChecker.GetWhatIsGround() | (1 << hit.collider.gameObject.layer)))
        {
            return false;
        }

        return true;
    }

    private bool IsWall(bool ahead)
    {
        var point = transform.position;
        RaycastHit2D hit;
        if (ahead)
        {
            var horizontalVector =
                GetDirectionVector(point,
                    new Vector2(point.x * -1, point.y));
            hit = Physics2D.Raycast(point, horizontalVector);
        }
        else
        {
            var horizontalVector =
                GetDirectionVector(point,
                    new Vector2(point.x, point.y));
            hit = Physics2D.Raycast(point, horizontalVector);
        }
        if (hit &&
            hit.collider &&
            GroundChecker.GetWhatIsGround() == 
                (GroundChecker.GetWhatIsGround() | (1 << hit.collider.gameObject.layer)))
        {
            return true;
        }

        return false;
    }
}
