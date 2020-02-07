using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker
{
    /*
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
    */

    public static bool Check(GameObject groundCheck)
    {
        // checks if you are within 0.15 position in the Y of the ground
        return Physics2D.OverlapCircle(groundCheck.transform.position, 0.15f, GetWhatIsGround());
    }

    public static bool Check(GameObject groundCheck, LayerMask whatIsGround)
    {
        // checks if you are within 0.15 position in the Y of the ground
        return Physics2D.OverlapCircle(groundCheck.transform.position, 0.15f, whatIsGround);
    }

    public static LayerMask GetWhatIsGround()
    {
        var default_Layer = 1;
        var transparentFX_Layer = 2;
        var water_Layer = 4;
        var ui_Layer = 5;
        var ground_Layer = 9;

        var default_LayerMask = 1 << default_Layer;
        var transparentFX_LayerMask = 1 << transparentFX_Layer;
        var water_LayerMask = 1 << water_Layer;
        var ui_LayerMask = 1 << ui_Layer;
        var ground_LayerMask = 1 << ground_Layer;

        return
            default_LayerMask |
            transparentFX_LayerMask |
            water_LayerMask |
            ui_LayerMask |
            ground_LayerMask;
    }

    public static bool CompareLayerToLayerMask(int layer)
    {
        return ((1 << layer) & GetWhatIsGround()) != 0;
    }

    public static bool CompareLayerToLayerMask(int layer, LayerMask layerMask)
    {
        return ((1 << layer) & layerMask) != 0;
    }
}