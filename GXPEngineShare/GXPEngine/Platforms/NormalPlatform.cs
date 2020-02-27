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
    }

	private void Update()
	{
        MovePlatforms();
        InversePlatforms();
        RespawnPlatforms();
	}

    private void MovePlatforms()
    {
        y += _moveSpeedY * yDir;

        x -= _moveSpeedX;
    }

	private void InversePlatforms()
	{
		if(y <= 50)
		{
            yDir *= -1;
		}
		
		if(y >= 800)
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
