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

    #endregion

    public FallingPlatform() : base("PlatformCrumblingSpritesheet.png", 10, 1, 1, true, true)
	{
        scale = 0.5f;
        _step = 0;
        _animationDrawsBetweenFrames = 16;

        randomChance = Utils.Random(0, 4);
        _moveSpeedX = Utils.Random(1, 3);
        _moveSpeedY = Utils.Random(1, 3);
	}

	private void Update()
	{
        y += _moveSpeedY * yDir;

        x -= _moveSpeedX;

        InversePlatforms();
        RespawnPlatforms();
	}

    private void playAnimation()
    {
        _step += 1;

        if (_step > _animationDrawsBetweenFrames)
        {
            NextFrame();
            _step = 0;
        }
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
            _playerOnPlatform = true;
        }
        else
        {
            _playerOnPlatform = false;
        }
    }
}
