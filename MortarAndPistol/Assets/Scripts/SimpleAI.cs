using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SimpleAI : MonoBehaviour
{
    public bool isStationary = false;
    public LayerMask whatIsGround;

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

    public void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _facingComponent = GetComponent<Facing>();
        _speed = GetComponent<Enemy>().speed;
    }

    public void Update()
    {
        _facing = _facingComponent.GetFacing();
        if (!isStationary)
        {
            if (!IsTargetObstructed(gameObject, _player))
            {
                MoveTowardsPlayer();
            }
            else
            {
                Wander();
            }
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
        if (CalculateDistance(transform.position, _player.transform.position, true) > rad &&
            IsGroundAhead())
        {
            transform.Translate(new Vector3(_speed * Time.deltaTime, 0, 0));
        }
    }

    private void Wander()
    {
        if(Time.deltaTime % 3 == 0)
        {
            if(!IsGroundAhead())
            {
                Flip();
            }

            var rand = new Random();
            var value = rand.Next(0, 1);
            if(value == 0) { Flip(); }

            transform.Translate(new Vector3(_speed * Time.deltaTime, 0, 0));
        }
    }

    private void Flip()
    {
        _facingComponent.SetFacing(Math.Abs(_facing - 1));
    }

    private Vector3 GetDirectionVector(GameObject origin, GameObject target)
    {
        Vector3 fromPosition = origin.transform.position;
        Vector3 toPosition = target.transform.position;
        return toPosition - fromPosition;
    }

    public bool IsTargetObstructed(GameObject origin, GameObject target)
    {
        Physics2D.queriesStartInColliders = false;
        var hit = Physics2D.Raycast(origin.transform.position, target.transform.position);
        if(hit && hit.collider && hit.collider.gameObject.tag.Equals("Player"))
        {
            Debug.DrawLine(origin.transform.position, target.transform.position, Color.green);
        }
        else
        {
            Debug.DrawLine(origin.transform.position, target.transform.position, Color.red);
        }

        if (!hit || !hit.collider)
        {
            return false;
        }

        return hit.collider.gameObject.name == target.name;
    }

    public void DrawLine(GameObject origin, GameObject target)
    {
        if(IsTargetObstructed(origin, target))
        {
            Debug.DrawLine(origin.transform.position, target.transform.position, Color.red);
        }
        else
        {
            Debug.DrawLine(origin.transform.position, target.transform.position, Color.green);
        }
    }

    private bool IsGroundAhead()
    {
        if (!_facingComponent)
        {
            Debug.LogError("Object has no facing component.");
        }
        var pos = transform.position;
        var offset = _facing == 0 ? -1 : 1;
        var pointAhead = new Vector2(pos.x - offset * 2, 0);
        var downVector = new Vector2(pos.x - offset * 2, -1 * 20);
        var hit = Physics2D.Linecast(pointAhead, downVector);
        //Debug.DrawLine(pointAhead, downVector);
        if (!hit || 
            hit.distance > 5 ||
            !GroundChecker.CompareLayerToLayerMask(hit.collider.gameObject.layer))
        {
            return false;
        }

        return true;

    }
}
