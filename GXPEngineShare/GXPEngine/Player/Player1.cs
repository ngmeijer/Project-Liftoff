using System;
using GXPEngine;
using GXPEngine.Core;

public class Player1 : Sprite
{
    #region Variables

    private float _moveSpeed = 6f;
    private float _jumpForce = 20f;
    private float _fallMultiplier = 7.5f;
    private bool _isJumping = false;

    private bool _standingOnPlatform;
    private bool _stillStandingOnPlatform;
    private int _offset = 64;

    private StartPlatform _startPlatform;
    private NormalPlatform _normalPlatform;
    private FallingPlatform _fallingPlatform;
    private bool _standingOnStart;
    private float speedY;
    private bool playerCanJump;
    private bool _stillStandingOnStart;
    private const float spawnPointX = 100;
    private const float spawnPointY = 100;

    private int coinPoint = 1;
    private bool playerHasDied;

    public int scoreCount { get; private set; }

    public int lifeCount { get; private set; }

    private readonly int _animationDrawsBetweenFrames;
    private int _step;
    private bool _stillStandingOnFallingPlatform;
    private bool _playerIsMoving;
    private bool playerHasMovedOnPlatform;
    private float _movedDistance;

    #endregion

    #region Constructor & Update

    public Player1(int xPos, int yPos) : base("TestPlayer.png", true, true)
    {
        scale = 0.75f;
        SetOrigin(this.x / 2, this.y + 65);

        lifeCount = 3;

        _step = 0;
        _animationDrawsBetweenFrames = 16;

        x = xPos;
        y = yPos;
    }

    private void Update()
    {
        MovePlayer();
        PlayerJump();
        CheckForPlatformCollision();
        CheckForScreenCollision();
        playAnimation();
    }

    #endregion

    #region Functions
    private void playAnimation()
    {
        _step += 1;

        if (_step > _animationDrawsBetweenFrames)
        {
            //NextFrame();
            _step = 0;
        }
    }

    private void MovePlayer()
    {
        //These input conditions are temporary, of course! Will be replaced by the actual controller.
        if (Input.GetKey(Key.LEFT))
        {
            _playerIsMoving = true;
            Translate(-_moveSpeed, 0);
            if (_standingOnPlatform)
            {
                _movedDistance -= _moveSpeed;
                playerHasMovedOnPlatform = true;
            }
        }
        else if (Input.GetKey(Key.RIGHT))
        {
            _playerIsMoving = true;
            Translate(_moveSpeed, 0);
            if (_standingOnPlatform)
            {
                _movedDistance += _moveSpeed;
                playerHasMovedOnPlatform = true;
            }
        }
        else
        {
            _playerIsMoving = false;
        }
    }

    private void PlayerJump()
    {
        y = y + speedY;

        if (speedY <= _fallMultiplier)
        {
            speedY = speedY + 1;
        }

        if (Input.GetKeyDown(Key.UP))
        {
            speedY = -_jumpForce;
            _isJumping = true;
        }
    }

    private void CheckForScreenCollision()
    {
        if (x >= game.width)
        {
            playerHasDied = true;
            RespawnPlayer();
        }

        if (x <= 0)
        {
            playerHasDied = true;
            RespawnPlayer();
        }

        if (y > game.height)
        {
            playerHasDied = true;
            RespawnPlayer();
        }
    }

    private void OnCollision(GameObject other)
    {
        if (other is NormalPlatform)
        {
            _normalPlatform = other as NormalPlatform;
            if (!_playerIsMoving)
            {
                x = _normalPlatform.x;
                //if (playerHasMovedOnPlatform)
                //{
                //    x = _normalPlatform.x + _movedDistance;
                //}
            }
            y = _normalPlatform.y - _offset;
        }

        if (other is FallingPlatform)
        {
            _fallingPlatform = other as FallingPlatform;
            if (!_playerIsMoving)
            {
                x = _fallingPlatform.x + 50;
                if (playerHasMovedOnPlatform)
                {
                    x = _fallingPlatform.x + _movedDistance;
                }
            }
            y = _fallingPlatform.y - _offset;
        }

        if (other is StartPlatform)
        {
            _startPlatform = other as StartPlatform;
            _standingOnStart = true;
            y = _startPlatform.y - _offset;
        }

        if (other is Coin)
        {
            scoreCount += coinPoint;
        }
    }

    private void CheckForPlatformCollision()
    {
        if (_standingOnPlatform)
        {
            _stillStandingOnPlatform = HitTest(_normalPlatform);

            if (_stillStandingOnPlatform)
            {
                playerCanJump = true;
                if (!_isJumping)
                {
                    x = _normalPlatform.x;
                    y = _normalPlatform.y;
                }
            }
            else if (!_stillStandingOnPlatform)
            {
                _standingOnPlatform = false;
                playerCanJump = false;
            }
        }

        if (_standingOnPlatform)
        {
            _stillStandingOnFallingPlatform = HitTest(_fallingPlatform);

            if (_stillStandingOnPlatform)
            {
                playerCanJump = true;
                if (!_isJumping)
                {
                    x = _fallingPlatform.x;
                    y = _fallingPlatform.y;
                }
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
            }
            else if (!_stillStandingOnStart)
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


        if (playerHasDied)
        {
            lifeCount -= 1;
            playerHasDied = false;
        }

        if (lifeCount <= 0)
        {
            Destroy();
        }
    }

    #endregion
}
