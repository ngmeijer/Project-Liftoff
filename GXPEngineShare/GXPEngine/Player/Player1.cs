using System;
using System.Collections.Generic;
using GXPEngine;
using GXPEngine.Core;

public class Player1 : AnimationSprite
{
    #region Variables

    private float _moveSpeed = 6f;
    private float _jumpForce = 18f;
    private float _fallMultiplier = 7.5f;
    private bool _isJumping = false;
    private int jumpCount = 0;

    private bool _standingOnPlatform;
    private bool _stillStandingOnPlatform;
    private int _offset = 64;

    private StartPlatform _startPlatform;
    private NormalPlatform _normalPlatform;
    private FallingPlatform _fallingPlatform;
    private bool _standingOnStart;
    private float speedY;
    private const float spawnPointX = 100;
    private const float spawnPointY = 100;

    private int coinPoint = 100;
    private int coinScore;
    private bool playerHasDied;

    public int scoreCount { get; private set; }

    public int lifeCount { get; private set; }

    private bool _playerIsMoving;
    private float _movedDistance;
    private float _animationTimer;
    private bool _stillStandingOnFallingPlatform;

    #endregion

    #region Constructor & Update

    public Player1(int xPos, int yPos) : base("Spritesheet_Jones.png", 4, 3)
    {
        scale = 0.65f;
        SetOrigin(this.x / 2, this.y + 65);

        lifeCount = 3;

        x = xPos;
        y = yPos;
    }

    private void Update()
    {
        MovePlayer();
        PlayerJump();
        CheckForPlatformCollision();
        CheckForScreenCollision();
        TrackScore();
    }

    #endregion

    #region Functions
    private void HandleIdleAnimation()
    {
        _animationTimer += Time.deltaTime;
        int frame = (int)(_animationTimer / 350f) % 2;

        SetFrame(frame);
    }

    private void HandleRunAnimation()
    {
        _animationTimer += Time.deltaTime;
        int frame = (int)(_animationTimer / 350f) % 3 + 8;

        SetFrame(frame);
    }

    private void HandleJumpAnimation()
    {
        _animationTimer += Time.deltaTime;
        int frame = (int)(_animationTimer / 350f) % 1 + 4;

        SetFrame(frame);
    }

    private void TrackScore() => scoreCount = Time.time / 150 + coinScore;

    private void MovePlayer()
    {
        if (Input.GetKey(Key.A))
        {
            HandleRunAnimation();
            _playerIsMoving = true;
            Translate(-_moveSpeed, 0);
            if (_standingOnPlatform)
            {
                _movedDistance -= _moveSpeed;
            }
        }
        else if (Input.GetKey(Key.D))
        {
            HandleRunAnimation();
            _playerIsMoving = true;
            Translate(_moveSpeed, 0);
            if (_standingOnPlatform)
            {
                _movedDistance += _moveSpeed;
            }
        }
        else
        {
            _playerIsMoving = false;
            if (!_playerIsMoving)
            {
                HandleIdleAnimation();
            }
        }
    }

    private void PlayerJump()
    {
        y = y + speedY;

        if (speedY <= _fallMultiplier)
        {
            speedY = speedY + 1;
        }

        if (Input.GetKeyDown(Key.SPACE) && (jumpCount <= 1))
        {
            jumpCount += 1;
            speedY = -_jumpForce;
            _isJumping = true;
            HandleJumpAnimation();
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
        if (!_standingOnStart)
        {
            if (other is NormalPlatform)
            {
                jumpCount = 0;
                _normalPlatform = other as NormalPlatform;
                if (!_playerIsMoving)
                {
                    x = _normalPlatform.x;
                }
                y = _normalPlatform.y - _offset;
            }
        }

        if (!_standingOnStart)
        {
            if (other is FallingPlatform)
            {
                jumpCount = 0;
                _fallingPlatform = other as FallingPlatform;
                if (!_playerIsMoving)
                {
                    x = _fallingPlatform.x + 50;
                }
                y = _fallingPlatform.y - _offset;
            }
        }
        if (other is StartPlatform)
        {
            jumpCount = 1;
            _startPlatform = other as StartPlatform;
            _standingOnStart = true;
            y = _startPlatform.y - _offset;
        }

        if (other is Pickup)
        {
            coinScore += coinPoint;
        }
    }

    private void CheckForPlatformCollision()
    {
        if (_standingOnPlatform)
        {
            _stillStandingOnPlatform = HitTest(_normalPlatform);

            if (_stillStandingOnPlatform)
            {
                if (!_isJumping)
                {
                    x = _normalPlatform.x;
                    y = _normalPlatform.y;
                }
            }
            else if (!_stillStandingOnPlatform)
            {
                _standingOnPlatform = false;
            }
        }

        if (_standingOnPlatform)
        {
            _stillStandingOnFallingPlatform = HitTest(_fallingPlatform);

            if (_stillStandingOnPlatform)
            {
                if (!_isJumping)
                {
                    x = _fallingPlatform.x;
                    y = _fallingPlatform.y;
                }
            }
            else if (!_stillStandingOnPlatform)
            {
                _standingOnPlatform = false;
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
            LateDestroy();
        }
    }

    #endregion
}
