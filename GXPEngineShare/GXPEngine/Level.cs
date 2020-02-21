using System;
using GXPEngine;

public class Level : GameObject
{
    #region Variables

    //Players
    public Player1 _player1 { get; set; }
    public Player2 _player2 { get; set; }

    private Background _background;
    private Background _background2;

    public HUD hud { get; private set; }
    private Menu menu;
    public bool resetGame;

    public ScreenBorders borders;
    private Spears spears;

    public bool duo { get; set; }

    //Pickups
    private Pickup[] _pickupArray;

    //Platforms
    private StartPlatform _startPlatform1;
    private StartPlatform _startPlatform2;

    public MovingPlatform[] _movingPlatformArray;
    public CrumblingPlatform[] _crumblingPlatformArray;
    private CrumblingPlatform[] _fallingPlatformArray2;
    public NormalPlatform[] _normalPlatformArray;

    private int[] fallingPlatformPosX = { 600, 800, 1000, 1200, 1400, 1600, 1800 };
    private int[] fallingPlatformPosY = { 150, 300, 450, 600, 750, 800, 950 };

    private int fallingCount = 0;

    private int xPosNormal = 800;
    private int yPosNormal = 300;

    private int xPosFalling = 500;
    private int yPosFalling = 200;
    private int xPosFalling2 = 500;
    private int yPosFalling2 = 800;
    private int xPosFalling3 = 700;

    private int xPosFake = 1200;
    private int yPosFake = 100;

    private int xPosPickups = 600;
    private int yPosPickups = 100;

    //SFX
    private Sound _backgroundMusic;
    private CrumblingPlatform[] _fallingPlatformArray3;
    private Sprite _hudBackground;

    public int sceneTime { get; set; }

    #endregion

    public Level(Menu menuScript)
    {
        menu = menuScript;

        _backgroundMusic = new Sound("LevelTheme.mp3", true, true);
        _backgroundMusic.Play(false);

        InitializeBackground();

        _hudBackground = new Sprite("HUDBackground.png");
        AddChild(_hudBackground);
        _hudBackground.y = 0;

        InitializeFallingPlatforms();
        InitializePlatforms();
        InitializePlayers();
        InitializePickups();
        InitializeHUD();

        borders = new ScreenBorders();
        AddChild(borders);
        borders.x = game.width - 50;

        spears = new Spears();
        AddChild(spears);
        spears.y = 920;
    }

    private void Update()
    {
        sceneTime++;
        SpawnNewFallingPlatforms();
    }

    #region Draw level
    private void InitializeBackground()
    {
        _background = new Background();
        _background2 = new Background();
        AddChild(_background);
        AddChild(_background2);

        _background2.x = game.width;
    }

    private void InitializePlayers()
    {
        _player1 = new Player1(100, 200, hud, this);
        AddChild(_player1);

        if (menu.duoPlayers)
        {
            _player2 = new Player2(100, 500, hud, this);
            AddChild(_player2);
        }
    }

    private void InitializeFallingPlatforms()
    {
        _crumblingPlatformArray = new CrumblingPlatform[5];

        for (fallingCount = 0; fallingCount < _crumblingPlatformArray.Length; fallingCount++)
        {
            _crumblingPlatformArray[fallingCount] = new CrumblingPlatform();
            _crumblingPlatformArray[fallingCount].SetSpawnPosition(xPosFalling, yPosFalling);
            xPosFalling += 310;

            AddChild(_crumblingPlatformArray[fallingCount]);
        }

        _fallingPlatformArray2 = new CrumblingPlatform[5];

        for (int count = 0; count < _fallingPlatformArray2.Length; count++)
        {
            _fallingPlatformArray2[count] = new CrumblingPlatform();
            _fallingPlatformArray2[count].SetSpawnPosition(xPosFalling2, yPosFalling2);
            xPosFalling2 += 310;

            AddChild(_fallingPlatformArray2[count]);
        }
    }

    private void InitializePlatforms()
    {
        //Normal platforms
        _movingPlatformArray = new MovingPlatform[7];

        for (int count = 0; count < _movingPlatformArray.Length; count++)
        {
            _movingPlatformArray[count] = new MovingPlatform(this, menu);
            _movingPlatformArray[count].SetSpawnPosition(xPosNormal, yPosNormal);
            xPosNormal += Utils.Random(150, 250);
            yPosNormal += Utils.Random(50, 150);

            AddChild(_movingPlatformArray[count]);
        }

        //Fake platforms
        _normalPlatformArray = new NormalPlatform[7];

        for (int count = 0; count < _normalPlatformArray.Length; count++)
        {
            _normalPlatformArray[count] = new NormalPlatform();
            _normalPlatformArray[count].SetSpawnPosition(xPosFake, yPosFake);
            xPosFake += 200;
            yPosFake += 100;

            AddChild(_normalPlatformArray[count]);
        }

        //Start platforms. Clean this up.
        _startPlatform1 = new StartPlatform(this);
        AddChild(_startPlatform1);
        _startPlatform1.y = 300;

        _startPlatform2 = new StartPlatform(this);
        AddChild(_startPlatform2);
        _startPlatform2.y = 700;
    }

    private void InitializePickups()
    {
        _pickupArray = new Pickup[4];

        for (int count = 0; count < _pickupArray.Length; count++)
        {
            int randomFirstPositionX = Utils.Random(600, 1800);
            int randomFirstPositionY = Utils.Random(200, 900);

            _pickupArray[count] = new Pickup(this);
            _pickupArray[count].SetSpawnPosition(randomFirstPositionX, randomFirstPositionY);
            AddChild(_pickupArray[count]);
        }
    }

    private void InitializeHUD()
    {
        hud = new HUD(_player1, _player2);
        AddChild(hud);
    }

    #endregion

    private void CheckGameReset()
    {
        if (_player1.lifeCount <= 0)
        {
            resetGame = true;
            _player1 = null;
            Destroy();
        }
        else
        {
            resetGame = false;
        }
    }

    private void SpawnNewFallingPlatforms()
    {
        if (sceneTime > 650)
        {
            _crumblingPlatformArray = new CrumblingPlatform[5];

            for (int count = 0; count < _crumblingPlatformArray.Length; count++)
            {
                _crumblingPlatformArray[count] = new CrumblingPlatform();
                _crumblingPlatformArray[count].SetSpawnPosition(xPosFalling3, yPosFalling);
                xPosFalling3 += 200;

                AddChild(_crumblingPlatformArray[count]);
                sceneTime = 0;
            }
        }
    }
}
