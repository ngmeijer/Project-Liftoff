using System;
using System.Collections;
using System.Collections.Generic;
using GXPEngine;

public class FallingPlatform : AnimationSprite
{
	#region Variables

    private float _moveSpeedX;
	private float _moveSpeedY = 1;
    private int yDir = 1;

    private int randomChance;
    private bool _playerOnPlatform;

    private readonly int _animationDrawsBetweenFrames;
    private int _step;

    private Player1 _player1;
    private Player2 _player2;
    private int _animationTimer;

    #endregion

    public FallingPlatform() : base("PlatformCrumblingSpritesheet.png", 10, 1)
	{
        scale = 0.5f;
        x = Utils.Random(400, 1800);
        _moveSpeedX = Utils.Random(1, 2);
        _moveSpeedY = Utils.Random(1, 2);
	}

	private void Update()
	{
        y += _moveSpeedY * yDir;

        x -= _moveSpeedX;

        InversePlatforms();
        RespawnPlatforms();
	}

    private void handleCrumbleAnimation()
    {
        _animationTimer += Time.deltaTime;
        int frame = (int)(_animationTimer / 350f) % 4 + 1;

        SetFrame(frame);

        if(frame >= 5)
        {
            LateDestroy();
        }
    }

    public void SetSpawnPosition(float xPos, float yPos)
    {
        x = xPos;
        y = yPos;
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

    private void OnCollision(GameObject hitInfo)
    {
        if ((hitInfo is Player1) || (hitInfo is Player2))
        {
            _player1 = hitInfo as Player1;
            _player2 = hitInfo as Player2;
            handleCrumbleAnimation();
        }
        else
        {
            _playerOnPlatform = false;
        }
    }
}
