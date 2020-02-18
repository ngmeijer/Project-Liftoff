using System;
using System.Collections;
using System.Collections.Generic;
using GXPEngine;

public class FallingPlatform : AnimationSprite
{
	#region Variables

    private float _moveSpeedX;
	private float _moveSpeedY = 1;
    public float offsetX;
    public float offsetY;
    private int newRandomPosX;
    private int yDir = 1;

    //Decrease to increase speed
    private int _animationSpeed = 50;

    private Player1 _player1;
    private Player2 _player2;
    private int _animationTimer;
    private bool _playerOnPlatform;

    #endregion

    public FallingPlatform() : base("PlatformCrumblingSpritesheet.png", 10, 1)
	{
        scale = 0.5f;
        _moveSpeedX = 2.5f;
        _moveSpeedY = Utils.Random(0, 3);

        offsetX = Utils.Random(-600, 500);
        offsetY = Utils.Random(-600, 300);

        newRandomPosX = Utils.Random(600, 1800);
    }

	private void Update()
	{
        InversePlatforms();
        RespawnPlatforms();
	}

    public void handleCrumbleAnimation()
    {
        _animationTimer += Time.deltaTime;
        int frame = (int)(_animationTimer / _animationSpeed) % 10 + 5;

        SetFrame(frame);

        if(frame >= 10)
        {
            
        }
    }

    public void SetSpawnPosition(float xPos, float yPos)
    {
        x = xPos;
        y = yPos;
    }

    private void InversePlatforms()
	{
		if(y <= 0)
		{
            yDir *= -1;
		}
		
		if(y >= 1080)
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
}
