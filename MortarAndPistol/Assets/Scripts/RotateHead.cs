using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHead : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 lookDirection;
        lookDirection = Input.mousePosition;
        lookDirection.z = 0.0f;
        lookDirection = Camera.main.ScreenToWorldPoint(lookDirection);
        lookDirection = lookDirection - transform.position;

        transform.rotation = new Quaternion(lookDirection.x, lookDirection.y, 0, 0);
    }
}
