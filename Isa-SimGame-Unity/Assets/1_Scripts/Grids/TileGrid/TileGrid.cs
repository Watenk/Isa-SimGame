using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : BaseClass
{
    public TileGridRenderer gridRenderer;

    public int Width; 
    public int Height;

    protected Tile[,] gridArray;

    public override void OnStart()
    {
        gridArray = new Tile[Width, Height];

        InitializeGrid();
        PrintGridSize();
        gridRenderer.Draw();
    }

    //---------------------------------------------------------------------

    public Tile GetTile(Vector2Int pos) 
    {
        if (IsInGridBounds(pos))
        {
            return gridArray[pos.x, pos.y]; 
        }
        return null;
    }

    public void SetTile(Vector2Int pos, ID groundID, int oxygenAmount, int carbonDioxideAmount, int temp, int lightLevel, int humidity, int fertility) 
    { 
        if (IsInGridBounds(pos))
        {
            Tile currentTile = GetTile(pos);
            currentTile.groundID = groundID;
            currentTile.oxygenAmount = oxygenAmount;
            currentTile.carbonDioxideAmount = carbonDioxideAmount;
            currentTile.temp = temp;
            currentTile.lightLevel = lightLevel;
            currentTile.humidity = humidity;
            currentTile.fertility = fertility;
        }
        else
        {
            Debug.Log("SetTile Out of Bounds: " + pos.x + ", " + pos.y);
        }
    }

    public void SetGroundID(Vector2Int pos, ID groundID)
    {
        if (IsInGridBounds(pos))
        {
            Tile currentTile = GetTile(pos);
            currentTile.groundID = groundID;
        }
    }

    public void SetAirAmount(Vector2Int pos, int oxygenAmount, int carbonDioxideAmount)
    {
        if (IsInGridBounds(pos))
        {
            Tile currentTile = GetTile(pos);
            currentTile.oxygenAmount = oxygenAmount;
            currentTile.carbonDioxideAmount = carbonDioxideAmount;
        }
    }

    public void SetTemp(Vector2Int pos, int temp)
    {
        if (IsInGridBounds(pos))
        {
            Tile currentTile = GetTile(pos);
            currentTile.temp = temp;
        }
    }

    public void SetLightLevel(Vector2Int pos, int lightLevel)
    {
        if (IsInGridBounds(pos))
        {
            Tile currentTile = GetTile(pos);
            currentTile.lightLevel = lightLevel;
        }
    }

    public void SetHumidity(Vector2Int pos, int humidity)
    {
        if (IsInGridBounds(pos))
        {
            Tile currentTile = GetTile(pos);
            currentTile.humidity = humidity;
        }
    }

    public void SetFertility(Vector2Int pos, int fertility)
    {
        if (IsInGridBounds(pos))
        {
            Tile currentTile = GetTile(pos);
            currentTile.fertility = fertility;
        }
    }

    //public void SetTiles(Vector2Int pos1, Vector2Int pos2, ID groundID, ID airID, int temp, int lightLevel)
    //{
    //    if (IsInGridBounds(pos1) && IsInGridBounds(pos2))
    //    {
    //        for (int y = pos1.y; y < pos2.y; y++)
    //        {
    //            for (int x = pos1.x; x < pos2.x; x++)
    //            {
    //                SetTile(new Vector2Int(x, y), groundID, airID, temp, lightLevel);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("SetTiles Out of Bounds: " + pos1.x + ", " + pos1.y + " & " + pos2.x + ", " + pos2.y);
    //    }
    //}

    public bool IsInGridBounds(Vector2Int pos)
    {
        if (pos.x >= 0 && pos.x <= Width - 1 && pos.y >= 0 && pos.y <= Height - 1)
        {
            return true;
        }
        return false;
    }

    //-----------------------------------------------------------

    private void InitializeGrid()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                gridArray[x, y] = new Tile(ID.dirt, 0, 10, new Vector2Int(x, y), 20000, 0, 10, 10);
            }
        }
    }

    private void PrintGridSize()
    {
        if (gridArray.Length < 1000000)
        {
            Debug.Log(gameObject.name + " size: " + gridArray.Length / 1000 + "K");
        }
        else
        {
            Debug.Log(gameObject.name + " size: " + gridArray.Length / 1000000 + "M");
        }
    }
}