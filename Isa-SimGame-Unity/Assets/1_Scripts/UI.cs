using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI : BaseClass
{
    public Text FrameRate;
    public Text AverageFrameRate;
    public int FrameAmountForAverageFramerate;
    public Text LowestFrame;
    public Text MouseGroundID;
    public Text MouseOxygenAmount;
    public Text MouseCarbonDioxideAmount;
    public Text MouseTemp;
    public Text MouseLight;

    private int averageFPS;
    private int lowestFrame;
    private float[] frames;
    private int frameCounter;

    private MainGrid mainGrid;
    private InputManager inputManager;

    public override void OnAwake()
    {
        mainGrid = FindObjectOfType<MainGrid>();
        inputManager = FindObjectOfType<InputManager>();
    }

    public override void OnStart()
    {
        frames = new float[FrameAmountForAverageFramerate];
    }

    public override void OnUpdate()
    {
        //Fps
        if (frameCounter != FrameAmountForAverageFramerate)
        {
            frames[frameCounter] = 1.0f / Time.deltaTime;
            frameCounter += 1;
        }
        else
        {
            averageFPS = (int)frames.Sum() / frames.Length;
            lowestFrame = (int)frames.Min();
            AverageFrameRate.text = "AverageFPS: " + averageFPS.ToString();
            LowestFrame.text = "LowestFrame: " + lowestFrame.ToString();
            frameCounter = 0;
        }
        FrameRate.text = "FPS: " + (1.0f / Time.deltaTime).ToString();

        //Grid
        if (mainGrid.IsInGridBounds(inputManager.mousePosGrid))
        {
            Tile currentTile = mainGrid.GetTile(inputManager.mousePosGrid);
            MouseGroundID.text = currentTile.groundID.ToString() + " : GroundID";
            MouseOxygenAmount.text = currentTile.oxygenAmount.ToString() + " : OxygenAmount";
            MouseCarbonDioxideAmount.text = currentTile.carbonDioxideAmount.ToString() + " : CarbonDioxideAmount";
            string temp = currentTile.temp.ToString().PadLeft(6);
            MouseTemp.text = temp.Insert(temp.Length - 3, ".") + " : Temp";
            MouseLight.text = currentTile.lightLevel + " : LightLevel";
        }
    }
}