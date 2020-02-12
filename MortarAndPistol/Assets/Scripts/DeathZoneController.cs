using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZoneController : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(col.gameObject);
    }

    private LayerMask GetWhatCanDieLayers()
    {
        var player_Layer = 8;
        var enemy_Layer = 10;

        var player_LayerMask = 1 << player_Layer;
        var enemy_LayerMask = 1 << enemy_Layer;

        return
            player_LayerMask |
            enemy_LayerMask;
    }
}
