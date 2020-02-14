using System;
using GXPEngine;
using GXPEngine.Core;

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
    private Pickup[] _coinArray;

    //Platforms
    private StartPlatform _startPlatform1;
    private StartPlatform _startPlatform2;

    private NormalPlatform[] _platformArray;
    private FallingPlatform[] _fallingPlatformArray;
    private FakePlatform[] _fakePlatformArray;

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

    private void InitializePlatforms()
    {
        //Normal platforms
        _platformArray = new NormalPlatform[6];
        for (int count = 0; count < _platformArray.Length; count++)
        {
            if (_platformArray[count] == null)
            {
                _platformArray[count] = new NormalPlatform
                {
                    y = Utils.Random(100, 800),
                };
                AddChild(_platformArray[count]);
                _platformArray[count].x += 100;
            }
        }

        ////Falling platforms
        _fallingPlatformArray = new FallingPlatform[6];
        for (int count = 0; count < _fallingPlatformArray.Length; count++)
        {
            if (_fallingPlatformArray[count] == null)
            {
                _fallingPlatformArray[count] = new FallingPlatform
                {
                    y = Utils.Random(100, 800),
                };
                AddChild(_fallingPlatformArray[count]);
            }
        }

        //Fake platforms
        _fakePlatformArray = new FakePlatform[6];
        for (int count = 0; count < _fakePlatformArray.Length; count++)
        {
            if (_fakePlatformArray[count] == null)
            {
                _fakePlatformArray[count] = new FakePlatform
                {
                    y = Utils.Random(100, 800),
                };
                AddChild(_fakePlatformArray[count]);
            }
        }

        _startPlatform1 = new StartPlatform();
        AddChild(_startPlatform1);
        _startPlatform1.y = 300;

        _startPlatform2 = new StartPlatform();
        AddChild(_startPlatform2);
        _startPlatform2.y = 700;
    }

    private void InitializeCoins()
    {
        _coinArray = new Pickup[5];
        for (int count = 0; count < _coinArray.Length; count++)
        {
            if (_coinArray[count] == null)
            {
                _coinArray[count] = new Pickup
                {
                    x = Utils.Random(0, game.width),
                    y = Utils.Random(100, 800),
                };
                AddChild(_coinArray[count]);
            }
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
            Destroy();
        }
        else
        {
            resetGame = false;
        }
    }
}
