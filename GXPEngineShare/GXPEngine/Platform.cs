using System;
using GXPEngine;

public class Platform : Sprite
{
	#region Variables

    private float _moveSpeedX;
	private float _moveSpeedY = 1;
    private float yDir = 1;

    #endregion

    public Platform() : base("Obstacle.png", true, true)
	{
        _moveSpeedX = Utils.Random(1, 4);
        _moveSpeedY = Utils.Random(1, 4);
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
        }
    }
}
