using GXPEngine;

public class Level : GameObject
{
    #region Variables

    //Players
    private Player1 _player1;
    private Player2 _player2;

    private Background _background;
    private Background _background2;

    private HUD hud;
    public bool resetGame;

    //Pickups
    private Pickup[] _pickupArray;

    //Platforms
    private StartPlatform _startPlatform1;
    private StartPlatform _startPlatform2;

    private NormalPlatform[] _platformArray;
    private FallingPlatform[] _fallingPlatformArray;
    private FakePlatform[] _fakePlatformArray;

    private float xPosNormal = 800;
    private float yPosNormal = 100;

    private float xPosFalling = 1000;
    private float yPosFalling = 100;

    private float xPosFake = 1200;
    private float yPosFake = 100;

    private float xPosCoins = 600;
    private float yPosCoins = 100;

    //SFX
    private Sound _backgroundMusic;

    #endregion

    public Level()
    {
        _backgroundMusic = new Sound("LevelTheme.mp3", true, true);
        _backgroundMusic.Play(false);

        InitializeBackground();
        InitializePlayers();
        InitializeHUD();
        InitializePlatforms();
        InitializeCoins();
        CheckGameReset();
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
        _player1 = new Player1(100, 200);
        AddChild(_player1);

        _player2 = new Player2(100, 500);
        AddChild(_player2);
    }

    /// <summary>
    /// 
    /// </summary>
    private void InitializePlatforms()
    {
        //Normal platforms
        _platformArray = new NormalPlatform[7];

        for (int count = 0; count < _platformArray.Length; count++)
        {
            _platformArray[count] = new NormalPlatform();
            _platformArray[count].SetSpawnPosition(xPosNormal, yPosNormal);
            xPosNormal += 200;
            yPosNormal += 100;

            AddChild(_platformArray[count]);
        }

        //Falling platforms
        _fallingPlatformArray = new FallingPlatform[7];

        for (int count = 0; count < _fallingPlatformArray.Length; count++)
        {
            _fallingPlatformArray[count] = new FallingPlatform();
            _fallingPlatformArray[count].SetSpawnPosition(xPosFalling, yPosFalling);
            xPosFalling += 200;
            yPosFalling += 100;

            AddChild(_fallingPlatformArray[count]);
        }

        //Fake platforms
        _fakePlatformArray = new FakePlatform[7];

        for (int count = 0; count < _fakePlatformArray.Length; count++)
        {
            _fakePlatformArray[count] = new FakePlatform();
            _fakePlatformArray[count].SetSpawnPosition(xPosFake, yPosFake);
            xPosFake += 200;
            yPosFake += 100;

            AddChild(_fakePlatformArray[count]);
        }

        //Start platforms. Clean this up.
        _startPlatform1 = new StartPlatform();
        AddChild(_startPlatform1);

        _startPlatform1.y = 300;

        _startPlatform2 = new StartPlatform();
        AddChild(_startPlatform2);
        _startPlatform2.y = 700;
    }

    private void InitializeCoins()
    {
        _pickupArray = new Pickup[5];

        for (int count = 0; count < _pickupArray.Length; count++)
        {
            _pickupArray[count] = new Pickup();
            _pickupArray[count].SetSpawnPosition(xPosCoins, yPosCoins);
            xPosCoins += _pickupArray[count].offsetX;
            yPosCoins += _pickupArray[count].offsetY;

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
}
