using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : EnemyAI
{
    private enum State
    {
        Idle,
        Aggressive
    }

    public bool isStationary = false;

    private GameObject _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (!isStationary)
        {
            Move();
        }
    }

    private float CalculateDistance(Vector2 p1, Vector2 p2, bool abs)
    {
        var sign = (p2.x - p1.x) >= 0 ? 1 : -1;
        return Mathf.Sqrt(Mathf.Pow((p2.x - p1.x), 2) + Mathf.Pow((p2.y - p1.y), 2)) * (abs ? 1 : sign);
    }

    private void Move()
    {
        // Get Player Collider radius
        var col = _player.GetComponent<Collider2D>();
        float rad = (col != null ? col.bounds.size.x : 1) * 1;

        //rotate to look at the player
        transform.LookAt(_player.transform.position);
        transform.Rotate(new Vector3(0, -90, 0), Space.Self);//correcting the original rotation

        //move towards the player
        if (CalculateDistance(transform.position, _player.transform.position, true) > rad)
        {//move if distance from target is greater than 1
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
    }

    private Vector3 GetDirectionVector(GameObject origin, GameObject target)
    {
        Vector3 fromPosition = origin.transform.position;
        Vector3 toPosition = target.transform.position;
        return toPosition - fromPosition;
    }

    public static bool IsTargetObstructed(GameObject origin, GameObject target)
    {
        var hit = Physics2D.Linecast(origin.transform.position, target.transform.position);
        return hit.collider.gameObject.name == target.name;
    }

    public static void DrawLine(GameObject origin, GameObject target)
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
}
