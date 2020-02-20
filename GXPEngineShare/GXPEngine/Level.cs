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

    public NormalPlatform[] _platformArray;
    public FallingPlatform[] _fallingPlatformArray;
    private FallingPlatform[] _fallingPlatformArray2;
    public FakePlatform[] _fakePlatformArray;

    private int[] fallingPlatformPosX = { 600, 800, 1000, 1200, 1400, 1600, 1800 };
    private int[] fallingPlatformPosY = { 150, 300, 450, 600, 750, 800, 950 };

    private int fallingCount = 0;

    private float xPosNormal = 800;
    private float yPosNormal = 300;

    private float xPosFalling = 500;
    private float yPosFalling = 200;
    private float xPosFalling2 = 500;
    private float yPosFalling2 = 800;
    private float xPosFalling3 = 700;

    private float xPosFake = 1200;
    private float yPosFake = 100;

    private float xPosPickups = 600;
    private float yPosPickups = 100;

    //SFX
    private Sound _backgroundMusic;
    private FallingPlatform[] _fallingPlatformArray3;
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
        InitializeCoins();
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
        //SpawnNewFallingPlatforms();
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
        _fallingPlatformArray = new FallingPlatform[7];

        for (fallingCount = 0; fallingCount < _fallingPlatformArray.Length; fallingCount++)
        {
            _fallingPlatformArray[fallingCount] = new FallingPlatform();
            _fallingPlatformArray[fallingCount].SetSpawnPosition(xPosFalling, yPosFalling);
            xPosFalling += 200;

            AddChild(_fallingPlatformArray[fallingCount]);
        }

        _fallingPlatformArray2 = new FallingPlatform[7];

        for (int count = 0; count < _fallingPlatformArray2.Length; count++)
        {
            _fallingPlatformArray2[count] = new FallingPlatform();
            _fallingPlatformArray2[count].SetSpawnPosition(xPosFalling2, yPosFalling2);
            xPosFalling2 += 200;

            AddChild(_fallingPlatformArray2[count]);
        }
    }

    private void InitializePlatforms()
    {
        //Normal platforms
        _platformArray = new NormalPlatform[7];

        for (int count = 0; count < _platformArray.Length; count++)
        {
            _platformArray[count] = new NormalPlatform(this, menu);
            _platformArray[count].SetSpawnPosition(xPosNormal, yPosNormal);
            xPosNormal += Utils.Random(150, 250);
            yPosNormal += Utils.Random(50, 150);

            AddChild(_platformArray[count]);
        }

        //Falling platforms

        //Fake platforms
        //_fakePlatformArray = new FakePlatform[7];

        //for (int count = 0; count < _fakePlatformArray.Length; count++)
        //{
        //    _fakePlatformArray[count] = new FakePlatform();
        //    _fakePlatformArray[count].SetSpawnPosition(xPosFake, yPosFake);
        //    xPosFake += 200;
        //    yPosFake += 100;

        //    AddChild(_fakePlatformArray[count]);
        //}

        //Start platforms. Clean this up.
        _startPlatform1 = new StartPlatform(this);
        AddChild(_startPlatform1);
        _startPlatform1.y = 300;

        _startPlatform2 = new StartPlatform(this);
        AddChild(_startPlatform2);
        _startPlatform2.y = 700;
    }

    private void InitializeCoins()
    {
        _pickupArray = new Pickup[5];

        for (int count = 0; count < _pickupArray.Length; count++)
        {
            _pickupArray[count] = new Pickup(this);
            _pickupArray[count].SetSpawnPosition(xPosPickups, yPosPickups);
            xPosPickups += _pickupArray[count].offsetX;
            yPosPickups += _pickupArray[count].offsetY;

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

    //private void SpawnNewFallingPlatforms()
    //{
    //    if (sceneTime > 650)
    //    {
    //        _fallingPlatformArray = new FallingPlatform[5];

    //        for (int count = 0; count < _fallingPlatformArray.Length; count++)
    //        {
    //            _fallingPlatformArray[count] = new FallingPlatform();
    //            _fallingPlatformArray[count].SetSpawnPosition(xPosFalling3, yPosFalling);
    //            xPosFalling3 += 200;

    //            AddChild(_fallingPlatformArray[count]);
    //            sceneTime = 0;
    //        }
    //    }
    //}
}
