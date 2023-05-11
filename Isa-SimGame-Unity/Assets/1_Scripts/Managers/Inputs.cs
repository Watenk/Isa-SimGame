using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inputs : BaseClass
{
    public float ScrollSpeed;
    public int minCamSize;
    public int maxCamSize;

    public int TempPowerAmount;

    public int currentPower;
    //public ID currentElement;

    private Vector2 referenceMousePos;

    //references
    private InputManager inputManager;
    private GameManager gameManager;
    private MainGrid mainGrid;

    public override void OnAwake()
    {
        inputManager = FindObjectOfType<InputManager>();
        gameManager = FindObjectOfType<GameManager>();
        mainGrid = FindObjectOfType<MainGrid>();
    }

    public override void OnUpdate()
    {
        Camera();
        //Overlays();
        Powers();
    }

    private void Camera()
    {
        //Mouse
        if (inputManager.MiddleMouseDown == true)
        {
            referenceMousePos = Input.mousePosition;
            referenceMousePos = UnityEngine.Camera.main.ScreenToWorldPoint(referenceMousePos);
        }

        if (inputManager.MiddleMouse == true)
        {
            //Get mousepos and calc newPos
            Vector2 currentMousePos = Input.mousePosition;
            currentMousePos = UnityEngine.Camera.main.ScreenToWorldPoint(currentMousePos);
            float xDifference = currentMousePos.x - referenceMousePos.x;
            float yDifference = currentMousePos.y - referenceMousePos.y;
            float newXPos = UnityEngine.Camera.main.transform.position.x - xDifference;
            float newYPos = UnityEngine.Camera.main.transform.position.y - yDifference;

            //Set newPos
            Vector3 newPos = new Vector3(newXPos, newYPos, -10);
            UnityEngine.Camera.main.transform.position = newPos;
        }

        //Scroll up
        if (inputManager.ScrollMouseDelta > 0f && UnityEngine.Camera.main.orthographicSize > minCamSize && Input.GetMouseButton(2) == false)
        {
            UnityEngine.Camera.main.orthographicSize -= UnityEngine.Camera.main.orthographicSize * ScrollSpeed * 0.01f;
        }

        //Scroll down
        if (inputManager.ScrollMouseDelta < 0f && UnityEngine.Camera.main.orthographicSize < maxCamSize && Input.GetMouseButton(2) == false)
        {
            UnityEngine.Camera.main.orthographicSize += UnityEngine.Camera.main.orthographicSize * ScrollSpeed * 0.01f;
        }
    }

    private void Powers()
    {
        //UPS
        if (inputManager.space == true)
        {
            if (gameManager.UPS == 20)
            {
                gameManager.UPS = 0;
            }
            else
            {
                gameManager.UPS = 20;
            }
        }

        if (inputManager.tab == true)
        {
            if (currentPower == 0)
            {
                currentPower = 1;
            }
            else
            {
                currentPower = 0;
            }
        }

        if (currentPower == 0)
        {
            //PlacePower();
        }

        if (currentPower == 1)
        {
            //TempPower();
        }
    }

    //private void TempPower()
    //{
    //    if (inputManager.LeftMouse == true)
    //    {
    //        Tile currentTile = mainGrid.GetTile(inputManager.mousePosGrid);
    //        mainGrid.SetTile(currentTile.pos, currentTile.id, currentTile.amount, currentTile.temp + TempPowerAmount);
    //    }

    //    if (inputManager.RightMouse == true)
    //    {
    //        Tile currentTile = mainGrid.GetTile(inputManager.mousePosGrid);
    //        mainGrid.SetTile(currentTile.pos, currentTile.id, currentTile.amount, currentTile.temp - TempPowerAmount);
    //    }
    //}

    //private void PlacePower()
    //{
    //    if (inputManager.one == true)
    //    {
    //        currentElement = ID.dirt;
    //    }
    //    if (inputManager.two == true)
    //    {
    //        currentElement = ID.grass;
    //    }
    //    if (inputManager.three == true)
    //    {
    //        currentElement = ID.water;
    //    }
    //    if (inputManager.four == true)
    //    {
    //        currentElement = ID.stone;
    //    }
    //    if (inputManager.five == true)
    //    {
    //        currentElement = ID.ice;
    //    }
    //    if (inputManager.six == true)
    //    {
    //        currentElement = ID.carbonDioxite;
    //    }
    //    if (inputManager.seven == true)
    //    {
    //        currentElement = ID.oxygen;
    //    }
    //    if (inputManager.eight == true)
    //    {
    //        currentElement = ID.steam;
    //    }

    //    if (inputManager.LeftMouse == true)
    //    {
    //        if (currentElement == ID.steam)
    //        {
    //            mainGrid.SetTile(inputManager.mousePosGrid, currentElement, 9, 1400000);

    //        }
    //        else if (currentElement == ID.ice)
    //        {
    //            mainGrid.SetTile(inputManager.mousePosGrid, currentElement, 9, -10000);

    //        }
    //        else
    //        {
    //            mainGrid.SetTile(inputManager.mousePosGrid, currentElement, 9, 20000);
    //        }
    //    }
    //}

    //private void Overlays()
    //{
    //    //TempOverlay
    //    if (inputManager.F1 == true)
    //    {
    //        if (tempRenderer.activeSelf == false)
    //        {
    //            tempRenderer.SetActive(true);
    //        }
    //        else
    //        {
    //            tempRenderer.SetActive(false);
    //        }
    //    }

    //    //AmountOverlay
    //    if (inputManager.F2 == true)
    //    {
    //        if (amountRenderer.activeSelf == false)
    //        {
    //            amountRenderer.SetActive(true);
    //        }
    //        else
    //        {
    //            amountRenderer.SetActive(false);
    //        }
    //    }
    //}
}