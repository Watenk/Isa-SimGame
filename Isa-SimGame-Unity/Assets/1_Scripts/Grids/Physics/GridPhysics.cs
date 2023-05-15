using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class GridPhysics : BaseClass
{
    public TileGrid physicsGrid; //Grid that should have physics

    //PhysicsSettings
    //Light
    public int time;
    public int dayCounter;
    private int dayDuration = 1200; //How many updates 1 day takes
    private int lightAmount = 75; // percentage of day that has light
    private int minSunLight = 40; // Min amount of light the sun can emit
    private int maxSunLight = 90; // Max amount of light the sun can emit
    private int currentSunLight;
    private int currentDayLightLevel;
    private int dayStart;
    private int dayEnd;

    //Temp
    private Dictionary<ID, int> thermalConductivity = new Dictionary<ID, int>()
    {
        { ID.dirt, 10 },
        { ID.grass, 10 },
        { ID.water, 5 },
        { ID.ice, 5 },
        { ID.stone, 2 },
        { ID.lava, 20 },
        { ID.carbonDioxite, 30 },
        { ID.oxygen, 10 },
    };

    private Dictionary<ID, int> minTemp = new Dictionary<ID, int>()
    {
        { ID.water, -1000 },
        { ID.lava, 1000000 },
        { ID.carbonDioxite, -200000 },
        { ID.oxygen, -200000 },
    };

    private Dictionary<ID, ID> ifMinTemp = new Dictionary<ID, ID>()
    {
        { ID.water, ID.ice },
        { ID.lava, ID.stone },
        { ID.carbonDioxite, ID.carbonDioxite }, //Need to implement
        { ID.oxygen, ID.oxygen }, //Need to implement
    };

    private Dictionary<ID, int> maxTemp = new Dictionary<ID, int>()
    {
        { ID.dirt, 1000000 },
        { ID.grass, 50000 },
        { ID.water, 100000 },
        { ID.ice, 1000 },
        { ID.stone, 5000000 },
    };

    private Dictionary<ID, ID> ifMaxTemp = new Dictionary<ID, ID>()
    {
        { ID.dirt, ID.lava },
        { ID.grass, ID.dirt },
        { ID.water, ID.steam },
        { ID.ice, ID.water },
        { ID.stone, ID.lava },
    };

    //Grass Specific
    private int grassSpreadChance = 40;
    private int grassMinCarbonAmount = 5;
    private int grassMinLightLevel = 50;
    private int grassMinHumidity = 5;
    private int grassMinFertility = 5;

    public override void OnStart()
    {
        dayStart = dayDuration - (dayDuration * lightAmount / 100);
        dayEnd = dayDuration - dayStart;
        currentDayLightLevel = Random.Range(minSunLight, maxSunLight);
    }

    public override void OnUPS()
    {
        for (int x = 0; x < physicsGrid.Width; x++)
        {
            for (int y = 0; y < physicsGrid.Height; y++)
            {
                //Get tiles
                Tile currentTile = physicsGrid.GetTile(new Vector2Int(x, y));
                Tile upTile = physicsGrid.GetTile(new Vector2Int(x, y - 1));
                Tile rightTile = physicsGrid.GetTile(new Vector2Int(x + 1, y));
                Tile downTile = physicsGrid.GetTile(new Vector2Int(x, y + 1));
                Tile leftTile = physicsGrid.GetTile(new Vector2Int(x - 1, y));

                //Physics
                TempPhysics(currentTile, upTile, rightTile, downTile, leftTile);
                lightPhysics(currentTile);
                GrassPhysics(currentTile, upTile, rightTile, downTile, leftTile);
                AirPhysics();
            }
        }

        UpdateTime();
        UpdateSunLight();
    }

    private void AirPhysics()
    {

    }

    private void UpdateTime()
    {
        time++;

        if (time >= dayDuration)
        {
            time = 0;
            currentDayLightLevel = Random.Range(minSunLight, maxSunLight);
            dayCounter++;
        }
    }

    private void UpdateSunLight()
    {
        if (time >= dayStart && time <= dayEnd) //day
        {
            currentSunLight = currentDayLightLevel;
        }
        else //night
        {
            currentSunLight = 0;
        }
    }

    private void lightPhysics(Tile currentTile)
    {
        physicsGrid.SetLightLevel(currentTile.pos, currentSunLight);
    }

    private void GrassPhysics(Tile currentTile, Tile upTile, Tile rightTile, Tile downTile, Tile leftTile)
    {

        if (currentTile.groundID == ID.grass)
        {
            int randomChance = Random.Range(1, 100 + (100 - currentTile.lightLevel));

            //Spawn Grass
            if (randomChance <= grassSpreadChance)
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
        }
    }

    private void CalcGrass(Tile targetTile)
    {
        if (targetTile.groundID == ID.dirt && targetTile.carbonDioxideAmount >= grassMinCarbonAmount)
        {
            minTemp.TryGetValue(ID.grass, out int grassMinTemp);
            maxTemp.TryGetValue(ID.grass, out int grassMaxTemp);
            if (targetTile.temp >= grassMinTemp && targetTile.temp <= grassMaxTemp && targetTile.lightLevel >= grassMinLightLevel && targetTile.humidity >= grassMinHumidity && targetTile.fertility >= grassMinFertility)
            {
                thermalConductivity.TryGetValue(targetTile.groundID, out int targetThermalConductivity);
                physicsGrid.SetGroundID(targetTile.pos, ID.grass, targetThermalConductivity);
                physicsGrid.SetAirAmount(targetTile.pos, targetTile.oxygenAmount += 1, targetTile.carbonDioxideAmount -= 1);
                physicsGrid.SetHumidity(targetTile.pos, targetTile.humidity -= 1);
                physicsGrid.SetFertility(targetTile.pos, targetTile.fertility -= 1);
                physicsGrid.SetTemp(targetTile.pos, targetTile.temp += Random.Range(50, 750));
            }
        }
    }

    private void TempPhysics(Tile currentTile, Tile upTile, Tile rightTile, Tile downTile, Tile leftTile)
    {
        int randomSide = Random.Range(0, 99);

        if (randomSide <= 24 && upTile != null)
        {
            CalcTemp(currentTile, upTile);
        }

        if (randomSide >= 25 && randomSide <= 49 && rightTile != null)
        {
            CalcTemp(currentTile, rightTile);
        }

        if (randomSide >= 50 && randomSide <= 74 && downTile != null)
        {
            CalcTemp(currentTile, downTile);
        }

        if (randomSide >= 75 && leftTile != null)
        {
            CalcTemp(currentTile, leftTile);
        }

        //Check if MinTemp
        if (minTemp.ContainsKey(currentTile.groundID))
        {
            minTemp.TryGetValue(currentTile.groundID, out int currentMinTemp);
            if (currentTile.temp <= currentMinTemp)
            {
                //Grass dies
                if (currentTile.groundID == ID.grass)
                {
                    physicsGrid.SetAirAmount(currentTile.pos, currentTile.oxygenAmount, currentTile.carbonDioxideAmount += 1);
                    physicsGrid.SetFertility(currentTile.pos, currentTile.fertility += 1);
                }

                //new values
                ifMinTemp.TryGetValue(currentTile.groundID, out ID newTile);
                thermalConductivity.TryGetValue(newTile, out int newThermalConductivity);

                physicsGrid.SetGroundID(currentTile.pos, newTile, newThermalConductivity);
            }
        }

        //Check if MaxTemp
        if (maxTemp.ContainsKey(currentTile.groundID))
        {
            maxTemp.TryGetValue(currentTile.groundID, out int currentMaxTemp);
            if (currentTile.temp >= currentMaxTemp)
            {
                //new values
                ifMaxTemp.TryGetValue(currentTile.groundID, out ID newTile);
                thermalConductivity.TryGetValue(newTile, out int newThermalConductivity);

                physicsGrid.SetGroundID(currentTile.pos, newTile, newThermalConductivity);
            }
        }
    }

    private void CalcTemp(Tile currentTile, Tile targetTile)
    {
        float tempDifference = Mathf.Abs(currentTile.temp - targetTile.temp) / 1000;

        if (tempDifference >= 0.1f)
        {
            if (currentTile.temp > targetTile.temp)
            {
                if (currentTile.thermalConductivity > targetTile.thermalConductivity)
                {
                    currentTile.temp -= targetTile.thermalConductivity + (int)tempDifference;
                    targetTile.temp += targetTile.thermalConductivity + (int)tempDifference;
                }
                else
                {
                    currentTile.temp -= currentTile.thermalConductivity + (int)tempDifference;
                    targetTile.temp += currentTile.thermalConductivity + (int)tempDifference;
                }
            }
            else
            {
                if (currentTile.thermalConductivity > targetTile.thermalConductivity)
                {
                    currentTile.temp += targetTile.thermalConductivity + (int)tempDifference;
                    targetTile.temp -= targetTile.thermalConductivity + (int)tempDifference;
                }
                else
                {
                    currentTile.temp += currentTile.thermalConductivity + (int)tempDifference;
                    targetTile.temp -= currentTile.thermalConductivity + (int)tempDifference;
                }
            }
        }
    }
}