using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

public class GridPhysics : BaseClass
{
    public TileGrid physicsGrid; //Grid that should have physics
    public int GrassSpreadChance;
    public int GrassMinTemp;
    public int GrassMaxTemp;
    public int GrassMinLightLevel;

    private Dictionary<ID, PhysicsID> PhysicsIDs = new Dictionary<ID, PhysicsID>();

    public override void OnStart()
    {
        //PhysicsSettings                             TCo 
        PhysicsIDs.Add(ID.dirt,          new PhysicsID( 30));
        PhysicsIDs.Add(ID.grass,         new PhysicsID( 30));
        PhysicsIDs.Add(ID.water,         new PhysicsID( 10));
        PhysicsIDs.Add(ID.ice,           new PhysicsID( 10));
        PhysicsIDs.Add(ID.stone,         new PhysicsID(  5));
        PhysicsIDs.Add(ID.lava,          new PhysicsID( 10));
    }

    public override void OnUPS()
    {
        for (int x = 0; x < physicsGrid.Width; x++)
        {
            for (int y = 0; y < physicsGrid.Height; y++)
            {
                //Get tiles
                Tile currentTile = physicsGrid.GetTile(new Vector2Int(x, y));
                PhysicsIDs.TryGetValue(currentTile.groundID, out PhysicsID currentTilePhysics);

                Tile upTile = physicsGrid.GetTile(new Vector2Int(x, y - 1));
                Tile rightTile = physicsGrid.GetTile(new Vector2Int(x + 1, y));
                Tile downTile = physicsGrid.GetTile(new Vector2Int(x, y + 1));
                Tile leftTile = physicsGrid.GetTile(new Vector2Int(x - 1, y));

                //physics
                TempPhysics(currentTile, currentTilePhysics, upTile, rightTile, downTile, leftTile);
                lightPhysics(currentTile);
                GrassPhysics(currentTile, upTile, rightTile, downTile, leftTile);
            }
        }
    }

    private void lightPhysics(Tile currentTile)
    {
        physicsGrid.SetTile(currentTile.pos, currentTile.groundID, currentTile.airID, currentTile.temp, 80);
    }

    private void GrassPhysics(Tile currentTile, Tile upTile, Tile rightTile, Tile downTile, Tile leftTile)
    {

        if (currentTile.groundID == ID.grass)
        {
            int randomChance = Random.Range(1, 100);

            //Spawn Grass
            if (randomChance <= GrassSpreadChance)
            {
                int randomSide = Random.Range(0, 99);

                if (randomSide <= 24 && upTile != null)
                {
                    CalcGrass(upTile);
                }

                if (randomSide >= 25 && randomSide <= 49 && rightTile != null)
                {
                    CalcGrass(rightTile);
                }

                if (randomSide >= 50 && randomSide <= 74 && downTile != null)
                {
                    CalcGrass(downTile);
                }

                if (randomSide >= 75 && leftTile != null)
                {
                    CalcGrass(leftTile);
                }
            }

            //Remove Grass
            if (currentTile.temp <= GrassMinTemp || currentTile.temp >= GrassMaxTemp)
            {
                physicsGrid.SetTile(currentTile.pos, ID.dirt, currentTile.airID, currentTile.temp, currentTile.lightLevel);
            }
        }
    }

    private void CalcGrass(Tile targetTile)
    {
        if (targetTile.groundID == ID.dirt && targetTile.airID == ID.carbonDioxite)
        {
            if (targetTile.temp >= GrassMinTemp && targetTile.temp <= GrassMaxTemp && targetTile.lightLevel >= GrassMinLightLevel)
            {
                physicsGrid.SetTile(targetTile.pos, ID.grass, ID.oxygen, targetTile.temp, targetTile.lightLevel);
            }
        }
    }

    private void TempPhysics(Tile currentTile, PhysicsID currentTilePhysics, Tile upTile, Tile rightTile, Tile downTile, Tile leftTile)
    {
        int randomSide = Random.Range(0, 99);

        if (randomSide <= 24 && upTile != null)
        {
            PhysicsIDs.TryGetValue(upTile.groundID, out PhysicsID upTilePhysics);
            CalcTemp(currentTile, currentTilePhysics, upTile, upTilePhysics);
        }

        if (randomSide >= 25 && randomSide <= 49 && rightTile != null)
        {
            PhysicsIDs.TryGetValue(rightTile.groundID, out PhysicsID rightTilePhysics);
            CalcTemp(currentTile, currentTilePhysics, rightTile, rightTilePhysics);
        }

        if (randomSide >= 50 && randomSide <= 74 && downTile != null)
        {
            PhysicsIDs.TryGetValue(downTile.groundID, out PhysicsID downTilePhysics);
            CalcTemp(currentTile, currentTilePhysics, downTile, downTilePhysics);
        }

        if (randomSide >= 75 && leftTile != null)
        {
            PhysicsIDs.TryGetValue(leftTile.groundID, out PhysicsID leftTilePhysics);
            CalcTemp(currentTile, currentTilePhysics, leftTile, leftTilePhysics);
        }

        ////Check if MinTemp
        //if (currentTilePhysics.hasMinTemp)
        //{
        //    if (currentTile.temp <= currentTilePhysics.minTemp)
        //    {
        //        mainGrid.SetTile(currentTile.pos, currentTilePhysics.ifMinTemp, currentTile.amount, currentTile.temp);
        //    }
        //}

        ////Check if MaxTemp
        //if (currentPhysics.hasMaxTemp)
        //{
        //    if (currentTile.temp >= currentPhysics.maxTemp)
        //    {
        //        mainGrid.SetTile(currentTile.pos, currentPhysics.ifMaxTemp, currentTile.amount, currentTile.temp);
        //    }
        //}
    }

    private void CalcTemp(Tile currentTile, PhysicsID currentTilePhysics, Tile targetTile, PhysicsID targetPhysics)
    {
        float tempDifference = Mathf.Abs(currentTile.temp - targetTile.temp) / 1000;

        if (tempDifference >= 0.1f)
        {
            if (currentTile.temp > targetTile.temp)
            {
                if (currentTilePhysics.thermalConductivity > targetPhysics.thermalConductivity)
                {
                    currentTile.temp -= targetPhysics.thermalConductivity + (int)tempDifference;
                    targetTile.temp += targetPhysics.thermalConductivity + (int)tempDifference;
                }
                else
                {
                    currentTile.temp -= currentTilePhysics.thermalConductivity + (int)tempDifference;
                    targetTile.temp += currentTilePhysics.thermalConductivity + (int)tempDifference;
                }
            }
            else
            {
                if (currentTilePhysics.thermalConductivity > targetPhysics.thermalConductivity)
                {
                    currentTile.temp += targetPhysics.thermalConductivity + (int)tempDifference;
                    targetTile.temp -= targetPhysics.thermalConductivity + (int)tempDifference;
                }
                else
                {
                    currentTile.temp += currentTilePhysics.thermalConductivity + (int)tempDifference;
                    targetTile.temp -= currentTilePhysics.thermalConductivity + (int)tempDifference;
                }
            }
        }
    }
}