using System;
using System.Collections;
using System.Collections.Generic;
using GXPEngine;

public class NormalPlatform : Sprite
{
	#region Variables

    private float _moveSpeedX;
	private float _moveSpeedY = 1;
    private float weight = 1;
    private int yDir = 1;

    public float offsetX;
    public float offsetY;
    private bool playerOnPlatform;

    #endregion

    public NormalPlatform() : base("Platform.png", true, true)
	{
        _moveSpeedX = 1.5f;
        _moveSpeedY = Utils.Random(1, 2);
        scale = 0.2f;

        offsetX = Utils.Random(0, 500);
        offsetY = Utils.Random(0, 500);
    }

	private void Update()
	{
        x -= _moveSpeedX;

        InversePlatforms();
        HandlePlayerWeight();
        RespawnPlatforms();
	}

    private void HandlePlayerWeight()
    {
        if (!playerOnPlatform)
        {
            y += _moveSpeedY * yDir;
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
		if(y <= 0)
		{
            yDir *= -1;
		}
		
		if(y >= game.height)
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

    private void OnCollision(GameObject hitInfo)
    {
        if ((hitInfo is Player1) || (hitInfo is Player2))
        {
            playerOnPlatform = true;
        }
    }
}
