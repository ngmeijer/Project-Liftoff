using System;
using System.Collections.Generic;
using GXPEngine;
using GXPEngine.Core;

public class Player1 : AnimationSprite
{
    #region Variables

    private float _moveSpeed = 6f;
    private float _jumpForce = 18f;
    private float _gravity;
    private float _defaultGravity = 7.5f;
    private float _whipGravity = 0f;
    private bool _isJumping = false;
    private int jumpCount = 0;

    public StartPlatform _startPlatform { get; set; }
    public NormalPlatform _normalPlatform { get; private set; }
    private MovingPlatform _movingPlatform;
    private CrumblingPlatform _crumblingPlatform;

    private Sprite _collider1;
    private Spears _spears;

    public JonesWhip whipSprite { get; private set; }

    private Level levelScript;
    private HUD hudScript;

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
    private bool usingWhip;

    public bool flyToBorder { get; set; }

    public int whipUsedCount { get; private set; }

    #endregion

    #region Constructor & Update

    public Player1(int xPos, int yPos, HUD hud, Level level) : base("Spritesheet_Jones.png", 4, 3)
    {
        levelScript = level;
        hudScript = hud;

        scale = 0.65f;
        SetOrigin(x / 2, y + 65);

        _collider1 = new Sprite("TestPlayerCollider.png", true, true);
        AddChild(_collider1);

        whipSprite = new JonesWhip(level);
        AddChild(whipSprite);
        whipSprite.visible = false;

        lifeCount = 3;
        _animationSpeed = 150f;

        _collider1.x = xPos - 85;
        _collider1.y = yPos - 110;

        x = _collider1.x;
        y = _collider1.y;

        _gravity = _defaultGravity;
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
        int frame = (int)(_animationTimer / 1000f) % 4 + 4;

        SetFrame(frame);
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
            scaleX = -0.65f;
            _playerIsMoving = true;
            HandleRunAnimation();
            Translate(-_moveSpeed, 0);
        }
        else if (Input.GetKey(Key.D))
        {
            //Consider taking out scaleX since it causes a bit of buggy movement. Rotates around x = 0 instead of pivot point. Preferably stay at same position.
            scaleX = 0.65f;
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

        if (speedY <= _gravity)
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
        if (flyToBorder)
        {
            x += 20;
        }

        float tempPosY = y;

        _whipGravity = _gravity;

        if (levelScript.hud._playerCanUseWhip)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _gravity = _whipGravity;
                usingWhip = true; 
                whipSprite.visible = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _gravity = _defaultGravity;
                usingWhip = false;
                whipSprite.visible = false;
                pickupsCollected = 0;
            }
        }
    }

    private void CheckForScreenCollision()
    {
        if (x >= game.width + 50)
        {
            playerHasDied = true;
            RespawnPlayer1();
        }

        if (x <= -50)
        {
            playerHasDied = true;
            RespawnPlayer1();
        }

        if (y > game.height + 50)
        {
            playerHasDied = true;
            RespawnPlayer1();
        }
    }

    private void CheckCollisions()
    {
        if (!usingWhip)
        {
            foreach (GameObject g in _collider1.GetCollisions())
            {
                if (g is StartPlatform)
                {
                    _startPlatform = g as StartPlatform;
                    jumpCount = 0;
                    y = _startPlatform.y - 55;
                }

                if (g is NormalPlatform)
                {
                    _normalPlatform = g as NormalPlatform;
                    jumpCount = 0;
                    y = _normalPlatform.y - 55;
                }

                if (g is MovingPlatform)
                {
                    _movingPlatform = g as MovingPlatform;
                    jumpCount = 0;
                    y = _movingPlatform.y - 65;
                }

                if (g is CrumblingPlatform)
                {
                    _crumblingPlatform = g as CrumblingPlatform;
                    if (_crumblingPlatform.visible)
                    {
                        jumpCount = 0;
                        y = _crumblingPlatform.y - 60;
                        _crumblingPlatform.playerOnPlatform = true;
                        _crumblingPlatform.handleCrumbleAnimation();
                    }
                }

                if (g is Spears)
                {
                    _spears = g as Spears;
                    playerHasDied = true;
                    RespawnPlayer1();
                }
            }
        }
    }

    private void RespawnPlayer1()
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