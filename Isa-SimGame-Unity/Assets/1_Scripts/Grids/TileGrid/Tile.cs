using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public ID groundID { get; set; }
    public int oxygenAmount { get; set; }
    public int carbonDioxideAmount { get; set; }
    public Vector2Int pos { get; set; }
    public int temp { get; set; }
    public int lightLevel { get; set; }
    public int humidity { get; set; } 
    public int fertility { get; set; } 

    public Tile(ID groundID, int oxygenAmount, int carbonDioxideAmount, Vector2Int pos, int temp, int lightLevel, int humidity, int fertility)
    {
        this.groundID = groundID;
        this.oxygenAmount = oxygenAmount;
        this.carbonDioxideAmount = carbonDioxideAmount;
        this.pos = pos;
        this.temp = temp;
        this.lightLevel = lightLevel;
        this.humidity = humidity;
        this.fertility = fertility;
    }
}