using System;
using System.Collections;
using System.Collections.Generic;
using GXPEngine;

public class FakePlatform : Sprite
{
	#region Variables

    private float _moveSpeedX;
	private float _moveSpeedY = 1;
    private int yDir = 1;

    private Timer _timer;
    private int randomChance;

    #endregion

    public FakePlatform() : base("Platform.png", true, true)
	{
        randomChance = Utils.Random(0, 4);
        _moveSpeedX = Utils.Random(1, 3);
        _moveSpeedY = Utils.Random(1, 3);
		scaleY = 0.2f;
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

    private void RespawnPlatforms()
    {
        if(x <= -64)
        {
            x = game.width + 128;
            y = Utils.Random(50, 950);
        }
    }

    //private void OnCollision(GameObject hitInfo)
    //{
    //    if((hitInfo is Player1) || (hitInfo is Player2))
    //    {
    //        LateDestroy();
    //    }
    //}
}
