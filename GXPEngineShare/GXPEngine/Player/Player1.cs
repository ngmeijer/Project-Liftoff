using System;
using GXPEngine;

public class Player1 : AnimationSprite
{
    #region Variables

    private float _moveSpeed = 6f;
    private float _jumpForce = 18f;
    private float _fallMultiplier = 7.5f;
    public bool _isJumping { get; private set; }
    public int jumpCount { get; set; }

    public bool _standingOnPlatform { get; set; }
    public bool _stillStandingOnPlatform { get; set; }
    public int _offset { get; private set; }

    public StartPlatform _startPlatform { get; set; }
    public NormalPlatform _normalPlatform { get; set; }
    public FallingPlatform _fallingPlatform { get; set; }

    private Whip whipSprite;
    private Sprite _collider;

    private Level levelScript;
    private HUD hudScript;

    public bool _standingOnStart { get; set; }
    private float speedY;
    private const float spawnPointX = 100;
    private const float spawnPointY = 100;

    public int pickupPoints { get; private set; }
    public int pickupScore { get; set; }
    private int scoreAhead;
    public int pickupsCollected { get; set; }

    private bool playerHasDied;

    public int scoreCount { get; private set; }
    public int lifeCount { get; private set; }

    public bool _playerIsMoving { get; private set; }
    private float _movedDistance;
    private float _animationTimer;
    public bool _stillStandingOnFallingPlatform { get; set; }
    private bool playerHasMovedOnPlatform;
    private bool _stillStandingOnStart;
    private bool _playerCanUseWhip;
    public int whipUsedCount { get; private set; }

    #endregion

    #region Constructor & Update

    public Player1(int xPos, int yPos, HUD hud, Level level) : base("Spritesheet_Jones.png", 4, 3, 1, true, false)
    {
        levelScript = level;
        hudScript = hud;

        scale = 0.65f;
        SetOrigin(this.x / 2, this.y);

        lifeCount = 3;
        _offset = 64;
        pickupPoints = 100;

        x = xPos;
        y = yPos;

        _collider = new Sprite("Collider.png", true, true);
        AddChild(_collider);

        x = _collider.x;
        y = _collider.y;

        _collider.x += 20;
        _collider.y += 150;

        _startPlatform = level._startPlatform1;

        whipSprite = new Whip();
        AddChild(whipSprite);
        whipSprite.visible = false;
    }

    private void Update()
    {
        MovePlayer();
        PlayerJump();
        UseWhip();
        CheckForCollisions();
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

    private void TrackScore()
    {
        scoreCount = Time.time / 400 + pickupScore + scoreAhead;

        if(x > levelScript._player2.x + 100)
        {
            scoreAhead += 1;
        }
    }
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

    private void CheckForCollisions()
    {
        _collider.GetCollisions();

        if (_collider.HitTest(_startPlatform))
        {
            y = _startPlatform.y - 100;
            jumpCount = 0;
        }

        //if (!_standingOnStart)
        //{
        //    if (other is NormalPlatform)
        //    {
        //        jumpCount = 0;
        //        _normalPlatform = other as NormalPlatform;
        //        if (!player1._playerIsMoving)
        //        {
        //            x = player1._normalPlatform.x;
        //        }
        //        player1.y = player1._normalPlatform.y - player1._offset;
        //    }
        //}

        //if (!player1._standingOnStart)
        //{
        //    if (other is FallingPlatform)
        //    {
        //        player1.jumpCount = 0;
        //        player1._fallingPlatform = other as FallingPlatform;
        //        if (!player1._playerIsMoving)
        //        {
        //            x = player1._fallingPlatform.x + 50;
        //        }
        //        y = player1._fallingPlatform.y - player1._offset;
        //    }
        //}
        //if (other is StartPlatform)
        //{
        //    player1.jumpCount = 0;
        //    player1._startPlatform = other as StartPlatform;
        //    player1._standingOnStart = true;
        //    y = player1._startPlatform.y - player1._offset;
        //}

        //if (other is Pickup)
        //{
        //    player1.pickupsCollected += 1;
        //    player1.pickupScore += player1.pickupPoints;
        //}

    }

    private void CheckForScreenCollision()
    {
        if (_collider.x >= game.width)
        {
            playerHasDied = true;
            RespawnPlayer();
        }

        if (_collider.x <= 0)
        {
            playerHasDied = true;
            RespawnPlayer();
        }

        if (_collider.y > game.height)
        {
            playerHasDied = true;
            RespawnPlayer();
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
