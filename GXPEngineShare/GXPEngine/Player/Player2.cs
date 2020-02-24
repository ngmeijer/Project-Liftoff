using GXPEngine;

public class Player2 : AnimationSprite
{
    #region Variables

    private float _moveSpeed = 6f;
    private float _jumpForce = 18f;
    private float _fallMultiplier = 7.5f;
    private float _gravity;
    private float _defaultGravity = 7.5f;
    private float _whipGravity = 0f;
    private bool _isJumping = false;
    private int jumpCount = 0;

    public StartPlatform _startPlatform { get; set; }
    public MovingPlatform _normalPlatform { get; private set; }
    private CrumblingPlatform _fallingPlatform;

    private Sprite _collider2;
    private Spears _spears;

    public InkaWhip whipSprite { get; private set; }

    private Level levelScript;
    private HUD hudScript;

    private float speedY;
    private const float spawnPointX = 100;
    private const float spawnPointY = 500;

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
    private bool stunned;

    public bool flyToBorder { get; set; }

    public int whipUsedCount { get; private set; }

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

        lifeCount = 3;
        _animationSpeed = 150f;
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
        if (!stunned)
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

    private void PlayerJump()
    {
        y = y + speedY;

        if (speedY <= _fallMultiplier)
        {
            speedY = speedY + 1;
            HandleJumpAnimation();
        }

        if (!stunned)
        {
            if (Input.GetKey(Key.UP) && (jumpCount < 2))
            {
                jumpCount += 1;
                speedY = -_jumpForce;
                HandleJumpAnimation();
            }
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
            if (Input.GetKeyDown(Key.PLUS))
            {
                _gravity = _whipGravity;
                usingWhip = true;
                whipSprite.visible = true;
            }

            if (Input.GetKeyUp(Key.PLUS))
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
        if (!usingWhip)
        {
            foreach (GameObject g in _collider2.GetCollisions())
            {
                if (g is StartPlatform)
                {
                    jumpCount = 0;
                    _startPlatform = g as StartPlatform;
                    y = _startPlatform.y - 55;
                }

                if (g is MovingPlatform)
                {
                    _normalPlatform = g as MovingPlatform;
                    jumpCount = 0;
                    y = _normalPlatform.y - 65;
                }

                if (g is CrumblingPlatform)
                {
                    _fallingPlatform = g as CrumblingPlatform;
                    jumpCount = 0;
                    y = _fallingPlatform.y - 60;
                    _fallingPlatform.handleCrumbleAnimation();
                }

                if (g is Spears)
                {
                    _spears = g as Spears;
                    playerHasDied = true;
                    RespawnPlayer2();
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
            LateDestroy();
        }
    }

    #endregion
}