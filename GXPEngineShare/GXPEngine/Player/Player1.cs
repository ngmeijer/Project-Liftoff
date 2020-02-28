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
    private float _defaultGravity = 4f;
    private float _gravity;
    private float _whipGravity = 0f;
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
    private bool throwingUp;
    public bool swinging { get; set; }
    private bool gameOver;
    private bool playerCanStun;
    private bool playerCanSwing;

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
    private const float spawnPointY = 200;
    private float _animationTimer;

    //SFX
    private Sound _jumpSound;
    private Sound gameOverSound;

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
        _collider1.visible = false;

        whipSprite = new JonesWhip(level);
        AddChild(whipSprite);
        whipSprite.x += 25f;
        whipSprite.visible = false;

        _jumpSound = new Sound("JumpSFX.wav", false, true);
        gameOverSound = new Sound("GameOver.wav", false, false);

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
        if (levelScript._player2 != null)
        {
            if (levelScript._player2.whipSprite.playerIsStunned)
            {
                HandleStunnedAnimation();
                stunnedTimer++;
                playerCanMove = false;
            }
            else if (!levelScript._player2.whipSprite.playerIsStunned)
            {
                HandleIdleAnimation();
                playerCanMove = true;
            }
        }

        if (stunnedTimer >= stunnedDuration)
        {
            playerCanMove = true;
            stunnedTimer = 0;
        }

        if (!swinging)
        {
            if (playerCanMove)
            {
                if (Input.GetKey(Key.LEFT))
                {
                    scaleX = -0.65f;
                    _playerIsMoving = true;
                    HandleRunAnimation();
                    Translate(-_moveSpeed, 0);
                }
                else if (Input.GetKey(Key.RIGHT))
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

        if (Input.GetKey(Key.UP) && (jumpCount < 2))
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
            if (Input.GetKeyDown(Key.LEFT_CTRL))
            {
                whipSprite.visible = true;
                whipSprite.rotation = 0f;
            }

            if (Input.GetKeyUp(Key.LEFT_CTRL))
            {
                whipSprite.visible = false;
                pickupsCollected = 0;
            }
        }

        if (playerCanSwing)
        {
            if (Input.GetKeyDown(Key.LEFT_CTRL))
            {
                swinging = true;
                whipSprite.scaleX = 3f;
                scaleX = 0.65f;
                whipSprite.visible = true;
                whipSprite.rotation = -55f;
                flyToBorder = true;
            }

            if (Input.GetKeyUp(Key.LEFT_CTRL))
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
        if (!swinging)
        {
            foreach (GameObject g in _collider1.GetCollisions())
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
                    RespawnPlayer1();
                }

                if (g is InkaWhip)
                {
                    _inkaWhip = g as InkaWhip;
                    Console.WriteLine("player 1 stunned");
                    if (_inkaWhip.visible)
                    {
                        y -= 50;
                        HandleStunnedAnimation();
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
            gameOverSound.Play();
            gameOver = true;
            LateDestroy();
        }
    }

    #endregion
}