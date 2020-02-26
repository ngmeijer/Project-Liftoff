using GXPEngine;
using System;

public class Player1 : AnimationSprite
{
    #region Variables

    //All class references
    private NormalPlatform _normalPlatform;
    public StartPlatform _startPlatform { get; set; }
    public MovingPlatform _movingPlatform { get; private set; }
    private CrumblingPlatform _crumblingPlatform;
    private Sprite _collider1;
    private Spears _spears;
    private Level levelScript;
    private HUD hudScript;
    public JonesWhip whipSprite { get; private set; }
    private InkaWhip _inkaWhip;

    //Player gameplay properties
    private float _moveSpeed = 6f;
    private float _jumpForce = 18f;
    private float _defaultGravity = 7.5f;
    private float _gravity;
    private float _whipGravity = 0f;
    private int jumpCount = 0;
    private int pickupPoints = 100;
    private int stunnedDuration = 100;
    private float _animationSpeed;

    //Bools
    private bool _isJumping = false;
    public bool flyToBorder { get; set; }
    public bool _stillStandingOnCrumblingPlatform { get; private set; }
    private bool usingWhip;
    private bool playerCanMove = true;
    private bool playerHasDied;
    private bool _playerIsMoving;
    private bool throwingUp;
    private bool swinging;

    //Integers
    public int pickupsCollected { get; set; }
    public bool heartCollected { get; set; }
    public int scoreCount { get; private set; }
    public int lifeCount { get; private set; }
    public int whipUsedCount { get; private set; }
    private int stunnedTimer;
    private int pickupScore;
    private int scoreAhead;

    //Floats
    private float speedY;
    private const float spawnPointX = 100;
    private const float spawnPointY = 200;
    private float _animationTimer;
    private float thrownupSpeed = 50f;

    //SFX
    private Sound _jumpSound;
    private bool gameOver;

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
        whipSprite.x += 25f;
        whipSprite.visible = false;

        _jumpSound = new Sound("JumpSFX.wav", false, true);

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
        TrackScoreAndLives();
    }

    #endregion

    #region Functions

    #region Animations
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
        int frame = (int)(_animationTimer / 1000f) % 3 + 4;

        SetFrame(frame);
    }

    private void HandleStunnedAnimation()
    {
        _animationTimer += Time.deltaTime;
        int frame = (int)(_animationTimer / 750f) % 1 + 7;

        SetFrame(frame);
    }

    #endregion 

    private void TrackScoreAndLives()
    {
        scoreCount = Time.time / 400 + pickupScore + scoreAhead;

        if (levelScript._player2 != null)
        {
            if (x > levelScript._player2.x + 100)
            {
                scoreAhead += 1;
            }
        }

        if (gameOver)
        {
            scoreCount = 0;
        }

        if (heartCollected)
        {
            lifeCount++;
            heartCollected = false;
        }
    }

    private void MovePlayer()
    {
        if (!playerCanMove)
        {
            stunnedTimer++;

            if(stunnedTimer >= stunnedDuration)
            {
                playerCanMove = true;
                stunnedTimer = 0;
            }
        }

        if (playerCanMove)
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
            _jumpSound.Play();
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

        if (levelScript.hud._playerCanSwing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                speedY = -1;
                swinging = true;
                whipSprite.rotation = -55f;
                whipSprite.visible = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _gravity = _defaultGravity;
                swinging = false;
                whipSprite.visible = false;
                pickupsCollected = 0;
                whipUsedCount = 1;
            }
        }

        if (levelScript.hud._playerCanThrowUp)
        {
            if (Input.GetMouseButtonDown(0))
            {
                throwingUp = true;
                whipSprite.rotation = 0;
                whipSprite.visible = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                throwingUp = false;
                whipSprite.visible = false;
                pickupsCollected = 0;
                whipUsedCount = 1;
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
        if ((!swinging) && (!throwingUp))
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

                    if (HitTest(_crumblingPlatform))
                    {
                        _stillStandingOnCrumblingPlatform = true;
                    }
                }

                if (g is Spears)
                {
                    _spears = g as Spears;
                    playerHasDied = true;
                    RespawnPlayer1();
                }

                if(g is InkaWhip)
                {
                    _inkaWhip = g as InkaWhip;
                    if (_inkaWhip.visible)
                    {
                        HandleStunnedAnimation();
                        playerCanMove = false;
                    }
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
            gameOver = true;
            LateDestroy();
        }
    }

    #endregion
}