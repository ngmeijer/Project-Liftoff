using System;
using System.Collections;
using System.Collections.Generic;
using GXPEngine;

public class MovingPlatform : Sprite
{
    #region Variables

    //Script references
    private Player1 _player1;
    Level level;
    Menu menu;

    //Floats
    private float _moveSpeedX;
    private float _moveSpeedY;
    private float weight = 1;
    public float offsetX;
    public float offsetY;

    //Integers
    private int yDir = 1;

    //Bools
    private bool playerOnPlatform;

    #endregion

    public MovingPlatform(Level levelScript, Menu menuScript) : base("MovingPlatform.png", true, true)
	{
        level = levelScript;
        menu = menuScript;

        _moveSpeedX = 1f;
        _moveSpeedY = 1f;

        SetScaleXY(0.3f, 0.3f);

        offsetX = Utils.Random(0, 500);
        offsetY = Utils.Random(0, 500);
    }

	private void Update()
	{
        InversePlatforms();
        HandlePlayerWeight();
        RespawnPlatforms();
	}

    private void HandlePlayerWeight()
    {
        x -= _moveSpeedX;

        if (!playerOnPlatform)
        {
            //y += _moveSpeedY * yDir;
        }
        //else if(playerOnPlatform)
        //{
        //    y += weight;
        //    if (weight <= 5)
        //    {
        //        weight *= 1.02f;
        //    }
        //}
    }

	private void InversePlatforms()
	{
		if(y <= 150)
		{
            yDir *= -1;
		}
		
		if(y >= game.height - 100)
		{
            yDir *= -1;
		}
	}

    public void SetSpawnPosition(float xPos, float yPos)
    {
        x = xPos;
        y = yPos;
    }

    private void RespawnPlatforms()
    {
        if(x <= -64)
        {
            x = game.width + 128;
            y = Utils.Random(50, 950);
        }
    }
}
