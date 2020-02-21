using System;
using System.Collections;
using System.Collections.Generic;
using GXPEngine;

public class NormalPlatform : Sprite
{
	#region Variables

    private float _moveSpeedX;
	private float _moveSpeedY = 1;
    private int yDir = 1;

    public float offsetX;
    public float offsetY;

    #endregion

    public NormalPlatform() : base("Platform.png", true, true)
	{
        _moveSpeedX = 2.5f;
        _moveSpeedY = Utils.Random(1, 2);
        scale = 0.15f;

        offsetX = Utils.Random(-300, 500);
        offsetY = Utils.Random(-300, 300);
    }

	private void Update()
	{
        y += _moveSpeedY * yDir;

        x -= _moveSpeedX;

        InversePlatforms();
        RespawnPlatforms();
	}

	private void InversePlatforms()
	{
		if(y <= 50)
		{
            yDir *= -1;
		}
		
		if(y >= 900)
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
