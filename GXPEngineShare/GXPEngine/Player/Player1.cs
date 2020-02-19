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

    private int frameCount = 1;

    private bool _standingOnPlatform;
    private bool playerCanJump;
    private bool _stillStandingOnPlatform;
    private int _offset = 64;

    public StartPlatform _startPlatform { get; set; }
    private NormalPlatform _normalPlatform;
    private FallingPlatform _fallingPlatform;

    private Sprite _collider;

    private Whip whipSprite;

    private Level levelScript;
    private HUD hudScript;

    private bool _standingOnStart;
    private float speedY;
    private const float spawnPointX = 100;
    private const float spawnPointY = 100;

    private int pickupPoints = 100;
    private int pickupScore;
    private int scoreAhead;
    public int pickupsCollected { get; set; }

    private bool playerHasDied;

    public int scoreCount { get; private set; }
    public int lifeCount { get; private set; }

    private bool _playerIsMoving;
    private float _animationTimer;
    private float _animationSpeed;
    public int whipUsedCount { get; private set; }

    #endregion

    #region Constructor & Update

    public Player1(int xPos, int yPos, HUD hud, Level level) : base("Spritesheet_Jones.png", 4, 3)
    {
        levelScript = level;
        hudScript = hud;

        scale = 0.65f;
        SetOrigin(this.x / 2, this.y + 65);

        _collider = new Sprite("Collider.png", true, true);
        AddChild(_collider);

        whipSprite = new Whip();
        AddChild(whipSprite);
        whipSprite.visible = false;

        lifeCount = 3;
        _animationSpeed = 150f;

        _collider.x = xPos - 85;
        _collider.y = yPos - 100;

        x = _collider.x;
        y = _collider.y;
    }

    private void Update()
    {
        MovePlayer();
        PlayerJump();
        UseWhip();
        CheckCollisions();
        CheckForScreenCollision();
        TrackScore();
    }

    #endregion

    #region Functions
    private void HandleIdleAnimation()
    {
        if (!_playerIsMoving)
        {
            _animationTimer += Time.deltaTime;
            int frame = (int)(_animationTimer / 350f) % 2;

            SetFrame(frame);
        }
    }

    private void HandleRunAnimation()
    {
        if (_playerIsMoving)
        {
            _animationTimer += Time.deltaTime;
            int frame = (int)(_animationTimer / _animationSpeed) % 4 + 9;

            SetFrame(frame);
        }
    }

    private void HandleJumpAnimation()
    {
        _animationTimer += Time.deltaTime;
        int frame = (int)(_animationTimer / 100f) % 1 + 4;

        SetFrame(4);
    }

    private void TrackScore()
    {
        scoreCount = Time.time / 400 + pickupScore + scoreAhead;

        if (levelScript._player2 != null)
        {
            if (x > levelScript._player2.x + 100)
            {
                scoreAhead += 1;
            }
        }
    }

    private void MovePlayer()
    {
        if (Input.GetKey(Key.A))
        {
            _playerIsMoving = true;
            HandleRunAnimation();
            Translate(-_moveSpeed, 0);
        }
        else if (Input.GetKey(Key.D))
        {
            _playerIsMoving = true;
            HandleRunAnimation();
            Translate(_moveSpeed, 0);
        }
        else
        {
            _playerIsMoving = false;
            HandleIdleAnimation();
        }
    }

    private void PlayerJump()
    {
        y = y + speedY;

        if (speedY <= _fallMultiplier)
        {
            speedY = speedY + 1;
            HandleJumpAnimation();
        }

        if (Input.GetKey(Key.SPACE) && (jumpCount < 2))
        {
            jumpCount += 1;
            speedY = -_jumpForce;
            HandleJumpAnimation();
        }
    }

    private void UseWhip()
    {
        if (levelScript.hud._playerCanUseWhip)
        {
            if (Input.GetMouseButtonDown(0))
            {
                whipSprite.visible = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                whipSprite.visible = false;
            }
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

    private void CheckCollisions()
    {
        foreach (GameObject g in _collider.GetCollisions())
        {
            if (g is StartPlatform)
            {
                jumpCount = 0;
                _startPlatform = g as StartPlatform;
                y = _startPlatform.y - 70;
            }

            if (g is NormalPlatform)
            {
                _normalPlatform = g as NormalPlatform;
                jumpCount = 0;
                x = _normalPlatform.x;
                y = _normalPlatform.y - 70;
            }

            if (g is FallingPlatform)
            {
                _fallingPlatform = g as FallingPlatform;
                jumpCount = 0;
                x = _fallingPlatform.x + 55;
                y = _fallingPlatform.y - 70;
                _fallingPlatform.handleCrumbleAnimation();
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
