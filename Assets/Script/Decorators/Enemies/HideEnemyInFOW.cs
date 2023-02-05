using System.Collections;
using System.Collections.Generic;
using Script.Decorators.Biomes;
using Unity.VisualScripting;
using UnityEngine;

public class HideEnemyInFOW : MonoBehaviour
{
    void Update()
    {
        Tile tile = GetTileUnderneath();
        bool renderingEnabled = tile == null || tile.GetComponentInChildren<Biome>() == null;
        MeshRenderer r = GetComponentInChildren<MeshRenderer>();
        if (r != null)
        {
            r.enabled = !renderingEnabled;
        }
    }
    
    public Tile GetTileUnderneath()
    {
        RaycastHit hit;
        if (Physics.Raycast(
                transform.position,
                Vector3.down,
                out hit,
                500f,
                LayerMask.GetMask("Tiles")
            ))
        {
            return hit.collider.GetComponent<Tile>();
        }

        return null;
    }
}
