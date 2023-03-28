using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{
    public Tilemap tilemap;
    BoundsInt bounds;
    TileBase[] allTiles;
    void Start()
    {
        bounds = tilemap.cellBounds;
        allTiles = tilemap.GetTilesBlock(bounds);

        foreach(Tile t in allTiles)
        {
            Debug.Log(t.transform.GetPosition().x);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
