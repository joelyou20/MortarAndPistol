using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker
{
    public static bool Check(GameObject groundCheck, LayerMask whatIsGround)
    {
        var dist = 0.5f;
        var pos = groundCheck.transform.position;
        pos = new Vector3(pos.x, pos.y - 0.1f, pos.z);

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, dist, whatIsGround);
        if (hit.collider)
        {
            return true;
        }

        return false;
    }
}
