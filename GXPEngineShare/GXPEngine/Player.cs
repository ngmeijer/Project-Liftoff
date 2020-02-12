using System;
using GXPEngine;
using GXPEngine.Core;

public class Player : Sprite
{
    #region Variables

    private float _moveSpeed = 5f;
    private float _jumpForce = 20f;
    private float _fallMultiplier = 25;
    private bool _isJumping = false;

    private Platform _platform;
    private bool _standingOnPlatform;
    private bool _stillStandingOnPlatform;
    private int _offset = 64;

    private CameraFollow _cameraFollow;
    private StartPlatform _startPlatform;
    private bool _standingOnStart;
    private float speedY;
    private bool playerCanJump;
    private bool _stillStandingOnStart;
    private const float spawnPointX = 100;
    private const float spawnPointY = 100;

    private int coinPoint = 1;
    private int scoreCount = 0;
    public int GetScoreCount()
    {
        return scoreCount;
    }

    private int lifeCount = 3;
    public int GetLifeCount()
    {
        return lifeCount;
    }

    #endregion

    #region Constructor & Update

    public Player() : base("PlayerSprite.png")
	{
        x = spawnPointX;
        y = spawnPointY;

        _cameraFollow = new CameraFollow(this);
        AddChild(_cameraFollow);
	}

    private void Update()
    {
        MovePlayer();
        PlayerJump();
        CheckForPlatformCollision();
        CheckForScreenCollision();
    }

    #endregion

    #region Functions

    private void MovePlayer()
    {
        //These input conditions are temporary, of course! Will be replaced by the actual controller.
        if (Input.GetKey(Key.A))
        {
            Translate(-_moveSpeed, 0);
        }

        if (Input.GetKey(Key.D))
        {
            Translate(_moveSpeed, 0);
        }
    }

    private void PlayerJump()
    {
        y = y + speedY;

        if (speedY <= _fallMultiplier)
        {
            speedY = speedY + 1;
        }

        if (Input.GetKeyDown(Key.SPACE) && playerCanJump)
        {
            speedY = -_jumpForce;
            _isJumping = true;
        }
    }

    private void CheckForScreenCollision()
    {
        if(x >= game.width)
        {
            RespawnPlayer();
        }

        if (x <= 0)
        {
            RespawnPlayer();
        }

        if(y > game.height)
        {
            RespawnPlayer();
        }
    }

    private void OnCollision(GameObject hitInfo)
    {
        if(hitInfo is Platform)
        {
            _platform = hitInfo as Platform;
            _standingOnPlatform = true;
            _isJumping = false;
            y = _platform.y - _offset;
        }

        if(hitInfo is StartPlatform)
        {
            _startPlatform = hitInfo as StartPlatform;
            _standingOnStart = true;
            y = _startPlatform.y - _offset;
        }

        if(hitInfo is Coin)
        {
            scoreCount += coinPoint;
        }
    }

    private void CheckForPlatformCollision()
    {
        if (_standingOnPlatform)
        {
            _stillStandingOnPlatform = HitTest(_platform);

            if (_stillStandingOnPlatform)
            {
                playerCanJump = true;
            }
            else if (!_stillStandingOnPlatform)
            {
                _standingOnPlatform = false;
                playerCanJump = false;
            }
        }

        if (_standingOnStart)
        {
            _stillStandingOnStart = HitTest(_startPlatform);

            if (_stillStandingOnStart)
            {
                playerCanJump = true;
            } else if (!_stillStandingOnStart)
            {
                _standingOnStart = false;
                playerCanJump = false;
            }
        }
    }

    private void RespawnPlayer()
    {
        x = spawnPointX;
        y = spawnPointY;

        lifeCount -= 1;

        if(lifeCount <= 0)
        {
            Destroy();
        }
    }

    #endregion
}
