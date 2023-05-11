using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGrid : TileGrid
{
    public override void OnStart()
    {
        base.OnStart();

        SetTile(new Vector2Int(1, 1), ID.grass, ID.carbonDioxite, 20000, 0);
    }
}
