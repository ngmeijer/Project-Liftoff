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
    public float offsetX;
    public float offsetY;
    private float speedIncrease = 1.5f;

    //Integers
    private int yDir = 1;
    private int speedTimer = 0;
    private int timeIncreaseSpeed = 1000;
    private int maxSpeed = 7;

    //Bools
    private bool playerOnPlatform;

    #endregion

    public MovingPlatform(Level levelScript, Menu menuScript) : base("MovingPlatform.png", true, true)
	{
        level = levelScript;
        menu = menuScript;

        _moveSpeedX = 7f;
        //speedIncrease /= 1000;

        SetScaleXY(0.3f, 0.3f);

        offsetX = Utils.Random(0, 500);
        offsetY = Utils.Random(0, 500);
    }

	private void Update()
	{
        InversePlatforms();
        MovePlatform();
        RespawnPlatforms();
	}

    private void MovePlatform()
    {
        x -= _moveSpeedX;

        if (_moveSpeedX <= maxSpeed)
        {
            speedTimer++;

            if (speedTimer >= timeIncreaseSpeed)
            {
                _moveSpeedX += speedIncrease;
                speedTimer = 0;
            }
        }
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
            //y = Utils.Random(50, 950);
        }
    }
}
