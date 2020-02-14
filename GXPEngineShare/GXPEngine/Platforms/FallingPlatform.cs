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
    private int yDir = 1;

    //Decrease to increase speed
    private int _animationSpeed = 200;

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

    private void handleCrumbleAnimation()
    {
        _animationTimer += Time.deltaTime;
        int frame = (int)(_animationTimer / _animationSpeed) % 10 + 5;

        if (_playerOnPlatform)
        {
            SetFrame(frame);

            if (frame >= 7)
            {
                _animationSpeed *= (int)1.4;
                LateDestroy();
            }
        }

        if((frame >= 2) && (!_playerOnPlatform))
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

    private void OnCollision(GameObject hitInfo)
    {
        if ((hitInfo is Player1) || (hitInfo is Player2))
        {
            _player1 = hitInfo as Player1;
            _player2 = hitInfo as Player2;
            _playerOnPlatform = true;
            handleCrumbleAnimation();
        }
        else
        {
            _playerOnPlatform = false;
        }
    }
}
