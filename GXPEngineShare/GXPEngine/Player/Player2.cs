using GXPEngine;
using System;


public class Player2 : AnimationSprite
{
    #region Variables

    //All class references
    private NormalPlatform _normalPlatform;
    public StartPlatform _startPlatform { get; set; }
    public MovingPlatform _movingPlatform { get; private set; }
    private CrumblingPlatform _crumblingPlatform;
    private Sprite _collider2;
    private Spears _spears;
    private Level levelScript;
    private HUD hudScript;
    public InkaWhip whipSprite { get; private set; }
    private JonesWhip _jonesWhip;

    //Player gameplay properties
    private float _moveSpeed = 6f;
    private float _jumpForce = 18f;
    private float _defaultGravity = 4f;
    private float _gravity;
    private int jumpCount = 0;
    private int pickupPoints = 100;
    private int stunnedDuration = 100;
    private float _animationSpeed;

    //Bools
    private bool _isJumping = false;
    public bool heartCollected { get; set; }
    public bool flyToBorder { get; set; }
    public bool _stillStandingOnCrumblingPlatform { get; private set; }
    private bool usingWhip;
    private bool playerCanMove = true;
    private bool playerHasDied;
    private bool _playerIsMoving;
    private bool swinging;
    private bool throwingUp;
    private bool playerCanStun;
    private bool playerCanSwing;
    private bool gameOver;

    //Integers
    public int pickupsCollected { get; set; }
    public int scoreCount { get; private set; }
    public int lifeCount { get; private set; }
    public int whipUsedCount { get; private set; }
    private int stunnedTimer;
    private int pickupScore;
    private int scoreAhead;

    //Floats
    private float speedY;
    private const float spawnPointX = 100;
    private const float spawnPointY = 500;
    private float _animationTimer;
    private Sound _jumpSound;
    private Sound _gameOverSound;
    #endregion

    #region Constructor & Update

    public Player2(int xPos, int yPos, HUD hud, Level level) : base("Spritesheet_Inka.png", 4, 3)
    {
        levelScript = level;
        hudScript = hud;

        scale = 0.65f;
        SetOrigin(x / 2, y + 65);

        _collider2 = new Sprite("TestPlayerCollider.png", true, true);
        AddChild(_collider2);
        _collider2.visible = false;

        x = xPos;
        y = yPos;

        //DON'T TOUCH THIS S#!&
        _collider2.x = x - 85;
        _collider2.y = y - 410;
        //Will mess up spawning & collider positions. Idk why it doesn't work the same way as Player1. Literally copied the script,
        // and changed the spawn positions.

        whipSprite = new InkaWhip(level);
        AddChild(whipSprite);
        whipSprite.visible = false;

        _jumpSound = new Sound("JumpSFX.wav", false, true);
        _gameOverSound = new Sound("GameOver.wav", false, false);
        lifeCount = 3;
        _animationSpeed = 150f;

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
        if (levelScript._player1 != null)
        {
            if (levelScript._player1.whipSprite.playerIsStunned)
            {
                HandleStunnedAnimation();
                stunnedTimer++;
                playerCanMove = false;
            }
            else if (!levelScript._player1.whipSprite.playerIsStunned)
            {
                HandleIdleAnimation();
                playerCanMove = true;
            }
        }
        if (!playerCanMove)
        {
            stunnedTimer++;

            if (stunnedTimer >= stunnedDuration)
            {
                playerCanMove = true;
                stunnedTimer = 0;
            }
        }
        if (!swinging)
        {
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
    }

    private void PlayerJump()
    {
        y = y + speedY;

        if (speedY <= _gravity)
        {
            speedY = speedY + 1;
            HandleJumpAnimation();
        }

        if (Input.GetKey(Key.E) && (jumpCount < 2))
        {
            jumpCount += 1;
            speedY = -_jumpForce;
            _jumpSound.Play();
            HandleJumpAnimation();
        }
    }

    private void UseWhip()
    {
        if (pickupsCollected == 0)
        {
            playerCanStun = false;
            playerCanSwing = false;
        }

        if (pickupsCollected == 1)
        {
            playerCanStun = true;
            playerCanSwing = false;
        }

        if (pickupsCollected == 2)
        {
            playerCanStun = false;
            playerCanSwing = true;
        }

        if (playerCanStun)
        {
            if (Input.GetKeyDown(Key.RIGHT_CTRL))
            {
                whipSprite.visible = true;
                whipSprite.rotation = 0f;
            }

            if (Input.GetKeyUp(Key.RIGHT_CTRL))
            {
                whipSprite.visible = false;
                pickupsCollected = 0;
            }
        }

        if (playerCanSwing)
        {
            if (Input.GetKeyDown(Key.RIGHT_CTRL))
            {
                swinging = true;
                whipSprite.scaleX = 3f;
                scaleX = 0.65f;
                whipSprite.visible = true;
                whipSprite.rotation = -55f;
                flyToBorder = true;
            }

            if (Input.GetKeyUp(Key.RIGHT_CTRL))
            {
                swinging = false;
                flyToBorder = false;
                whipSprite.visible = false;
                pickupsCollected = 0;
            }
        }

        if (flyToBorder)
        {
            x += 15;
        }
    }

    private void CheckForScreenCollision()
    {
        if (x >= game.width + 50)
        {
            playerHasDied = true;
            RespawnPlayer2();
        }

        if (x <= -50)
        {
            playerHasDied = true;
            RespawnPlayer2();
        }

        if (y > game.height + 50)
        {
            playerHasDied = true;
            RespawnPlayer2();
        }
    }

    private void CheckCollisions()
    {
        if (!swinging)
        {
            foreach (GameObject g in _collider2.GetCollisions())
            {
                if (g is StartPlatform)
                {
                    _startPlatform = g as StartPlatform;
                    jumpCount = 0;
                    y = _startPlatform.y - 55;
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
                    RespawnPlayer2();
                }

                if (g is JonesWhip)
                {
                    _jonesWhip = g as JonesWhip;
                    Console.WriteLine("stunned");
                    if (_jonesWhip.visible)
                    {
                        HandleStunnedAnimation();
                        playerCanMove = false;
                    }
                }
            }
        }
    }

    private void RespawnPlayer2()
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
            _gameOverSound.Play();
            gameOver = true;
            LateDestroy();
        }
    }
}

        #endregion