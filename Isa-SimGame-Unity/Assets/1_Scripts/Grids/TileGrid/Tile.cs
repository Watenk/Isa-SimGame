using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public ID groundID { get; set; }
    public ID airID { get; set; }
    public Vector2Int pos { get; set; }
    public int temp { get; set; }
    public int lightLevel { get; set; }
    public int fertility { get; set; } //Need to implement
    public int humidity { get; set; } //Need to implement

    public Tile(ID groundID, ID airID, Vector2Int pos, int temp, int lightLevel)
    {
        this.groundID = groundID;
        this.airID = airID;
        this.pos = pos;
        this.temp = temp;
        this.lightLevel = lightLevel;
    }
}