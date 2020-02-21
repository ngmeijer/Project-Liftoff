using System;
using System.Collections;
using System.Collections.Generic;
using GXPEngine;

public class MovingPlatform : Sprite
{
    #region Variables

    private float _moveSpeedX;
    private float[] _moveSpeedY = { 0, 1 };
    private float weight = 1;
    private int yDir = 1;

    public float offsetX;
    public float offsetY;
    private bool playerOnPlatform;

    Level level;
    Menu menu;

    private Player1 _player1;

    #endregion

    public MovingPlatform(Level levelScript, Menu menuScript) : base("Platform.png", true, true)
	{
        level = levelScript;
        menu = menuScript;

        _moveSpeedX = 2.5f;
        scale = 0.2f;

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
            y += _moveSpeedY[1] * yDir;
        }
        else
        {
            y += weight;
            if (weight <= 5)
            {
                weight *= 1.02f;
            }
        }
    }

	private void InversePlatforms()
	{
		if(y <= 150)
		{
            yDir *= -1;
		}
		
		if(y >= game.height - 200)
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
